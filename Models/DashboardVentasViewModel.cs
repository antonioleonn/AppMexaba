namespace AppMexaba.Models
{
    public class DashboardVentasViewModel
    {
        public DateTime FechaSeleccionada { get; set; }

        public List<VentasPorSucursal> VentasSucursal { get; set; } = new();
        public List<VentasPorSegmento> VentasSegmento { get; set; } = new();
        public List<VentasSocioVSucursal> ComparativaSocioSucursal { get; set; } = new();
        public List<ParticipacionSegmento> ParticipacionSegmento { get; set; } = new();
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public decimal TotalGeneralVentas { get; set; }
        public List<VentasPorDiaTipoVenta> VentasPorDiaTipoVenta { get; set; } = new();


    }
}
