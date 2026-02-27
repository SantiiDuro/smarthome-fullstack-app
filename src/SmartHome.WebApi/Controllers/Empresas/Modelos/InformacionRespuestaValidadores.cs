namespace SmartHome.WebApi.Controllers.Empresas.Modelos;

public record class InformacionRespuestaValidadores
{
    public string Validador { get; init; }

    public InformacionRespuestaValidadores(string validador)
    {
        Validador = validador;
    }
}
