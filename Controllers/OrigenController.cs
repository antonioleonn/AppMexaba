using AppMexaba.Models;
using AppMexaba.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMexaba.Controllers
{
    public class OrigenController : Controller
    {
        private readonly IOrigenService _origenService;

        public OrigenController(IOrigenService origenService)
        {
            _origenService = origenService;
        }

        public async Task<IActionResult> Index(int page = 1, string busqueda = "")
        {
            const int pageSize = 50;
            var lista = await _origenService.ObtenerOrigenesAsync();

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                lista = lista.Where(x =>
                    x.art.Contains(busqueda, StringComparison.OrdinalIgnoreCase) ||
                    x.alm.Contains(busqueda, StringComparison.OrdinalIgnoreCase) ||
                    x.salm.Contains(busqueda, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            int total = lista.Count;
            int totalPages = (int)Math.Ceiling((double)total / pageSize);
            var pagedData = lista.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Total = total;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Busqueda = busqueda;

            return View(pagedData);
        }

        [HttpPost]
        [Route("/Origen/Actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] List<alm_art> origenes)
        {
            if (origenes == null || origenes.Count == 0)
                return BadRequest("No se recibieron datos");

            try
            {
                await _origenService.ActualizarOrigenesAsync(origenes);
                TempData["Mensaje"] = "Artículo actualizado correctamente";
                return Json(new { redireccion = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar: {ex.Message}");
            }
        }




        public IActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        [Route("/Origen/BuscarArticulos")]
        public async Task<IActionResult> BuscarArticulos([FromQuery] string q)
        {
            var resultados = await _origenService.BuscarArticulosAsync(q);
            return Json(new { results = resultados });
        }

        public async Task<IActionResult> Editar(string art)
        {
            var origenes = await _origenService.ObtenerOrigenesPorArticuloAsync(art);
            if (origenes == null || origenes.Count == 0)
                return NotFound();

            string descripcion = await _origenService.ObtenerDescripcionArticuloAsync(art);
            ViewBag.Descripcion = descripcion;
            ViewBag.Art = art;

            return View("Editar", origenes);
        }









        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] List<alm_art> origenes)
        {
            if (origenes == null || origenes.Count == 0)
                return BadRequest("No se recibieron datos");

            try
            {
                await _origenService.InsertarOrigenesAsync(origenes);
                return Ok(new { mensaje = "Guardado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el servicio: {ex.Message}");
            }
        }
    }
}
