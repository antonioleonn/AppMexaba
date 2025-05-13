using AppMexaba.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMexaba.Services
{
    public interface IOrigenService
    {
        Task InsertarOrigenesAsync(List<alm_art> origenes);
        Task<List<object>> BuscarArticulosAsync(string busqueda);
        Task<List<alm_art>> ObtenerOrigenesAsync();
        Task<alm_art> ObtenerOrigenAsync(string alm, string salm, string art);
        Task<string> ObtenerDescripcionArticuloAsync(string art);
        Task ActualizarOrigenesAsync(List<alm_art> origenes);
        Task<List<alm_art>> ObtenerOrigenesPorArticuloAsync(string art);


    }
}
