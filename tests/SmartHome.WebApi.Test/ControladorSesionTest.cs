using Moq;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.Autenticaciones.Modelos;
using SmartHome.WebApi.Controllers.Sesiones;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ControladorSesionTest
{
    private CrearSolicitudAutenticacion _solicitud = null!;
    private Mock<IUsuarioLogica> _logicaUsuarioMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;
    private ControladorSesion _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaUsuarioMock = new Mock<IUsuarioLogica>(MockBehavior.Default);
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);
        _controlador = new ControladorSesion(_logicaUsuarioMock.Object, _logicaSesionMock.Object);
    }

    #region Autenticar
    #region Error
    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void AutenticarConArgsNullLanzaExcepcion()
    {
        _controlador.Autenticar(null);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentException))]
    public void AutenticarConEmailNullOVacioLanzaExcepcion(string email)
    {
        _solicitud = new CrearSolicitudAutenticacion
        {
            Email = email,
            Contraseña = "pepe1234."
        };

        _controlador.Autenticar(_solicitud);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentException))]
    public void AutenticarConContraseñaNullOVacioLanzaExcepcion(string contra)
    {
        _solicitud = new CrearSolicitudAutenticacion
        {
            Email = "pepegomez@gmail.com",
            Contraseña = contra
        };

        _controlador.Autenticar(_solicitud);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AutenticarConUsuarioNoExistenteLanzaExcepcion()
    {
        _solicitud = new CrearSolicitudAutenticacion
        {
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234."
        };

        _logicaUsuarioMock
            .Setup(l => l.Existe(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        _controlador.Autenticar(_solicitud);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AutenticarConUsuarioYaEnSesionLanzaExcepcion()
    {
        _solicitud = new CrearSolicitudAutenticacion
        {
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234."
        };

        _logicaUsuarioMock
            .Setup(l => l.Existe(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        _logicaSesionMock
            .Setup(l => l.UsuarioEnSesion(It.IsAny<Usuario>()))
            .Returns(true);

        _controlador.Autenticar(_solicitud);
    }
    #endregion
    #region Exito
    [TestMethod]
    public void AutenticarConUsuarioExistenteYNoEnSesionAgregaSesion()
    {
        var token = Guid.NewGuid().ToString();

        var usuario = new Usuario
        {
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = RolesPredefinidos.DueñoHogar
        };

        _solicitud = new CrearSolicitudAutenticacion
        {
            Email = usuario.Email,
            Contraseña = usuario.Contraseña
        };

        _logicaUsuarioMock
            .Setup(l => l.Existe(It.Is<string>(e => e == usuario.Email), It.Is<string>(c => c == usuario.Contraseña)))
            .Returns(true);

        _logicaUsuarioMock
            .Setup(l => l.ObtenerUsuarioPorEmail(It.Is<string>(e => e == usuario.Email)))
            .Returns(usuario);

        _logicaSesionMock
            .Setup(l => l.UsuarioEnSesion(It.Is<Usuario>(u => u == usuario)))
            .Returns(false);

        _logicaSesionMock
            .Setup(l => l.AgregarSesion(It.Is<Usuario>(u => u == usuario)))
            .Returns(token);

        _logicaSesionMock
            .Setup(l => l.ObtenerUsuarioPorToken(It.Is<string>(t => t == token)))
            .Returns(usuario);

        _controlador.Autenticar(_solicitud);

        _logicaSesionMock.Verify(l => l.AgregarSesion(It.Is<Usuario>(u => u == usuario)), Times.Once);
    }
    #endregion
    #endregion
    #region Desautenticar
    #region Éxito

    [TestMethod]
    public void DesautenticarConUsuarioEnSesionCierraSesion()
    {
        var token = "token valido";
        var usuario = new Usuario { Id = Guid.NewGuid(), Nombre = "Usuario", Email = "usuario@gmail.com" };

        _logicaSesionMock.Setup(l => l.ObtenerUsuarioPorToken(It.Is<string>(t => t == token))).Returns(usuario);

        _controlador.Desautenticar(token);

        _logicaSesionMock.Verify(l => l.CerrarSesion(It.IsAny<Usuario>()), Times.Once);
    }
    #endregion
}
#endregion
