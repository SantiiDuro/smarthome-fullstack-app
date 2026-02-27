using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.WebApi.Controllers.Camaras.Modelos;

public sealed record CrearSolicitudCamara
{
    public string Nombre { get; init; } = null!;
    public string Modelo { get; init; } = null!;
    public string Descripcion { get; init; } = null!;
    public List<FotografiaDispositivo> Fotografias { get; init; } = null!;
    public bool DetectaMovimiento { get; init; }
    public bool DetectaPersona { get; init; }
    public bool UsoExterior { get; init; }
    public bool UsoInterior { get; init; }

    public CrearCamarasArgs Args(Usuario usuario)
    {
        if (string.IsNullOrEmpty(Nombre))
        {
            throw new ArgumentNullException(nameof(Nombre));
        }

        if (string.IsNullOrEmpty(Modelo))
        {
            throw new ArgumentNullException(nameof(Modelo));
        }

        if (string.IsNullOrEmpty(Descripcion))
        {
            throw new ArgumentNullException(nameof(Descripcion));
        }

        ArgumentNullException.ThrowIfNull(Fotografias);

        if (!Fotografias.Any(f => f.EsPrincipal == true))
        {
            throw new InvalidOperationException("El dispositivo no contiene fotografía principal");
        }

        var fotografiasPrincipales = Fotografias.Where(f => f.EsPrincipal == true);
        if (fotografiasPrincipales.Count() > 1)
        {
            throw new InvalidOperationException("El dispositivo contiene más de una fotografía principal");
        }

        if (usuario.Empresa is null)
        {
            throw new InvalidOperationException("El usuario no tiene una empresa asociada");
        }

        return new CrearCamarasArgs(
            Nombre,
            Modelo,
            Descripcion,
            Fotografias,
            usuario.Empresa,
            DetectaMovimiento,
            DetectaPersona,
            UsoExterior,
            UsoInterior);
    }
}
