using System;

namespace AppMexaba.Models
{
    public class VentasPorSegmento
    {
        public DateTime Fecha { get; set; }
        public string Segmento { get; set; } = string.Empty;
        public decimal TotalVentas { get; set; }
    }
}
