using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs
{
    public class AutorCreacionDTO
    {
        [Required(ErrorMessage = "El campo Nombre del autor es requerido.")]
        [FirstLetterUCAttribute]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0}, no puede contener más de {1} caractéres.")]
        public string NombreAutor { get; set; }
    }
}
