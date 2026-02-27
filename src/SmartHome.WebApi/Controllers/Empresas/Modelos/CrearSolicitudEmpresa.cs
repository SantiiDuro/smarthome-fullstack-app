using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.WebApi.Controllers.Empresas.Modelos;

public class CrearSolicitudEmpresa
{
    public string Nombre { get; init; } = null!;
    public string Logotipo { get; init; } = null!;
    public string Rut { get; init; } = null!;
    public string Validador { get; init; } = null!;

    public CrearEmpresasArgs Args()
    {
        if (string.IsNullOrEmpty(Nombre))
        {
            throw new ArgumentNullException(nameof(Nombre));
        }

        if (string.IsNullOrEmpty(Logotipo))
        {
            throw new ArgumentNullException(nameof(Logotipo));
        }

        if (string.IsNullOrEmpty(Rut))
        {
            throw new ArgumentNullException(nameof(Rut));
        }

        if (string.IsNullOrEmpty(Validador))
        {
            throw new ArgumentNullException(nameof(Validador));
        }

        return new CrearEmpresasArgs(
            Nombre,
            Logotipo,
            Rut,
            Validador);
    }
}
