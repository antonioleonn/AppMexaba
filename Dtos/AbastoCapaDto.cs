namespace AppMexaba.Dtos
{
    public class AbastoCapaDto
    {
        public string Stat { get; set; }
        public string Clas { get; set; }
        public string Almacen { get; set; }
        public string Proveedor { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Origen { get; set; }
        public decimal VtaMens { get; set; }
        public decimal VtaProm { get; set; }
        public decimal Existencia { get; set; }
        public decimal BackOrder { get; set; }
        public decimal CostoProm { get; set; }
        public decimal Margen { get; set; }
        public decimal VtaPromDinero { get; set; } // $VtaProm
        public decimal ValorInv { get; set; }
        public decimal DiasInv { get; set; }
        public decimal Sugerido { get; set; }
        public int Nivel { get; set; }
        public string Capas { get; set; }
        public string TipoImpuesto { get; set; }
    }
}
