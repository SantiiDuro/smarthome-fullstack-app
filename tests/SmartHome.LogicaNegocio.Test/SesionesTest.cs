using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Sesiones.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public class SesionLogicaTests
{
    private Mock<ISesionRepositorio> _mockRepositorioSesion = null!;
    private ISesionLogica _sesionLogica = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockRepositorioSesion = new Mock<ISesionRepositorio>();
        _sesionLogica = new SesionLogica(_mockRepositorioSesion.Object);
    }

    [TestMethod]
    public void CrearSesionDeberiaInicializarPropiedades()
    {
        var id = Guid.NewGuid();
        var token = Guid.NewGuid().ToString();
        var usuario = new Usuario { Id = Guid.NewGuid(), Nombre = "Test User" };

        var sesion = new Sesion
        {
            Id = id,
            Token = token,
            Usuario = usuario
        };

        sesion.Id.Should().Be(id);
        sesion.Token.Should().Be(token);
        sesion.Usuario.Should().Be(usuario);
    }

    [TestMethod]
    public void ObtenerUsuarioPorTokenConSesionDeberiaRetornarUsuario()
    {
        var token = Guid.NewGuid().ToString();
        var usuario = new Usuario { Id = Guid.NewGuid(), Nombre = "Test User" };
        var sesion = new Sesion { Token = token, Usuario = usuario };

        _mockRepositorioSesion.Setup(r => r.ObtenerTodos()).Returns([sesion]);

        var result = _sesionLogica.ObtenerUsuarioPorToken(token);

        result.Should().Be(usuario);
    }

    [TestMethod]
    public void ObtenerUsuarioPorTokenCuandoLaSesionNoExisteLanzaExcepcion()
    {
        var token = Guid.NewGuid().ToString();
        _mockRepositorioSesion.Setup(r => r.ObtenerTodos()).Returns([]);

        Action act = () => _sesionLogica.ObtenerUsuarioPorToken(token);

        act.Should().Throw<Exception>().WithMessage($"No se encontró una sesión activa para el token: {token}");
    }

    [TestMethod]
    public void SesionActivaConSesionExistenteRetornaTrue()
    {
        var token = Guid.NewGuid().ToString();
        var sesion = new Sesion { Token = token, Usuario = new Usuario() };

        _mockRepositorioSesion.Setup(r => r.ObtenerTodos()).Returns([sesion]);

        var result = _sesionLogica.SesionActiva(token);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void SesionActivaCuandoLaSesionNoExisteRetornaFalse()
    {
        var token = Guid.NewGuid().ToString();
        _mockRepositorioSesion.Setup(r => r.ObtenerTodos()).Returns([]);

        var result = _sesionLogica.SesionActiva(token);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void AgregarSesionDeberiaAgregarSesion()
    {
        var usuario = new Usuario { Id = Guid.NewGuid(), Nombre = "Pepe" };
        _mockRepositorioSesion.Setup(r => r.Agregar(It.IsAny<Sesion>()));

        var token = _sesionLogica.AgregarSesion(usuario);

        _mockRepositorioSesion.Verify(r => r.Agregar(It.Is<Sesion>(s => s.Usuario == usuario && s.Token == token)), Times.Once);
        _mockRepositorioSesion.Verify(r => r.GuardarCambios(), Times.Once);
    }

    [TestMethod]
    public void UsuarioEnSesionConUsuarioEnSesionRetornaTrue()
    {
        var usuario = new Usuario { Id = Guid.NewGuid(), Nombre = "Pepe" };
        var sesion = new Sesion { Usuario = usuario };

        _mockRepositorioSesion.Setup(r => r.ObtenerTodos()).Returns([sesion]);

        var result = _sesionLogica.UsuarioEnSesion(usuario);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void UsuarioEnSesionCuandoElUsuarioNoEstaEnSesionRetornaFalse()
    {
        var usuario = new Usuario { Id = Guid.NewGuid(), Nombre = "Pepe", Email = "pepe@gmail.com" };
        _mockRepositorioSesion.Setup(r => r.ObtenerTodos()).Returns([]);

        var result = _sesionLogica.UsuarioEnSesion(usuario);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void CerrarSesionConUsuarioEnSesionCierraSesion()
    {
        var usuario = new Usuario { Id = Guid.NewGuid(), Nombre = "Pepe", Email = "pepe@gmail.com" };
        var sesion = new Sesion { Usuario = usuario };

        _mockRepositorioSesion.Setup(r => r.ObtenerTodos()).Returns([sesion]);

        _sesionLogica.CerrarSesion(usuario);

        _mockRepositorioSesion.Verify(r => r.Eliminar(sesion.Token), Times.Once);
        _mockRepositorioSesion.Verify(r => r.GuardarCambios(), Times.Once);
    }
}
