using AppMexaba.Models;

namespace AppMexaba.Services
{
    public interface IVentaCFRService
    {
        Task<List<VentaCFR>> ObtenerDetallePorFecha(DateTime desde, DateTime hasta);
        Task<List<ResumenCliente>> ObtenerTotalesPorCliente(DateTime desde, DateTime hasta);
        Task<List<ResumenClienteArticulo>> ObtenerDetallePorClienteYArticulo(DateTime desde, DateTime hasta);
    }

}
