using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;
using WebApiAutores.Utilidades;

namespace WebApiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly ILogger<AutoresController> logger;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public AutoresController(AplicationDbContext context,
            ILogger<AutoresController> logger,
            IMapper mapper,
            IAuthorizationService authorizationService)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name = "obtenerAutoresv1")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromHeader] string incluirHATEOAS)
        {
            var autor = await context.Autores.Include(x => x.AutoresLibros).ThenInclude(x => x.Libro).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autor);
        }

        [HttpGet("{nombre}", Name = "obtenerAutorPorNombrev1")]
        public async Task<ActionResult<List<AutorDTO>>> BuscarAutorPorNombre(string nombre)
        {
            var Autor = await context.Autores.Where(x => x.NombreAutor.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(Autor);
        }

        [HttpGet("PrimerAutor", Name = "obtenerPrimerAutorv1")]
        //[ServiceFilter(typeof(FiltroDeAcciones))]
        public async Task<ActionResult<AutorDTO>> PrimerAutor()
        {
            //Console.WriteLine("Escribe un número: ");
            //int n2 = int.Parse(Console.ReadLine());
            //Console.WriteLine("Escribe otro número: ");
            //int n3 = int.Parse(Console.ReadLine());
            //int n1 = n2 * n3;
            //logger.LogInformation($"El resultado de {n2} * {n3} es = {n1}");

            return mapper.Map<AutorDTO>(await context.Autores.FirstOrDefaultAsync());
        }

        [HttpGet("{id:int}", Name = "obtenerAutorv1")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<AutorDTO>> BuscarAutor(int id, [FromHeader] string incluirHATEOAS)
        {
            var ConsultaAutor = await context.Autores.Where(x => x.Id == id).FirstAsync();

            if (ConsultaAutor == null) return BadRequest($"No se encontró autor con ID: {id}");

            var dto = mapper.Map<AutorDTO>(ConsultaAutor);

            return dto;
        }

        [HttpPost(Name = "crearAutorv1")]
        public async Task<ActionResult> Post(AutorCreacionDTO autorCreacionDTO)
        {
            var busquedaAutor = await context.Autores.AnyAsync(x => x.NombreAutor == autorCreacionDTO.NombreAutor);

            if (busquedaAutor)
            {
                return BadRequest($"El nombre: {autorCreacionDTO.NombreAutor}, ya se encuentra registrado en la base de datos.");
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);

            context.Add(autor);
            await context.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorDTO>(autor);

            return CreatedAtRoute("ConsultarAutor", new { id = autor.Id }, autorDTO);
        }

        [HttpPut("{id:int}", Name = "editarAutorv1")] //api/autores/IDAutor
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {

            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound($"No se encontró autor con el ID: {id}");
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);

            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "borrarAutorv1")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound($"No se encontró autor con el ID: {id}");
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}