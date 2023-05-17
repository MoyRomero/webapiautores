using Microsoft.Extensions.Logging;
using System.IO;

namespace WebApiAutores.Middlewares
{
    public static class LoguearRespuestaHttpMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoguearRespuestaHttp(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
        }
    }

    public class LoguearRespuestaHTTPMiddleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<LoguearRespuestaHTTPMiddleware> logger;

        public LoguearRespuestaHTTPMiddleware(RequestDelegate siguiente, ILogger<LoguearRespuestaHTTPMiddleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext contexto)
        {
            using (var ms = new MemoryStream())
            {
                var CuerpoOriginarRespuesta = contexto.Response.Body;
                contexto.Response.Body = ms;

                await siguiente(contexto);

                ms.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(CuerpoOriginarRespuesta);
                contexto.Response.Body = CuerpoOriginarRespuesta;
                logger.LogInformation(respuesta);
            }
        }

    }
}
