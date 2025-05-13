using System;

namespace AppMexaba.Models

{
    public class VentasPorSucursal
    {
        public DateTime Fecha { get; set; }
        public string Sucursal { get; set; }
        public decimal TotalVentas { get; set; }
    }
}
