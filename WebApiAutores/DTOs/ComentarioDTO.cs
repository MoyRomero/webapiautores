using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Entidades;

namespace WebApiAutores.DTOs
{
    [NotMapped]
    public class ComentarioDTO
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
    }
}
