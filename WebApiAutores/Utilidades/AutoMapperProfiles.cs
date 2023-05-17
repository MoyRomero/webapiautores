using AutoMapper;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>().ForMember(x=>x.Libros, option => option.MapFrom(MapAutorDTOLibros));

            CreateMap<LibroCreacionDTO, Libro>().ForMember(x => x.AutoresLibros, z => z.MapFrom(MapAutoresLibros));
            CreateMap<Libro,LibroDTO>().ForMember(x=>x.Autores, opcion => opcion.MapFrom(MapLibroDTOAutores));
            CreateMap<LibroPatchDTO, Libro>().ReverseMap();

            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
        {
            var resultado = new List<LibroDTO>();

            if (autor.AutoresLibros == null) return resultado;

            foreach (var dato in autor.AutoresLibros) resultado.Add(new LibroDTO { Id= dato.Libro.Id, Titulo = dato.Libro.Titulo});
            
            return resultado;
        }
        private List<AutorLibro> MapAutoresLibros( LibroCreacionDTO libroCreacionDTO, Libro libro)
        {
            var resultado = new List<AutorLibro>();

            if(libroCreacionDTO.AutoresIDs == null) return resultado;

            foreach(var autorId in libroCreacionDTO.AutoresIDs)
            {
                resultado.Add(new AutorLibro() { AutorId = autorId });
            }

            return resultado;
        }
        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorDTO>();

            if (libro == null || libro.AutoresLibros == null) return resultado;

            foreach (var dato in libro.AutoresLibros) resultado.Add(new AutorDTO { Id = dato.AutorId, NombreAutor = dato.Autor?.NombreAutor });

            return resultado;
       
        }
    }
}
