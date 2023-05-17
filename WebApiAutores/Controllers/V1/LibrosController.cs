using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Utilidades;

namespace WebApiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(AplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet(Name = "obtenerLibros")]
        public async Task<ActionResult<List<LibroDTO>>> Get()
        {
            var Libro = await context.Libros.Include(x => x.AutoresLibros).ThenInclude(x => x.Autor).ToListAsync();

            for (int i = 0; i < Libro.Count; i++)
            {
                Libro[i].AutoresLibros = Libro[i].AutoresLibros.OrderBy(x => x.Orden).ToList();
            }

            return mapper.Map<List<LibroDTO>>(Libro);
        }

        [HttpGet("{id:int}", Name = "obtenerLibroPorId")]
        public async Task<ActionResult<LibroDTO>> ConsultarLibro(int id)
        {
            var libro = await context.Libros
                .Include(x => x.AutoresLibros)
                .ThenInclude(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();

            return mapper.Map<LibroDTO>(libro);
        }

        [HttpPost(Name = "crearLibro")]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            try
            {
                if (libroCreacionDTO.AutoresIDs == null) return BadRequest("No se puede crear un libro sin autores.");

                var autores = await context.Autores.Where(x => libroCreacionDTO.AutoresIDs.Contains(x.Id)).Select(x => x.Id).ToListAsync();

                if (libroCreacionDTO.AutoresIDs.Count != autores.Count)
                {
                    return BadRequest("No existe alguno de los autores enviados.");
                }

                var BusquedaLibroRepetido = await context.Libros.AnyAsync(x => x.Titulo == libroCreacionDTO.Titulo);

                if (BusquedaLibroRepetido) return BadRequest($"Ya existe un libro con el título: {libroCreacionDTO.Titulo}");

                var libro = mapper.Map<Libro>(libroCreacionDTO);

                OrdenarAutores(libro);

                context.Add(libro);
                await context.SaveChangesAsync();

                var libroDTO = mapper.Map<LibroDTO>(libro);

                return CreatedAtRoute("ConsultaLibro", new { id = libro.Id }, libroDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR PERSONALIZADO:" + ex.ToString());
            }
            return BadRequest("Hubo un error");
        }

        [HttpPut("{id:int}", Name = "editarLibro")]
        public async Task<ActionResult> PutLibro(LibroCreacionDTO libroCreacionDTO, int id)
        {
            var libro = await context.Libros.Include(x => x.AutoresLibros).FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null) return NotFound();

            libro = mapper.Map(libroCreacionDTO, libro);

            OrdenarAutores(libro);

            await context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPatch("{id:int}", Name = "editarCamposDeLibro")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> jsonPatchDocument)
        {
            if (jsonPatchDocument == null) return BadRequest("El archivo json es nulo.");

            var libro = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null) return NotFound($"No se encontró el libro con el id: {id}");

            var libroDTO = mapper.Map<LibroPatchDTO>(libro);

            jsonPatchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(libroDTO);

            if (!esValido) return BadRequest("Errores de validación:" + ModelState);

            mapper.Map(libroDTO, libro);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "borrarLibro")]
        public async Task<ActionResult> Delete(int id)
        {
            var libro = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null) return NotFound($"No se encontró el libro con el id: {id}");

            context.Remove(libro);

            await context.SaveChangesAsync();

            return Ok();
        }


        private void OrdenarAutores(Libro libro)
        {
            if (libro.AutoresLibros != null) for (int i = 0; i < libro.AutoresLibros.Count; i++) libro.AutoresLibros[i].Orden = i;
        }
    }
}
