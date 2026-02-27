using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.WebApi.Controllers.DueñosEmpresa.Modelos;

public sealed record CrearSolicitudDueñoEmpresa
{
    public string Nombre { get; init; } = null!;
    public string Apellido { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Contraseña { get; init; } = null!;

    public CrearDueñosEmpresaArgs Args()
    {
        if (string.IsNullOrEmpty(Nombre))
        {
            throw new ArgumentNullException(nameof(Nombre));
        }

        if (string.IsNullOrEmpty(Apellido))
        {
            throw new ArgumentNullException(nameof(Apellido));
        }

        if (string.IsNullOrEmpty(Email))
        {
            throw new ArgumentNullException(nameof(Email));
        }

        if (string.IsNullOrEmpty(Contraseña))
        {
            throw new ArgumentNullException(nameof(Contraseña));
        }

        return new CrearDueñosEmpresaArgs(
            Nombre,
            Apellido,
            Email,
            Contraseña);
    }
}
