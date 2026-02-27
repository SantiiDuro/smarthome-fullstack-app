using SmartHome.LogicaNegocio.Sesiones.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Sesiones;

public sealed class SesionLogica(ISesionRepositorio repositorioSesion)
    : ISesionLogica
{
    public Usuario ObtenerUsuarioPorToken(string token)
    {
        var sesion = repositorioSesion.ObtenerTodos().FirstOrDefault(s => s.Token == token);

        if (sesion is null)
        {
            throw new KeyNotFoundException($"No se encontró una sesión activa para el token: {token}");
        }

        return sesion.Usuario;
    }

    public bool SesionActiva(string token)
    {
        return repositorioSesion.ObtenerTodos().Any(s => s.Token == token);
    }

    public string AgregarSesion(Usuario usuario)
    {
        var token = Guid.NewGuid().ToString();
        var sesion = new Sesion { Token = token, Usuario = usuario };

        repositorioSesion.Agregar(sesion);
        repositorioSesion.GuardarCambios();

        return token;
    }

    public bool UsuarioEnSesion(Usuario usuario)
    {
        return repositorioSesion.ObtenerTodos().Any(s => s.Usuario.Email == usuario.Email);
    }

    public void CerrarSesion(Usuario usuario)
    {
        var sesion = repositorioSesion.ObtenerTodos().FirstOrDefault(s => s.Usuario.Email == usuario.Email);

        if (sesion is not null)
        {
            repositorioSesion.Eliminar(sesion.Token);
            repositorioSesion.GuardarCambios();
        }
    }
}
