using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAutores.DTOs
{
    public class Recurso
    {
        [NotMapped]
        public List<DatoHATEOAS> Enlaces { get; set; } = new List<DatoHATEOAS>();
    }
}
