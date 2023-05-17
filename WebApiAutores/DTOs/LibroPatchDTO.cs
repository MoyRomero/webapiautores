using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs
{
    public class LibroPatchDTO
    {
        [Required(ErrorMessage = "El campo {0} del libro, es requerido.")]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0}, no puede contener más de {1} caractéres.")]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
