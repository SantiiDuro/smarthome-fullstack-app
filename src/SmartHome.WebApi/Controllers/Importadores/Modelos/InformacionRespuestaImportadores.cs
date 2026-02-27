namespace SmartHome.WebApi.Controllers.Importadores.Modelos;

public record class InformacionRespuestaImportadores
{
    public string Importador { get; init; }

    public InformacionRespuestaImportadores(string importador)
    {
        Importador = importador;
    }
}
