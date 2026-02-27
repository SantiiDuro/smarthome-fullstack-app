using FluentAssertions;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.Persistencia.Test;

[TestClass]
public class UsuarioRepositorioTest
{
    private readonly ContextoSql _contexto;
    private readonly UsuarioRepositorio _repositorio;

    public UsuarioRepositorioTest()
    {
        _contexto = ContextoSqlTests.CrearContextoMemoria();
        _repositorio = new UsuarioRepositorio(_contexto);
    }

    [TestInitialize]
    public void Setup()
    {
        _contexto.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _contexto.Database.EnsureDeleted();
    }

    #region Agregar
    #region Éxito

    [TestMethod]
    public void CuandoSeProporcionaInfoDeberiaAgregarseALaBaseDeDatos()
    {
        var permisos = new List<PermisoUsuario>();

        var rol = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = permisos
        };

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol,
            RolId = rol.Id,
            FotoPerfil = "/downloads/pepeGomez"
        };

        _repositorio.Agregar(usuario);
        _repositorio.GuardarCambios();

        var usuarioEncontrado = _contexto.Usuarios.Find(usuario.Id);

        usuarioEncontrado.Id.Should().Be(usuario.Id);
        usuarioEncontrado.Nombre.Should().Be(usuario.Nombre);
        usuarioEncontrado.Apellido.Should().Be(usuario.Apellido);
        usuarioEncontrado.Email.Should().Be(usuario.Email);
        usuarioEncontrado.Contraseña.Should().Be(usuario.Contraseña);
        usuarioEncontrado.Rol.Should().Be(usuario.Rol);
        usuarioEncontrado.FotoPerfil.Should().Be(usuario.FotoPerfil);
    }
    #endregion
    #endregion

    #region Existe

    [TestMethod]
    public void CuandoExisteUsuarioConEmailDeberiaRetornarTrue()
    {
        var permisos = new List<PermisoUsuario>();

        var rol = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = permisos
        };

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol,
            RolId = rol.Id,
            FotoPerfil = "/downloads/pepeGomez"
        };

        _repositorio.Agregar(usuario);
        _repositorio.GuardarCambios();

        var existe = _repositorio.Existe(u => u.Email == usuario.Email);

        existe.Should().BeTrue();
    }
    #endregion

    #region ObtenerTodos

    [TestMethod]
    public void CuandoSeAgreganUsuariosDeberiaRetornarTodosLosUsuarios()
    {
        var rol = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = []
        };

        var usuario1 = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol,
            RolId = rol.Id,
            FotoPerfil = "/downloads/pepeGomez"
        };

        var usuario2 = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = rol,
            RolId = rol.Id,
            FotoPerfil = "/downloads/cocoPerez"
        };

        _repositorio.Agregar(usuario1);
        _repositorio.Agregar(usuario2);
        _repositorio.GuardarCambios();

        var usuarios = _repositorio.ObtenerTodos(null, null);

        usuarios.Should().NotBeNull();
        usuarios.Usuarios.Should().ContainEquivalentOf(usuario1);
        usuarios.Usuarios.Should().ContainEquivalentOf(usuario2);
        usuarios.CantidadPaginas.Should().Be(1);
    }

    [TestMethod]
    public void CuandoSeEliminaUnUsuarioDeberiaNoExistirEnLaLista()
    {
        var permisos = new List<PermisoUsuario>();

        var rol = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = permisos
        };

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol,
            RolId = rol.Id,
            FotoPerfil = "/downloads/pepeGomez"
        };

        _repositorio.Agregar(usuario);
        _repositorio.GuardarCambios();

        var existeAntesDeEliminar = _repositorio.Existe(u => u.Email == usuario.Email);
        existeAntesDeEliminar.Should().BeTrue();

        _repositorio.Eliminar(usuario.Email);
        _repositorio.GuardarCambios();

        var existeDespuesDeEliminar = _repositorio.Existe(u => u.Email == usuario.Email);
        existeDespuesDeEliminar.Should().BeFalse();
    }

    [TestMethod]
    public void CuandoSeAgreganUsuariosDeberiaRetornar1Usuario()
    {
        var rol = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = []
        };
        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol,
            RolId = rol.Id,
            FotoPerfil = "/downloads/pepeGomez",
            FechaCreacion = DateTime.Now
        };
        var usuario2 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = rol,
            RolId = rol.Id,
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Now
        };

        _repositorio.Agregar(usuario1);
        _repositorio.Agregar(usuario2);
        _repositorio.GuardarCambios();

        var parametros = new ParametroPaginacion(2, 1);
        var usuarios = _repositorio.ObtenerTodos(parametros, null);

        usuarios.Should().NotBeNull();
        usuarios.Usuarios.Should().ContainEquivalentOf(usuario1);
        usuarios.Usuarios.Should().NotContainEquivalentOf(usuario2);
        usuarios.CantidadPaginas.Should().Be(3);
    }

    [TestMethod]
    public void CuandoSeAgreganUsuariosDeberiaRetornar1UsuarioConRolDueñoEmpresa()
    {
        var rol1 = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = []
        };
        var rol2 = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño empresa",
            Permisos = []
        };
        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol1,
            RolId = rol1.Id,
            FotoPerfil = "/downloads/pepeGomez",
            FechaCreacion = DateTime.Now
        };
        var usuario2 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = rol2,
            RolId = rol2.Id,
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Now
        };

        _repositorio.Agregar(usuario1);
        _repositorio.Agregar(usuario2);
        _repositorio.GuardarCambios();

        var parametros = new ParametroUsuarioFiltro("dueño empresa", null);
        var paginacion = new ParametroPaginacion(1, 2);
        var usuarios = _repositorio.ObtenerTodos(paginacion, parametros);

        usuarios.Should().NotBeNull();
        usuarios.Usuarios.Should().ContainEquivalentOf(usuario2);
        usuarios.Usuarios.Should().NotContainEquivalentOf(usuario1);
        usuarios.CantidadPaginas.Should().Be(1);
    }

    [TestMethod]
    public void CuandoSeAgreganUsuariosDeberiaRetornar1UsuarioConNombreCompletoPepeGomez()
    {
        var rol1 = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = []
        };
        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol1,
            RolId = rol1.Id,
            FotoPerfil = "/downloads/pepeGomez",
            FechaCreacion = DateTime.Now
        };
        var usuario2 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = rol1,
            RolId = rol1.Id,
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Now
        };

        _repositorio.Agregar(usuario1);
        _repositorio.Agregar(usuario2);
        _repositorio.GuardarCambios();

        var parametros = new ParametroUsuarioFiltro(null, "Pepe Gomez");
        var paginacion = new ParametroPaginacion(1, 2);
        var usuarios = _repositorio.ObtenerTodos(paginacion, parametros);

        usuarios.Should().NotBeNull();
        usuarios.Usuarios.Should().ContainEquivalentOf(usuario1);
        usuarios.Usuarios.Should().NotContainEquivalentOf(usuario2);
        usuarios.CantidadPaginas.Should().Be(1);
    }

    [TestMethod]
    public void CuandoSeAgreganUsuariosDeberiaRetornar1UsuarioConRolDueñoEmpresaYNombreCompletoPepeGomez()
    {
        var rol1 = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = []
        };
        var rol2 = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño empresa",
            Permisos = []
        };
        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol1,
            RolId = rol1.Id,
            FotoPerfil = "/downloads/pepeGomez",
            FechaCreacion = DateTime.Now
        };
        var usuario2 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepito@gmail.com",
            Contraseña = "coco1234.",
            Rol = rol2,
            RolId = rol2.Id,
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Now
        };
        var usuario3 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Juan",
            Apellido = "Perez",
            Email = "juanperez@gmail.com",
            Contraseña = "juan1234.",
            Rol = rol2,
            RolId = rol2.Id,
            FotoPerfil = "/downloads/pepeGomez",
            FechaCreacion = DateTime.Now
        };

        _repositorio.Agregar(usuario1);
        _repositorio.Agregar(usuario2);
        _repositorio.Agregar(usuario3);
        _repositorio.GuardarCambios();

        var parametros = new ParametroUsuarioFiltro("dueño empresa", "Pepe Gomez");
        var paginacion = new ParametroPaginacion(1, 2);
        var usuarios = _repositorio.ObtenerTodos(paginacion, parametros);

        usuarios.Should().NotBeNull();
        usuarios.Usuarios.Should().ContainEquivalentOf(usuario2);
        usuarios.Usuarios.Should().NotContainEquivalentOf(usuario1);
        usuarios.Usuarios.Should().NotContainEquivalentOf(usuario3);
        usuarios.CantidadPaginas.Should().Be(1);
    }
    #endregion

    #region ObtenerRolPorId

    [TestMethod]
    public void CuandoSeProporcionaUnIdValidoDeRolDeberiaRetornarElRol()
    {
        var rol = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = []
        };

        _contexto.Roles.Add(rol);
        _contexto.SaveChanges();

        var rolEncontrado = _repositorio.ObtenerRolPorId(rol.Id);

        rolEncontrado.Should().NotBeNull();
        rolEncontrado.Id.Should().Be(rol.Id);
        rolEncontrado.Tipo.Should().Be(rol.Tipo);
    }

    [TestMethod]
    public void CuandoSeProporcionaUnIdInvalidoDeRolDeberiaRetornarNull()
    {
        var rolEncontrado = _repositorio.ObtenerRolPorId(Guid.NewGuid());

        rolEncontrado.Should().BeNull();
    }
    #endregion

    #region ActualizarEmpresa

    [TestMethod]
    public void CuandoSeActualizaLaEmpresaDeberiaCambiarLaEmpresaDelUsuario()
    {
        var empresa1 = new Empresa
        {
            Nombre = "ORT",
            Logotipo = "ort.png",
            Rut = "123456789",
            NombreCreador = "Juan",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var empresa2 = new Empresa
        {
            Nombre = "UM",
            Logotipo = "um.png",
            Rut = "12343268",
            NombreCreador = "Pepe",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        _contexto.Empresas.Add(empresa1);
        _contexto.Empresas.Add(empresa2);
        _contexto.SaveChanges();

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Juan",
            Apellido = "Perez",
            Email = "juanperez@gmail.com",
            Contraseña = "juan1234.",
            Empresa = empresa1,
            FotoPerfil = "/downloads/juanPerez",
            FechaCreacion = DateTime.Now
        };

        _repositorio.Agregar(usuario);
        _repositorio.GuardarCambios();

        usuario.Empresa = empresa2;

        _repositorio.Actualizar(usuario);
        _repositorio.GuardarCambios();

        var usuarioActualizado = _contexto.Usuarios.Find(usuario.Id);

        usuarioActualizado.Empresa.Should().NotBeNull();
        usuarioActualizado.Empresa.Id.Should().Be(empresa2.Id);
        usuarioActualizado.Empresa.Nombre.Should().Be(empresa2.Nombre);
    }

    #endregion

    #region ActualizarRol

    [TestMethod]
    public void CuandoSeActualizaElRolDeberiaCambiarElRolDelUsuario()
    {
        var rol1 = _contexto.Roles.FirstOrDefault(x => x.Tipo == "administrador");
        var rol2 = _contexto.Roles.FirstOrDefault(x => x.Tipo == "administrador dueño hogar");

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol1,
            RolId = rol1.Id,
            FotoPerfil = "/downloads/pepeGomez",
            FechaCreacion = DateTime.Now
        };

        _repositorio.Agregar(usuario);
        _repositorio.GuardarCambios();

        usuario.Rol = rol2;

        _repositorio.Actualizar(usuario);
        _repositorio.GuardarCambios();

        var usuarioActualizado = _contexto.Usuarios.Find(usuario.Id);

        usuarioActualizado.RolId.Should().Be(rol2.Id);
        usuarioActualizado.Rol.Should().Be(rol2);
    }

    #endregion

    #region ObtenerPorId
    [TestMethod]
    public void ObtenerPorIdExito()
    {
        var permisos = new List<PermisoUsuario>();

        var rol = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = permisos
        };

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol,
            RolId = rol.Id,
            FotoPerfil = "/downloads/pepeGomez"
        };

        _repositorio.Agregar(usuario);
        _repositorio.GuardarCambios();

        var resultado = _repositorio.ObtenerPorId(usuario.Id);

        resultado.Should().NotBeNull();
        resultado.Should().Be(usuario);
    }
    #endregion

    #region ObtenerPorEmail
    [TestMethod]
    public void ObtenerPorEmailExito()
    {
        var permisos = new List<PermisoUsuario>();

        var rol = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = permisos
        };

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol,
            RolId = rol.Id,
            FotoPerfil = "/downloads/pepeGomez"
        };

        _repositorio.Agregar(usuario);
        _repositorio.GuardarCambios();

        var resultado = _repositorio.ObtenerPorEmail(usuario.Email);

        resultado.Should().NotBeNull();
        resultado.Should().Be(usuario);
    }
    #endregion
}
