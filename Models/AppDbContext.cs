using System.Collections.Generic;
using System.Reflection.Emit;
using AppMexaba.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AppMexaba.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ArticuloComprador> cpr_art { get; set; }
        public DbSet<ComboArticuloDto> ComboArticuloDto { get; set; }
        public DbSet<cprcom> cprcom { get; set; }
        public DbSet<tcausr> tcausr { get; set; }
        public DbSet<invart> invart { get; set; }
        public DbSet<inviar> inviar { get; set; }
        public DbSet<cprprv> cprprv { get; set; }
        public DbSet<cpr_map_comprador> cpr_map_comprador { get; set; }
        public DbSet<alm_art> alm_art { get; set; }

        public DbSet<VentaCFR> VentasClienteFrecuente { get; set; }
        public DbSet<VentasPorSucursal> VentasPorSucursal { get; set; }
        public DbSet<VentasPorSegmento> VentasPorSegmento { get; set; }
        public DbSet<VentasSocioVSucursal> VentasSocioVSucursal { get; set; }
        public DbSet<ParticipacionSegmento> ParticipacionSegmento { get; set; }
        public DbSet<VentasPorDiaTipoVenta> VentasPorDiaTipo { get; set; }
        public DbSet<VentasSocioVSucursal> VentasSocioVSucursal_Unificada { get; set; }
        public DbSet<VentasSocioVSucursal> ComparativaVentas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticuloComprador>().ToTable("cpr_art");
            modelBuilder.Entity<ArticuloComprador>().HasKey(x => x.Art);

            modelBuilder.Entity<ComboArticuloDto>().HasNoKey();

            modelBuilder.Entity<cprcom>().ToTable("cprcom");
            modelBuilder.Entity<cprcom>().HasKey(x => x.cve);

            modelBuilder.Entity<tcausr>().ToTable("tcausr");
            modelBuilder.Entity<tcausr>().HasKey(x => x.nombre);

            modelBuilder.Entity<invart>().ToTable("invart");
            modelBuilder.Entity<invart>().HasKey(x => x.art);

            modelBuilder.Entity<inviar>().ToTable("inviar");
            modelBuilder.Entity<inviar>().HasKey(x => x.art);

            modelBuilder.Entity<cprprv>().ToTable("cprprv");
            modelBuilder.Entity<cprprv>().HasKey(x => x.proveedor);

            modelBuilder.Entity<cpr_map_comprador>().ToTable("cpr_map_comprador");
            modelBuilder.Entity<cpr_map_comprador>().HasKey(x => x.Cve);

            modelBuilder.Entity<alm_art>().ToTable("alm_art");
            modelBuilder.Entity<alm_art>().HasKey(x => new { x.alm, x.salm, x.art });

            modelBuilder.Entity<VentaCFR>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VentasClienteFrecuente");
            });

            modelBuilder.Entity<VentasPorSucursal>().HasNoKey().ToView("VentasPorSucursal");
            modelBuilder.Entity<VentasPorSegmento>().HasNoKey().ToView("VentasPorSegmento");
            modelBuilder.Entity<VentasSocioVSucursal>().HasNoKey().ToView("VentasSocioVSucursal");
            modelBuilder.Entity<ParticipacionSegmento>().HasNoKey().ToView("ParticipacionSegmento");

            modelBuilder.Entity<VentasPorDiaTipoVenta>().HasNoKey(); // obligatorio si es función
            modelBuilder.Entity<VentasPorDiaTipoVenta>().ToView(null); // para evitar que EF piense que es una tabla o vista

            modelBuilder.Entity<VentasSocioVSucursal>().HasNoKey().ToView("VentasSocioVSucursal_Unificada");
            modelBuilder.Entity<VentasSocioVSucursal>().HasNoKey().ToFunction("fn_VentasSocioVSucursalUnificada");
        }
    }
}
