using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.Usuarios;
using SmartHome.WebApi.Controllers.Usuarios.Modelos;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ControladorUsuarioTest
{
    private Mock<IUsuarioLogica> _logicaUsuarioMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;
    private ControladorUsuario _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaUsuarioMock = new Mock<IUsuarioLogica>(MockBehavior.Default);
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);
        _controlador = new ControladorUsuario(_logicaUsuarioMock.Object, _logicaSesionMock.Object);
    }

    #region ObtenerTodos
    [TestMethod]
    public void ObtenerTodosRetornaUsuarios()
    {
        var usuario1 = new Usuario
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

        var usuario2 = new Usuario
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

        var usuarios = new List<Usuario> { usuario1, usuario2 };
        var obtenerUsuarios = new ObtenerUsuariosArgs(usuarios, 1);

        _logicaUsuarioMock
            .Setup(l => l.ObtenerTodos(It.IsAny<ParametroPaginacion>(),
                It.IsAny<ParametroUsuarioFiltro>()))
            .Returns(obtenerUsuarios);

        var paginacion = new ParametroPaginacion(1, 2);
        var filtrado = new ParametroUsuarioFiltro();

        var resultado = _controlador.ObtenerTodos(paginacion, filtrado);

        resultado.Usuarios.Should().NotBeNullOrEmpty();
        resultado.Usuarios.Should().HaveCount(usuarios.Count);
        resultado.Usuarios.Should().BeEquivalentTo(usuarios, options => options.ExcludingMissingMembers());
        resultado.Usuarios.Should().Contain(u => u.Nombre == usuario1.Nombre);
        resultado.Usuarios.Should().Contain(u => u.Nombre == usuario2.Nombre);
        resultado.Usuarios.Should().Contain(u => u.Apellido == usuario1.Apellido);
        resultado.Usuarios.Should().Contain(u => u.Apellido == usuario2.Apellido);
        resultado.Usuarios.Should().Contain(u => u.TipoRol == usuario1.Rol.Tipo);
        resultado.Usuarios.Should().Contain(u => u.TipoRol == usuario2.Rol.Tipo);
        resultado.Usuarios.Should().Contain(u => u.FechaCreacion == usuario1.FechaCreacion);
        resultado.Usuarios.Should().Contain(u => u.FechaCreacion == usuario2.FechaCreacion);
        resultado.Usuarios.Should().Contain(u => u.NombreCompleto == usuario1.Nombre + " " + usuario1.Apellido);
        resultado.Usuarios.Should().Contain(u => u.NombreCompleto == usuario2.Nombre + " " + usuario2.Apellido);
        resultado.Usuarios.Should().Contain(u => u.Email == usuario1.Email);
        resultado.Usuarios.Should().Contain(u => u.Email == usuario2.Email);
        resultado.CantidadPaginas.Should().Be(1);
    }
    #endregion
    #region ActualizarRol

    [TestMethod]
    public void ActualizarRolDueñoEmpresaRetornaUsuarioConNuevoRol()
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = new Rol { Id = Guid.NewGuid(), Tipo = "dueño empresa", Permisos = [PermisoUsuario.CrearEmpresa] },
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Today
        };

        var nuevoRol = new Rol { Id = Guid.NewGuid(), Tipo = "dueño empresa y hogar", Permisos = [PermisoUsuario.CrearHogar] };

        _logicaSesionMock.Setup(l => l.ObtenerUsuarioPorToken(It.IsAny<string>())).Returns(usuario);
        _logicaUsuarioMock.Setup(l => l.ActualizarRol(usuario));

        var authorization = "tokenValido";
        _controlador.ActualizarRol(authorization);

        _logicaUsuarioMock.Verify(l => l.ActualizarRol(usuario), Times.Once);
    }

    [TestMethod]
    public void ActualizarRolAdministradorRetornaUsuarioConNuevoRol()
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = new Rol { Id = Guid.NewGuid(), Tipo = "administrador", Permisos = [PermisoUsuario.CrearAdmin] },
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Today
        };

        var nuevoRol = new Rol { Id = Guid.NewGuid(), Tipo = "administrador dueño hogar", Permisos = [PermisoUsuario.CrearHogar] };

        _logicaSesionMock.Setup(l => l.ObtenerUsuarioPorToken(It.IsAny<string>())).Returns(usuario);
        _logicaUsuarioMock.Setup(l => l.ActualizarRol(usuario));

        var authorization = "tokenValido";
        _controlador.ActualizarRol(authorization);

        _logicaUsuarioMock.Verify(l => l.ActualizarRol(usuario), Times.Once);
    }

    #endregion

    #region ActualizarFotoPerfil
    [TestMethod]
    public void ActualizarFotoPerfilLlamaAMetodoLogica()
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "cocoperez@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            FotoPerfil = "/downloads/cocoPerez",
            FechaCreacion = DateTime.Today
        };

        var solicitud = new SolicitudActualizarFotoPerfil
        {
            FotoPerfil = "/downloads/cocoPerez"
        };

        _logicaSesionMock.Setup(l => l.ObtenerUsuarioPorToken(It.IsAny<string>())).Returns(usuario);
        _logicaUsuarioMock.Setup(l => l.ActualizarFotoPerfil(usuario, solicitud.FotoPerfil));

        var authorization = "tokenValido";
        _controlador.ActualizarFotoPerfil(solicitud, authorization);

        _logicaUsuarioMock.Verify(l => l.ActualizarFotoPerfil(usuario, solicitud.FotoPerfil), Times.Once);
    }
    #endregion
}
