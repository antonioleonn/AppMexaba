using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppMexaba.Models
{
    public class ArticuloComprador
    {
        [Key]
        [Required]
        [StringLength(15)]
        public string Art { get; set; } // Clave primaria

        [StringLength(6)]
        public string Cve { get; set; }

        [StringLength(10)]
        public string Usuario { get; set; }

        [Required]
        [StringLength(60)]
        public string Des1 { get; set; }

        [StringLength(30)]
        public string Comprador { get; set; }

        [StringLength(3)]
        public string NumComprador { get; set; }

        [StringLength(9)]
        public string NumProveedor { get; set; }

        [StringLength(50)]
        public string Proveedor { get; set; }
    }
}
