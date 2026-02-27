using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Usuarios;

public interface IUsuarioLogica
{
    void GuardarCambios();
    Usuario AgregarDueñoHogar(CrearDueñosHogarArgs usuario);
    Usuario AgregarAdmin(CrearAdminsArgs usuario);
    Usuario AgregarDueñoEmpresa(CrearDueñosEmpresaArgs usuario);
    ObtenerUsuariosArgs ObtenerTodos(ParametroPaginacion? parametroPaginacion, ParametroUsuarioFiltro? parametroFiltro);
    bool Existe(string email, string contraseña);
    Usuario ObtenerUsuarioPorEmail(string email);
    Usuario ObtenerUsuarioPorId(Guid id);
    void ActualizarRol(Usuario usuario);
    void ActualizarFotoPerfil(Usuario usuario, string fotoPerfil);
    bool EliminarAdmin(Usuario usuario, string email);
}
