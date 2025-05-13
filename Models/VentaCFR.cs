namespace AppMexaba.Models
{
    public class VentaCFR
    {
        public string TipoVenta { get; set; }
        public string Sucursal { get; set; }
        public DateTime Fecha { get; set; }
        public string Caja { get; set; }
        public string NumTransaccion { get; set; }
        public string Estatus { get; set; }
        public string CodBarras { get; set; }
        public string NombreCliente { get; set; }
        public string Segmento { get; set; }
        public decimal Cantidad { get; set; }
        public string ClaveArticulo { get; set; }
        public string DescripcionArticulo { get; set; }
        public decimal Subtotal { get; set; }
        public decimal SubtotalConImp { get; set; }
    }
}
