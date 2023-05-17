using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/libros/{libroId:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly AplicationDbContext BD;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger logger;

        public ComentariosController(AplicationDbContext Bd,
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            ILogger<ComentariosController> logger)
        {
            BD = Bd;
            this.mapper = mapper;
            this.userManager = userManager;
            this.logger = logger;
        }

        [HttpGet(Name = "obtenerComentariosPorLibro")]
        public async Task<ActionResult<List<ComentarioDTO>>> GetComents(int libroId)
        {
            var LibroComentarios = await BD.Comentarios.Where(x => x.LibroId == libroId).ToListAsync();

            return mapper.Map<List<ComentarioDTO>>(LibroComentarios);
        }

        [HttpGet("{id:int}", Name = "obtenerComentarioPorId")]
        public async Task<ActionResult<ComentarioDTO>> GetComent(int id)
        {
            var Comentario = await BD.Comentarios.FirstOrDefaultAsync(x => x.Id == id);

            if (Comentario == null)
            {
                return NotFound($"El comentario con id: {id}, no existe.");
            }

            return mapper.Map<ComentarioDTO>(Comentario);
        }

        [HttpPost(Name = "crearComentario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PostComent(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            try
            {
                var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = emailClaim.Value;
                var Usuario = await userManager.FindByEmailAsync(email);
                var UsuarioId = Usuario.Id;

                var Libro = await BD.Libros.Where(x => x.Id == libroId).AnyAsync();

                if (!Libro) return NotFound($"No se encontró el libro con el ID:{libroId}");

                var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);

                comentario.LibroId = libroId;
                comentario.UsuarioId = UsuarioId;

                BD.Add(comentario);
                await BD.SaveChangesAsync();

                var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);

                return CreatedAtRoute("ConsultarComentario", new { id = comentario.Id, libroId }, comentarioDTO);

            }
            catch (Exception ex)
            {
                logger.LogInformation($"Error personalizado: {ex}");
                return BadRequest("Error personalizado: " + ex);
            }
        }

        [HttpPut("{id:int}", Name = "editarComentario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(ComentarioCreacionDTO comentarioCreacionDTO, int id, int libroId)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var Usuario = await userManager.FindByEmailAsync(email);
            var UsuarioId = Usuario.Id;

            var Libro = await BD.Libros.Where(x => x.Id == libroId).AnyAsync();

            if (!Libro) return NotFound($"No se encontró el libro con el ID:{libroId}");

            var ExisteComent = await BD.Comentarios.AnyAsync(x => x.Id == id);

            if (!ExisteComent) return NotFound($"No se encontró ningún comentario con el ID: {id}");

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);

            comentario.Id = id;
            comentario.LibroId = libroId;
            comentario.UsuarioId = UsuarioId;

            BD.Update(comentario);
            await BD.SaveChangesAsync();

            return NoContent();
        }
    }
}
