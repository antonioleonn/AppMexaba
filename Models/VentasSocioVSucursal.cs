using System;

namespace AppMexaba.Models
{
    public class VentasSocioVSucursal
    {
        public DateTime Fecha { get; set; }
        public string Sucursal { get; set; }
        public decimal MexabaSocio { get; set; }
        public decimal VentasSucursal { get; set; }

        // Valor calculado (puede hacerse en el front o servicio si no se desea hacer en SQL)
        public decimal PorcentajeRegistrado =>
            VentasSucursal == 0 ? 0 : Math.Round(MexabaSocio * 100 / VentasSucursal, 2);
    }
}
