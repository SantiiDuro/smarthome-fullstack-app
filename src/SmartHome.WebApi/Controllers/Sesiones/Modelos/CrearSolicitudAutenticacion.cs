using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;

namespace SmartHome.WebApi.Controllers.Autenticaciones.Modelos;

public sealed record CrearSolicitudAutenticacion
{
    public string Email { get; init; } = null!;
    public string Contraseña { get; init; } = null!;

    public string ValidarSolicitudAutenticacion(IUsuarioLogica logicaUsuario, ISesionLogica logicaSesion)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            throw new ArgumentException("El email no puede estar vacío.", nameof(Email));
        }

        if (string.IsNullOrWhiteSpace(Contraseña))
        {
            throw new ArgumentException("La contraseña no puede estar vacía.", nameof(Contraseña));
        }

        if (!logicaUsuario.Existe(Email, Contraseña))
        {
            throw new ArgumentException("Credenciales inválidas");
        }

        var usuario = logicaUsuario.ObtenerUsuarioPorEmail(Email);

        if (logicaSesion.UsuarioEnSesion(usuario))
        {
            throw new ArgumentException("El usuario ya está en sesión.", nameof(Email));
        }

        return logicaSesion.AgregarSesion(usuario);
    }
}
