using AppMexaba.Dtos;
using AppMexaba.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AppMexaba.Controllers
{
    // Controllers/VentasVipController.cs
    public class VentasVipController : Controller
    {
        private readonly VentasVipService _service;

        public VentasVipController(VentasVipService service)
        {
            _service = service;
        }

        // ✔ Acción principal que agrupa por sucursal y pasa a la vista
        public async Task<IActionResult> Index()
        {
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            var datos = await _service.ObtenerVentasVip("", fecha, "");
            var sucursales = datos
                .GroupBy(d => d.SUCURSAL)
                .Select(g => new SucursalResumenDto
                {
                    Sucursal = g.Key,
                    Total = g.Count(),
                    Cumplen = g.Count(x => x.ESTATUS == "CUMPLE"),
                    NoCumplen = g.Count(x => x.ESTATUS == "NO CUMPLE")
                }).ToList();

            ViewBag.Detalles = datos;
            return View(sucursales); // OK
        }


        // ✔ Acción AJAX para tooltip al hacer hover
        public async Task<IActionResult> ResumenSucursal(string suc)
        {
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            var datos = await _service.ObtenerVentasVip(suc, fecha, "");

            var resumen = new
            {
                sucursal = suc,
                total = datos.Count,
                cumplen = datos.Count(x => x.ESTATUS == "CUMPLE"),
                noCumplen = datos.Count(x => x.ESTATUS == "NO CUMPLE")
            };

            return Json(resumen); // OK
        }


        // ✔ Acción que carga la vista de detalle
        public async Task<IActionResult> DetalleSucursal(string suc)
        {
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            var datos = await _service.ObtenerVentasVip(suc, fecha, "");
            ViewBag.Sucursal = suc;
            return View("~/Views/VentasVip/DetalleSucursal.cshtml", datos); 
        }

        // GET: /VentasVip/EditarWhatsApp
        public async Task<IActionResult> EditarWhatsApp(string cliente)
        {
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            var datos = await _service.ObtenerVentasVip("", fecha, "");
            var modelo = datos.FirstOrDefault(x => x.CLIENTE == cliente);

            if (modelo == null)
                return NotFound();

            return View("EditarWhatsApp", modelo);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarWhatsApp(string CLIENTE, string WHATSAPP)
        {
            var exito = await _service.ActualizarWhatsAppCliente(CLIENTE, WHATSAPP);
            var sucursal = await _service.ObtenerSucursalPorCliente(CLIENTE);

            if (exito)
            {
                ViewBag.Cliente = CLIENTE;
                ViewBag.Sucursal = sucursal;
                return View("ConfirmacionWhatsApp");
            }
            else
            {
                TempData["Error"] = "Error al actualizar el WhatsApp del cliente.";
                return RedirectToAction("DetalleSucursal", new { suc = sucursal });
            }
        }





    }

}
