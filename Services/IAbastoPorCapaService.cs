using AppMexaba.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMexaba.Services
{
    public interface IAbastoPorCapaService
    {
        Task<List<string>> ObtenerSucursales();
        Task<List<AbastoCapaDto>> ObtenerDetallePorSucursal(string sucursal);
    }
}
