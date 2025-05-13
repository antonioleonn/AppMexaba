namespace AppMexaba.Models
{
    public class VentasPorDiaTipoVenta
    {
        public string TipoVenta { get; set; }
        public DateTime Fecha { get; set; }
        public string CodBarras { get; set; }
        public decimal TotalVentas { get; set; }
    }
}
