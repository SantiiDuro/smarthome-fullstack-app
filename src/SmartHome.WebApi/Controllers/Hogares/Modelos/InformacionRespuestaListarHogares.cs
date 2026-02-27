using SmartHome.LogicaNegocio.Hogares.Entidades;

namespace SmartHome.WebApi.Controllers.Hogares.Modelos;

public class InformacionRespuestaListarHogares
{
    public string Id { get; init; } = null!;
    public string Calle { get; init; } = null!;
    public string? Alias { get; init; } = null!;
    public int NumPuerta { get; init; }

    public InformacionRespuestaListarHogares(Hogar hogar)
    {
        Id = hogar.Id.ToString();
        Calle = hogar.Calle;
        Alias = hogar.Alias;
        NumPuerta = hogar.NumPuerta;
    }
}
