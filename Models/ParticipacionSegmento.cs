using System;

namespace AppMexaba.Models
{
    public class ParticipacionSegmento
    {
        public DateTime Fecha { get; set; }
        public string Segmento { get; set; }
        public decimal TotalVentas { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
