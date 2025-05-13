namespace AppMexaba.Dtos
{
    // Dtos/VentasVipDto.cs
    public class VentasVipDto
    {
        public string SUCURSAL { get; set; }
        public string VENDEDOR { get; set; }
        public string NOMBRE_VENDEDOR { get; set; }
        public string CLIENTE { get; set; }
        public string NOMBRE { get; set; }
        public string WHATSAPP { get; set; }
        public string GIRO { get; set; }
        public int ULTIMA_VENTA { get; set; }
        public decimal VENTA { get; set; }
        public decimal CUOTA { get; set; }
        public decimal DIFERENCIA { get; set; }
        public string ESTATUS { get; set; }
        public decimal VENTA_MES_ANTERIOR { get; set; }
        public string MES_ANTERIOR { get; set; }
        public decimal VENTA_2_MESES_ATRAS { get; set; }
        public string DOS_MESES_ATRAS { get; set; }
    }
}
