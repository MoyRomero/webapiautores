using Microsoft.AspNetCore.Builder;
using WebApiAutores;

var builder = WebApplication.CreateBuilder(args);

var startUp = new Startup(builder.Configuration);

startUp.ConfigureServices(builder.Services);

var app = builder.Build();
var ServicioLogger = (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));
startUp.Configure(app, app.Environment, ServicioLogger);

app.Run();
