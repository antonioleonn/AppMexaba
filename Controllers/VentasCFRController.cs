using AppMexaba.Models;
using AppMexaba.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace AppMexaba.Controllers
{
    public class VentasCFRController : Controller
    {
        private readonly IVentaCFRService _service;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        private static readonly HashSet<string> SucursalesCFR = new HashSet<string>
        {
            "ACC", "CCC", "ICC", "ORI", "TBC", "XCC", "UCC", "VCC", "ZOC"
        };

        public VentasCFRController(IVentaCFRService service, AppDbContext context, IConfiguration configuration)
        {
            _service = service;
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(DateTime? fecha, DateTime? desde, DateTime? hasta, int? mes, int? anio, string sucursal = null, string tipoCliente = null)
        {
            DateTime fechaInicio, fechaFin;

            // Prioridad de fechas
            if (desde.HasValue && hasta.HasValue)
            {
                fechaInicio = desde.Value.Date;
                fechaFin = hasta.Value.Date;
            }
            else if (mes.HasValue && anio.HasValue)
            {
                fechaInicio = new DateTime(anio.Value, mes.Value, 1);
                fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            }
            else if (fecha.HasValue)
            {
                fechaInicio = fecha.Value.Date;
                fechaFin = fecha.Value.Date;
            }
            else
            {
                fechaInicio = DateTime.Today.AddDays(-1);
                fechaFin = fechaInicio;
            }

            var comparativaRaw = new List<VentasSocioVSucursal>();
            var connectionString = _configuration.GetConnectionString("ConexionSQL");

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("sp_VentasSocioVSucursalFiltrada", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 180;
                cmd.Parameters.AddWithValue("@Desde", fechaInicio);
                cmd.Parameters.AddWithValue("@Hasta", fechaFin);
                cmd.Parameters.AddWithValue("@Sucursal", (object)sucursal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TipoCliente", (object)tipoCliente ?? DBNull.Value);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var suc = reader.GetString(1);
                        if (string.IsNullOrEmpty(sucursal) && !SucursalesCFR.Contains(suc))
                            continue;

                        comparativaRaw.Add(new VentasSocioVSucursal
                        {
                            Fecha = reader.GetDateTime(0),
                            Sucursal = suc,
                            MexabaSocio = reader.GetDecimal(2),
                            VentasSucursal = reader.GetDecimal(3)
                        });
                    }
                }
            }

            // 🧠 Agrupamos por sucursal para evitar duplicados por fecha
            var comparativaAgrupada = comparativaRaw
                .GroupBy(x => x.Sucursal)
                .Select(g => new VentasSocioVSucursal
                {
                    Fecha = fechaInicio, // se usa solo como referencia
                    Sucursal = g.Key,
                    MexabaSocio = g.Sum(x => x.MexabaSocio),
                    VentasSucursal = g.Sum(x => x.VentasSucursal)
                })
                .OrderBy(x => x.Sucursal)
                .ToList();

            var cargarVistasCFR = tipoCliente != "N";

            var ventasSucursal = cargarVistasCFR
                ? await _context.VentasPorSucursal
                    .Where(f => f.Fecha >= fechaInicio && f.Fecha <= fechaFin)
                    .ToListAsync()
                : new List<VentasPorSucursal>();

            var ventasSegmento = cargarVistasCFR
                ? await _context.VentasPorSegmento
                    .Where(f => f.Fecha >= fechaInicio && f.Fecha <= fechaFin)
                    .ToListAsync()
                : new List<VentasPorSegmento>();

            var participacion = cargarVistasCFR
                ? await _context.ParticipacionSegmento
                    .Where(f => f.Fecha >= fechaInicio && f.Fecha <= fechaFin)
                    .ToListAsync()
                : new List<ParticipacionSegmento>();

            var totalVentas = ventasSucursal.Sum(x => x.TotalVentas);

            var vm = new DashboardVentasViewModel
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                FechaSeleccionada = fechaInicio,
                VentasSucursal = ventasSucursal,
                VentasSegmento = ventasSegmento,
                ComparativaSocioSucursal = comparativaAgrupada,
                ParticipacionSegmento = participacion,
                VentasPorDiaTipoVenta = new List<VentasPorDiaTipoVenta>(),
                TotalGeneralVentas = totalVentas
            };

            return View(vm);
        }

    }
}
