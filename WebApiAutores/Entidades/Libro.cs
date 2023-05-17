using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Libro
    {  
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} del libro, es requerido.")]
        [StringLength(maximumLength:20, ErrorMessage = "El campo {0}, no puede contener más de {1} caractéres.")]
        [FirstLetterUC]
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }

    }
}
