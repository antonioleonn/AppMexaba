using AppMexaba.Models;  // Referencia al modelo que ahora está en Models
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AppMexaba.Controllers
{
    public class SucursalesController : Controller
    {
        private static readonly Dictionary<string, List<Sucursal>> SucursalesPorSeccion = new()
        {
            { "OFC", new List<Sucursal>
                {
                    new Sucursal("Sucursal OFC = Oficina Central", "192.168.1.181", "192.168.1.2")
                }
            },
            { "Mostradores 7.6", new List<Sucursal>
                {
                    new Sucursal("Sucursal ABE = Acayucan Bebidas", "192.168.7.203", "192.168.7.254"),
                    new Sucursal("Sucursal ACT = Acatlan", "192.168.30.200", "192.168.30.254"),
                    new Sucursal("Sucursal CAB = Cabada", "192.168.27.200", "192.168.27.254"),
                    new Sucursal("Sucursal CER = Cerritos", "192.168.19.200", "192.168.19.254"),
                    new Sucursal("Sucursal CMA = Córdoba CMA", "192.168.15.201", "192.168.15.254"),
                    new Sucursal("Sucursal COM = Córdoba COM", "192.168.15.200", "192.168.15.254"),
                    new Sucursal("Sucursal LOM = Loma Bonita", "192.168.35.200", "192.168.35.254"),
                    new Sucursal("Sucursal TRV = Tres Valles", "192.168.32.200", "192.168.32.254"),
                    new Sucursal("Sucursal ZAB = Zapata Bebidas", "192.168.70.200", "192.168.70.254"),
                    new Sucursal("Sucursal ZOB = Zongolica Bebidas", "192.168.24.200", "192.168.24.254")
                }
            },
            { "Mostradores 7.9", new List<Sucursal>
                {
                    new Sucursal("Sucursal ACA = Acayucan Madre", "192.168.7.200", "192.168.7.254"),
                    new Sucursal("Sucursal MIB = Minatitlan Bebidas", "192.168.11.202", "192.168.11.254"),
                    new Sucursal("Sucursal MIN = Minatitlan Madre", "192.168.11.200", "192.168.11.254"),
                    new Sucursal("Sucursal ORI = Orizaba Madre", "192.168.18.200", "192.168.18.254"),
                    new Sucursal("Sucursal SAB = San Andres Bebidas", "192.168.12.200", "192.168.12.254"),
                    new Sucursal("Sucursal SAN = San Andres", "192.168.25.200", "192.168.25.254"),
                    new Sucursal("Sucursal TBC = Tierra Blanca Madre", "192.168.31.200", "192.168.31.254"),
                    new Sucursal("Sucursal TBE = Tierra Blanca Bebidas", "192.168.31.202", "192.168.31.254"),
                    new Sucursal("Sucursal TEB = Tezonapa Bebidas", "192.168.29.200", "192.168.29.254"),
                    new Sucursal("Sucursal TEZ = Tezonapa", "192.168.28.200", "192.168.28.254"),
                    new Sucursal("Sucursal TUX = Tuxtepec Madre", "192.168.34.200", "192.168.34.254"),
                    new Sucursal("Sucursal XAC = Xalapa XAC", "192.168.36.200", "192.168.36.254"),
                    new Sucursal("Sucursal XAL = Xalapa XAL", "192.168.37.200", "192.168.37.254"),
                    new Sucursal("Sucursal ZAM = Zapata Mayoreo", "192.168.21.200", "192.168.21.254")
                }
            },
            { "Cedis", new List<Sucursal>
                {
                    new Sucursal("CEC Cedis Cordoba", "192.168.13.208", "192.168.13.254"),
                    new Sucursal("CMI Cedis Minatitlan", "192.168.39.200", "192.168.39.254")
                }
            },
            { "Retail", new List<Sucursal>
                {
                    new Sucursal("Sucursal ACC = Alvarado", "192.168.40.200", "192.168.40.254")
                    {
                        Cajas = new List<Caja>
                        {
                            new Caja("Caja01", "192.168.40.31"),
                            new Caja("Caja03", "192.168.40.35")
                        }
                    },
                    new Sucursal("Sucursal CCC = Córdoba", "192.168.13.202", "192.168.13.254")
                    {
                        Cajas = new List<Caja>
                        {
                            new Caja("Caja01", "192.168.13.210"),
                            new Caja("Caja02", "192.168.13.211"),
                            new Caja("Caja03", "192.168.13.212"),
                            new Caja("Caja04", "192.168.13.213")
                        }
                    },
                    new Sucursal("Sucursal ICC = Isla", "192.168.10.200", "192.168.10.254")
                    {
                        Cajas = new List<Caja>
                        {
                            new Caja("Caja01", "192.168.10.23"),
                            new Caja("Caja02", "192.168.10.21"),
                            new Caja("Caja03", "192.168.10.22"),
                            new Caja("Caja04", "192.168.10.25")
                        }
                    },
                    new Sucursal("Sucursal XCC = Xalapa", "192.168.38.200", "192.168.38.254")
                    {
                        Cajas = new List<Caja>
                        {
                            new Caja("Caja01", "192.168.38.11"),
                            new Caja("Caja02", "192.168.38.12")
                        }
                    },
                    new Sucursal("Sucursal UCC = Cuitlahuac", "192.168.17.200", "192.168.17.254")
                    {
                        Cajas = new List<Caja>
                        {
                            new Caja("Caja01", "192.168.17.21"),
                            new Caja("Caja02", "192.168.17.22")
                        }
                    },
                    new Sucursal("Sucursal VCC = Veracruz", "192.168.3.200", "192.168.3.254")
                    {
                        Cajas = new List<Caja>
                        {
                            new Caja("Caja04", "192.168.3.211"),
                            new Caja("Caja02", "192.168.3.212"),
                            new Caja("Caja03", "192.168.3.213")
                        }
                    },
                    new Sucursal("Sucursal ZOC = Zapata", "192.168.9.201", "192.168.9.254")
                    {
                        Cajas = new List<Caja>
                        {
                            new Caja("Caja01", "192.168.9.210"),
                            new Caja("Caja02", "192.168.9.212")
                        }
                    }
                }
            }
        };

        public async Task<IActionResult> Index()
        {
            foreach (var seccion in SucursalesPorSeccion.Values)
            {
                foreach (var sucursal in seccion)
                {
                    await sucursal.VerificarEstadoAsync();
                }
            }
            return View(SucursalesPorSeccion);
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar()
        {
            foreach (var seccion in SucursalesPorSeccion.Values)
            {
                foreach (var sucursal in seccion)
                {
                    await sucursal.VerificarEstadoAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
