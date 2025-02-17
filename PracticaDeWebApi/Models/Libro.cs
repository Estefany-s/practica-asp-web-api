using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PracticaDeWebApi.Models
{
    public class Libro
    {
        [Key]
        public int id_libro { get; set; }
        public string? titulo { get; set; }
        public DateTime? anioPublicacion { get; set; }
        public int id_autor {  get; set; }
        public int id_categoria { get; set; }
        public string? resumen { get; set; }

    }
}
