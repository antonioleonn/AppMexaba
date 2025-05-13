using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMexaba.Models
{
    [Table("cpr_map_comprador")]
    public class cpr_map_comprador
    {
        [Key]
        [Column("cve")]
        public string Cve { get; set; } = string.Empty;

        [Column("nombre_completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Column("usuario")]
        public string Usuario { get; set; } = string.Empty;

        [Column("num_comprador")]
        public string NumComprador { get; set; } = string.Empty;
    }
}
