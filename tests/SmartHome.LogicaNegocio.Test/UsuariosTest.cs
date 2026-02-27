using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public sealed class UsuariosTest
{
    private Mock<IUsuarioRepositorio> _logicaUsuarioMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;
    private UsuarioLogica _servicio = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaUsuarioMock = new Mock<IUsuarioRepositorio>(MockBehavior.Strict);
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);
        _servicio = new UsuarioLogica(_logicaUsuarioMock.Object, _logicaSesionMock.Object);
    }

    #region Metodos

    [TestMethod]
    public void ObtenerTodosLosUsuariosExito()
    {
        var usuario1 = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = new Rol(),
            FotoPerfil = "/downloads/pepeGomez",
            FechaCreacion = DateTime.Today
        };

        var usuario2 = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = new Rol(),
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Today
        };

        var usuariosEsperados = new List<Usuario> { usuario1, usuario2 };
        var obtenerUsuarios = new ObtenerUsuariosArgs(usuariosEsperados, 1);

        _logicaUsuarioMock
            .Setup(x => x.ObtenerTodos(null, null))
            .Returns(obtenerUsuarios);

        var respuesta = _servicio.ObtenerTodos(null, null);

        respuesta.Usuarios.Should().NotBeNullOrEmpty();
        respuesta.Usuarios.Should().HaveCount(usuariosEsperados.Count);
        respuesta.Usuarios.Should().BeEquivalentTo(usuariosEsperados);
        respuesta.CantidadPaginas.Should().Be(1);
    }

    [TestMethod]
    public void ExisteUsuarioExito()
    {
        var usuario1 = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = new Rol(),
            FotoPerfil = "/downloads/pepeGomez",
            FechaCreacion = DateTime.Today
        };

        _logicaUsuarioMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Usuario, bool>>>(e => e.Compile()(new Usuario()
            {
                Email = usuario1.Email,
                Contraseña = usuario1.Contraseña
            }))))
            .Returns(true);

        var usuarioExiste = _servicio.Existe("pepegomez@gmail.com", "pepe1234.");

        usuarioExiste.Should().BeTrue();
    }

    [TestMethod]
    public void ObtenerUsuarioPorEmailExito()
    {
        var usuario1 = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = new Rol(),
            FotoPerfil = "/downloads/pepeGomez",
            FechaCreacion = DateTime.Today
        };

        var usuario2 = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = new Rol(),
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Today
        };

        _logicaUsuarioMock
            .Setup(l => l.ObtenerPorEmail(usuario1.Email))
            .Returns(usuario1);

        var usuario = _servicio.ObtenerUsuarioPorEmail("pepegomez@gmail.com");

        usuario.Should().NotBeNull();
        usuario.Should().BeEquivalentTo(usuario1);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void ObtenerUsuarioNuloPorEmailLanzaError()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = new Rol(),
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Today
        };

        var usuariosEsperados = new List<Usuario> { usuario };
        var obtenerUsuarios = new ObtenerUsuariosArgs(usuariosEsperados, 1);

        _logicaUsuarioMock
            .Setup(l => l.ObtenerPorEmail("pepegomez@gmail.com"))
            .Returns((Usuario)null);

        _servicio.ObtenerUsuarioPorEmail("pepegomez@gmail.com");
    }

    [TestMethod]
    public void ObtenerUsuarioPorIdExito()
    {
        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = new Rol(),
            FotoPerfil = "/downloads/pepeGomez",
            FechaCreacion = DateTime.Today
        };

        _logicaUsuarioMock
            .Setup(l => l.ObtenerPorId(usuario1.Id))
            .Returns(usuario1);

        var usuario = _servicio.ObtenerUsuarioPorId(usuario1.Id);

        usuario.Should().NotBeNull();
        usuario.Should().BeEquivalentTo(usuario1);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void ObtenerUsuarioNuloPorIdLanzaError()
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = new Rol(),
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Today
        };

        var guid = Guid.NewGuid();

        _logicaUsuarioMock
            .Setup(l => l.ObtenerPorId(guid))
            .Returns((Usuario)null);

        _servicio.ObtenerUsuarioPorId(guid);
    }

    [TestMethod]
    public void UsuarioTienePermiso()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = new Rol()
            {
                Permisos = [PermisoUsuario.CrearHogar]
            },
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Today
        };

        usuario.Rol.TienePermiso("CrearHogar").Should().BeTrue();
    }

    [TestMethod]
    public void UsuarioNoTienePermiso()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = new Rol(),
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Today
        };

        usuario.Rol.TienePermiso("permiso-invalido").Should().BeFalse();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EliminarseASiMismoLanza()
    {
        var adminQueElimina = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_ADMIN,
            FotoPerfil = "/downloads/cocoPerez"
        };

        _logicaUsuarioMock
            .Setup(i => i.ObtenerPorEmail("admin@gmail.com"))
            .Returns(adminQueElimina);

        _servicio.EliminarAdmin(adminQueElimina, "admin@gmail.com");
    }

    [TestMethod]
    public void EliminarAdministrador()
    {
        var adminQueElimina = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_ADMIN,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var email = "cocoperez@gmail.com";

        var admin = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = email,
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_ADMIN,
            FotoPerfil = "/downloads/cocoPerez"
        };

        _logicaUsuarioMock
            .Setup(i => i.ObtenerPorEmail(email))
            .Returns(admin);

        _logicaUsuarioMock
            .Setup(x => x.Eliminar(email));

        _logicaSesionMock
            .Setup(s => s.CerrarSesion(admin));

        _logicaUsuarioMock
            .Setup(u => u.GuardarCambios());

        var eliminado = _servicio.EliminarAdmin(adminQueElimina, email);

        eliminado.Should().BeTrue();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EliminarUsuarioNoAdminLanzaError()
    {
        var adminQueElimina = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_ADMIN,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var email = "cocoperez@gmail.com";

        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = email,
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.DueñoHogar,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        _logicaUsuarioMock
            .Setup(i => i.ObtenerPorEmail(email))
            .Returns(usuario);

        _servicio.EliminarAdmin(adminQueElimina, email);
    }

    [TestMethod]
    public void ActualizarRolCuandoEsDueñoEmpresaActualizaRolCorrectamente()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.DueñoEmpresa,
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Today
        };

        var rolActualizado = RolesPredefinidos.DueñoEmpresaYHogar;

        _logicaUsuarioMock.Setup(r => r.ObtenerRolPorId(RolesPredefinidos.ID_DUEÑO_EMPRESA_Y_HOGAR)).Returns(rolActualizado);
        _logicaUsuarioMock.Setup(r => r.Actualizar(usuario));

        _servicio.ActualizarRol(usuario);

        usuario.Rol.Should().Be(rolActualizado);
    }

    [TestMethod]
    public void ActualizarRolCuandoEsAdministradorActualizaRolCorrectamente()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Today
        };

        var rolActualizado = RolesPredefinidos.AdminDueñoHogar;

        _logicaUsuarioMock.Setup(r => r.ObtenerRolPorId(RolesPredefinidos.ID_ADMIN_DUEÑO_HOGAR)).Returns(rolActualizado);
        _logicaUsuarioMock.Setup(r => r.Actualizar(usuario));

        _servicio.ActualizarRol(usuario);

        usuario.Rol.Should().Be(rolActualizado);
    }

    [TestMethod]
    public void ActualizarRolCuandoNoEsAdminNiDueñoEmpresaNoActualizaRol()
    {
        var usuario = new Usuario { Id = Guid.NewGuid(), Rol = RolesPredefinidos.DueñoHogar };

        _servicio.ActualizarRol(usuario);

        usuario.Rol.Should().Be(RolesPredefinidos.DueñoHogar);

        _logicaUsuarioMock.Verify(r => r.Actualizar(It.IsAny<Usuario>()), Times.Never);
    }

    [TestMethod]
    public void ActualizarFotoPerfilExito()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.AdminDueñoHogar,
            FechaCreacion = DateTime.Today
        };

        _logicaUsuarioMock.Setup(r => r.Actualizar(usuario));

        var fotoPerfil = "foto.png";

        _servicio.ActualizarFotoPerfil(usuario, fotoPerfil);

        usuario.FotoPerfil.Should().Be(fotoPerfil);
    }
}
#endregion
