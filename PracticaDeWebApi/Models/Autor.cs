using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PracticaDeWebApi.Models
{
    public class Autor
    {
        [Key]
    //Autor(Id, Nombre, Nacionalidad).
        public int id_autor { get; set; }
        public string? nombre { get; set; }
        public string? nacionalidad { get; set; }
    }
}
