using AppMexaba.Models;

namespace AppMexaba.Services
{
    public interface ICatalogoService
    {
        Task<List<ArticuloComprador>> ObtenerArticulosAsync(string busqueda);
        Task<ArticuloComprador?> ObtenerArticuloPorIdAsync(string id);
        Task<bool> CrearArticuloAsync(ArticuloComprador model);
        Task<bool> EditarArticuloAsync(ArticuloComprador model);
        Task<List<object>> BuscarArticulosAsync(string busqueda);
        Task<List<object>> BuscarCompradoresAsync(string busqueda);
    }
}
