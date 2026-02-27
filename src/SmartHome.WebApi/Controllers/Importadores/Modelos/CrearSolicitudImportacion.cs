namespace SmartHome.WebApi.Controllers.Importadores.Modelos;

public record class CrearSolicitudImportacion
{
    public string Ruta { get; init; } = null!;
    public string IdentificadorImportador { get; init; } = null!;
}
