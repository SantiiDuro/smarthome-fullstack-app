using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SmartHome.LogicaNegocio.Cuartos;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.DispositivosHogar;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Notificaciones;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.Persistencia;
using SmartHome.WebApi.Filtros;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApp",
        policy => policy.WithOrigins("http://localhost:4200", "http://localhost:8080")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Add services to the container.

var services = builder.Services;
var configuration = builder.Configuration;

services.AddDbContext<ContextoSql>(options => options.UseSqlServer("name=ConnectionStrings:SmartHome"));

services.AddScoped<IDispositivoLogica, DispositivoLogica>();

services.AddScoped<IDispositivoRepositorio, DispositivoRepositorio>();

services.AddScoped<IUsuarioLogica, UsuarioLogica>();

services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

services.AddScoped<IHogarLogica, HogarLogica>();

services.AddScoped<IHogarRepositorio, HogarRepositorio>();

services.AddScoped<ISesionLogica, SesionLogica>();

services.AddScoped<ISesionRepositorio, SesionRepositorio>();

services.AddScoped<IEmpresaLogica, EmpresaLogica>();

services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();

services.AddScoped<IDispositivoHogarLogica, DispositivoHogarLogica>();

services.AddScoped<IDispositivoHogarRepositorio, DispositivoHogarRepositorio>();

services.AddScoped<INotificacionLogica, NotificacionLogica>();

services.AddScoped<INotificacionRepositorio, NotificacionRepositorio>();

services.AddScoped<ICuartoLogica, CuartoLogica>();

services.AddScoped<ICuartoRepositorio, CuartoRepositorio>();

builder.Services.AddControllers();

builder.
    Services.AddControllers(options =>
    {
        options.Filters.Add<ExcepcionFiltro>();
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

var app = builder.Build();

app.UseCors("AllowFrontendApp");

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
}
