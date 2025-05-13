namespace AppMexaba.Models
{
    public class ResumenCliente
    {
        public string CodBarras { get; set; }
        public string NombreCliente { get; set; }
        public decimal TotalArticulos { get; set; }
        public decimal TotalCompras { get; set; }
        public int TotalComprasDistintas { get; set; }
    }

}
