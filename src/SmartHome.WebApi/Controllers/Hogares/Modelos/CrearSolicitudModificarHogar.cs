namespace SmartHome.WebApi.Controllers.Hogares.Modelos;

public sealed record CrearSolicitudModificarHogar
{
    public string Alias { get; init; } = null!;

    public string ObtenerAlias()
    {
        if (string.IsNullOrEmpty(Alias))
        {
            throw new ArgumentNullException(nameof(Alias));
        }

        return Alias;
    }
}
