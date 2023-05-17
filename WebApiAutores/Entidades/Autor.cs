using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.DTOs;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El campo Nombre del autor es requerido.")]
        [FirstLetterUCAttribute]
        [StringLength(maximumLength:20, ErrorMessage = "El campo {0}, no puede contener más de {1} caractéres.")]
        public string NombreAutor { get; set;}
        public List<AutorLibro> AutoresLibros { get; set;}
        public List<LibroDTO> Libros { get; set;}   
    }
}
