using SmartHome.LogicaNegocio.Cuartos.Entidades;

namespace SmartHome.WebApi.Controllers.Hogares.Modelos;

public record InformacionRespuestaListarCuartos
{
    public string Id { get; init; }
    public string Nombre { get; init; }

    public InformacionRespuestaListarCuartos(Cuarto cuarto)
    {
        Id = cuarto.Id.ToString();
        Nombre = cuarto.Nombre;
    }
}
