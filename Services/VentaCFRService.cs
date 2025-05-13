using AppMexaba.Models;
using Microsoft.EntityFrameworkCore;


namespace AppMexaba.Services
{
    public class VentaCFRService : IVentaCFRService
    {
        private readonly AppDbContext _context;

        public VentaCFRService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<VentaCFR>> ObtenerDetallePorFecha(DateTime desde, DateTime hasta)
        {
            return await _context.VentasClienteFrecuente
                .Where(v => v.Fecha >= desde && v.Fecha <= hasta)
                .OrderByDescending(v => v.Fecha)
                .Take(500)
                .ToListAsync();
        }


        public async Task<List<ResumenCliente>> ObtenerTotalesPorCliente(DateTime desde, DateTime hasta)
        {
            return await _context.VentasClienteFrecuente
                .Where(v => v.Fecha >= desde && v.Fecha <= hasta)
                .GroupBy(v => new { v.CodBarras, v.NombreCliente })
                .Select(g => new ResumenCliente
                {
                    CodBarras = g.Key.CodBarras,
                    NombreCliente = g.Key.NombreCliente,
                    TotalArticulos = g.Sum(x => x.Cantidad),
                    TotalCompras = g.Sum(x => x.SubtotalConImp),
                    TotalComprasDistintas = g.Select(x => x.NumTransaccion).Distinct().Count()
                })
                .OrderByDescending(x => x.TotalCompras)
                .Take(20)
                .ToListAsync();
        }

        public async Task<List<ResumenClienteArticulo>> ObtenerDetallePorClienteYArticulo(DateTime desde, DateTime hasta)
        {
            return await _context.VentasClienteFrecuente
                .Where(v => v.Fecha >= desde && v.Fecha <= hasta)
                .GroupBy(v => new { v.CodBarras, v.NombreCliente, v.ClaveArticulo, v.DescripcionArticulo })
                .Select(g => new ResumenClienteArticulo
                {
                    CodBarras = g.Key.CodBarras,
                    NombreCliente = g.Key.NombreCliente,
                    ClaveArticulo = g.Key.ClaveArticulo,
                    DescripcionArticulo = g.Key.DescripcionArticulo,
                    TotalArticulos = g.Sum(x => x.Cantidad),
                    TotalCompras = g.Sum(x => x.SubtotalConImp)
                })
                .OrderByDescending(x => x.TotalCompras)
                .ToListAsync();
        }
    }


}
