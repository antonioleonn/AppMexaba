using AppMexaba.Dtos;
using AppMexaba.Models;
using AppMexaba.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppMexaba.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ICatalogoService _catalogoService;
        private const int PageSize = 50;

        public CatalogoController(ICatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        public async Task<IActionResult> Index(string busqueda, int pagina = 1)
        {
            var query = await _catalogoService.ObtenerArticulosAsync(busqueda);

            var totalRegistros = query.Count;
            var articulos = query.Skip((pagina - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.Total = totalRegistros;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / PageSize);
            ViewBag.Busqueda = busqueda;

            ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Error = TempData["Error"];

            return View(articulos);
        }

        public IActionResult Crear()
        {
            return View(new ArticuloComprador());
        }


        [HttpPost]
        public async Task<IActionResult> Crear(ArticuloComprador model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos en el formulario.";
                return View(model);
            }

            try
            {
                var exito = await _catalogoService.CrearArticuloAsync(model);
                if (exito)
                {
                    TempData["Mensaje"] = "Artículo creado correctamente.";
                    return RedirectToAction("Index");
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado al crear el artículo.";
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Editar(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var item = await _catalogoService.ObtenerArticuloPorIdAsync(id);
            if (item == null)
                return NotFound();

            return View("Crear", item);
        }


        [HttpPost]
        public async Task<IActionResult> Editar(ArticuloComprador model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos en el formulario.";
                return View("Crear", model);
            }

            try
            {
                var exito = await _catalogoService.EditarArticuloAsync(model);
                if (exito)
                {
                    TempData["Mensaje"] = "Artículo actualizado correctamente.";
                    return RedirectToAction("Index");
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado al actualizar el artículo.";
            }

            return View("Crear", model);
        }


        [HttpGet("/api/catalogo/buscar-articulos")]
        public async Task<IActionResult> BuscarArticulos(string q)
        {
            var resultados = await _catalogoService.BuscarArticulosAsync(q);
            return Json(new { results = resultados });
        }

        [HttpGet("/api/catalogo/buscar-compradores")]
        public async Task<IActionResult> BuscarCompradores(string q)
        {
            var resultados = await _catalogoService.BuscarCompradoresAsync(q);
            return Json(new { results = resultados });
        }
    }
}
