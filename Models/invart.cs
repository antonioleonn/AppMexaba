using System.ComponentModel.DataAnnotations.Schema;

namespace AppMexaba.Models
{
    public class invart
    {
        public string art { get; set; } // Clave primaria esperada
        public string cve_pro { get; set; } // FK hacia cprprv
        // Agrega más propiedades si las necesitas más adelante
    }
}

