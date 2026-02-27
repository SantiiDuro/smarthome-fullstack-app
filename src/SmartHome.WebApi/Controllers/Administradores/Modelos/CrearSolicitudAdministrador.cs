using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.WebApi.Controllers.Administradores.Modelos;

public sealed record CrearSolicitudAdministrador
{
    public string Nombre { get; init; } = null!;
    public string Apellido { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Contraseña { get; init; } = null!;

    public CrearAdminsArgs Args()
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

        return new CrearAdminsArgs(
            Nombre,
            Apellido,
            Email,
            Contraseña);
    }
}
