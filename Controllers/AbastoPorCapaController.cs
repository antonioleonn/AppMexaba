using Microsoft.AspNetCore.Mvc;
using AppMexaba.Servicios;
using AppMexaba.Models;
using AppMexaba.Services;

namespace AppMexaba.Controllers
{
    public class AbastoPorCapaController : Controller
    {
        private readonly IAbastoPorCapaService _service;

        public AbastoPorCapaController(IAbastoPorCapaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var sucursales = await _service.ObtenerSucursales();
            return View(sucursales);
        }

        public async Task<IActionResult> Detalle(string sucursal)
        {
            var resultado = await _service.ObtenerDetallePorSucursal(sucursal);
            ViewBag.Sucursal = sucursal;
            return View(resultado);
        }


        [HttpGet]
        public async Task<IActionResult> Resumen(string sucursal)
        {
            var datos = await _service.ObtenerDetallePorSucursal(sucursal);

            var resumen = datos
                .GroupBy(x => x.Capas)
                .Select(g => new {
                    capa = g.Key,
                    cantidad = g.Count()
                });

            return Json(resumen);
        }

    }
}
