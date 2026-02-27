using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Sesiones;

public interface ISesionLogica
{
    Usuario ObtenerUsuarioPorToken(string token);
    bool SesionActiva(string token);
    string AgregarSesion(Usuario usuario);
    bool UsuarioEnSesion(Usuario usuario);
    void CerrarSesion(Usuario usuario);
}
