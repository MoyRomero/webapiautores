using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAutores.DTOs
{
    [NotMapped]
    public class AutorDTO: Recurso
    {
        public int Id { get; set; }
        public string NombreAutor { get; set; } 
        public List<LibroDTO> Libros { get; set;}
    }
}
