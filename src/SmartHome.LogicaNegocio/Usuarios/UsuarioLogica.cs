using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Usuarios;

public sealed class UsuarioLogica(IUsuarioRepositorio repositorioUsuario, ISesionLogica logicaSesion)
    : IUsuarioLogica
{
    public void GuardarCambios()
    {
        repositorioUsuario.GuardarCambios();
    }

    public Usuario AgregarDueñoHogar(CrearDueñosHogarArgs args)
    {
        var existe = repositorioUsuario.Existe(x => x.Email == args.Email);
        if (existe)
        {
            throw new ArgumentException("El email ya está asociado a una cuenta.");
        }

        var rolExistente = repositorioUsuario.ObtenerRolPorId(RolesPredefinidos.ID_DUEÑO_HOGAR);

        var usuario = new Usuario
        {
            Nombre = args.Nombre,
            Apellido = args.Apellido,
            Email = args.Email,
            Contraseña = args.Contraseña,
            Rol = rolExistente,
            RolId = rolExistente.Id,
            FotoPerfil = args.FotoPerfil,
            FechaCreacion = DateTime.Today
        };

        repositorioUsuario.Agregar(usuario);
        return usuario;
    }

    public Usuario AgregarAdmin(CrearAdminsArgs args)
    {
        var existe = repositorioUsuario.Existe(x => x.Email == args.Email);
        if (existe)
        {
            throw new ArgumentException("El email ya está asociado a una cuenta.");
        }

        var rolExistente = repositorioUsuario.ObtenerRolPorId(RolesPredefinidos.ID_ADMIN);

        var usuario = new Usuario
        {
            Nombre = args.Nombre,
            Apellido = args.Apellido,
            Email = args.Email,
            Contraseña = args.Contraseña,
            Rol = rolExistente,
            RolId = rolExistente.Id,
            FechaCreacion = DateTime.Today
        };

        repositorioUsuario.Agregar(usuario);
        return usuario;
    }

    public Usuario AgregarDueñoEmpresa(CrearDueñosEmpresaArgs args)
    {
        var existe = repositorioUsuario.Existe(x => x.Email == args.Email);
        if (existe)
        {
            throw new ArgumentException("El email ya está asociado a una cuenta.");
        }

        var rolExistente = repositorioUsuario.ObtenerRolPorId(RolesPredefinidos.ID_DUEÑO_EMPRESA);

        var usuario = new Usuario
        {
            Nombre = args.Nombre,
            Apellido = args.Apellido,
            Email = args.Email,
            Contraseña = args.Contraseña,
            Rol = rolExistente,
            RolId = rolExistente.Id,
            Empresa = args.Empresa,
            FechaCreacion = DateTime.Today
        };

        repositorioUsuario.Agregar(usuario);
        return usuario;
    }

    public ObtenerUsuariosArgs ObtenerTodos(ParametroPaginacion? parametroPaginacion, ParametroUsuarioFiltro? parametroFiltro)
    {
        return repositorioUsuario.ObtenerTodos(parametroPaginacion, parametroFiltro);
    }

    public bool Existe(string email, string contraseña)
    {
        return repositorioUsuario.Existe(u => u.Email == email && u.Contraseña == contraseña);
    }

    public Usuario ObtenerUsuarioPorEmail(string email)
    {
        var usuario = repositorioUsuario.ObtenerPorEmail(email);

        if (usuario is null)
        {
            throw new KeyNotFoundException($"No se encontró un usuario con el email: {email}");
        }

        return usuario;
    }

    public Usuario ObtenerUsuarioPorId(Guid id)
    {
        var usuario = repositorioUsuario.ObtenerPorId(id);

        if (usuario is null)
        {
            throw new KeyNotFoundException("No se encontró un usuario con ese ID.");
        }

        return usuario;
    }

    public void ActualizarRol(Usuario usuario)
    {
        if (usuario.Rol.Tipo == "administrador")
        {
            var nuevoRol = repositorioUsuario.ObtenerRolPorId(RolesPredefinidos.ID_ADMIN_DUEÑO_HOGAR);

            usuario.Rol = nuevoRol;

            repositorioUsuario.Actualizar(usuario);
        }
        else if (usuario.Rol.Tipo == "dueño empresa")
        {
            var nuevoRol = repositorioUsuario.ObtenerRolPorId(RolesPredefinidos.ID_DUEÑO_EMPRESA_Y_HOGAR);

            usuario.Rol = nuevoRol;

            repositorioUsuario.Actualizar(usuario);
        }
    }

    public void ActualizarFotoPerfil(Usuario usuario, string fotoPerfil)
    {
        usuario.FotoPerfil = fotoPerfil;
        repositorioUsuario.Actualizar(usuario);
    }

    public bool EliminarAdmin(Usuario usuarioQueElimina, string email)
    {
        var usuarioAEliminar = ObtenerUsuarioPorEmail(email);

        if (usuarioQueElimina.Id == usuarioAEliminar.Id)
        {
            throw new InvalidOperationException("No puedes eliminarte a ti mismo.");
        }

        if (usuarioAEliminar.RolId != RolesPredefinidos.ID_ADMIN)
        {
            throw new InvalidOperationException("El usuario a eliminar no es administrador.");
        }

        logicaSesion.CerrarSesion(usuarioAEliminar);
        repositorioUsuario.Eliminar(email);
        GuardarCambios();

        return true;
    }
}
