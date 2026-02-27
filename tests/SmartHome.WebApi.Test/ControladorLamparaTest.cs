using Moq;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.Lamparas;
using SmartHome.WebApi.Controllers.Lamparas.Modelos;

namespace SmartHome.WebApi.Test;
[TestClass]
public class ControladorLamparaTest
{
    private CrearSolicitudLampara _solicitud = null!;
    private Mock<IDispositivoLogica> _logicaDispositivoMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;

    private ControladorLampara _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaDispositivoMock = new Mock<IDispositivoLogica>(MockBehavior.Default);
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);

        _controlador = new ControladorLampara(_logicaDispositivoMock.Object, _logicaSesionMock.Object);
    }

    #region Error
    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void CrearConArgsNullLanzaExcepcion()
    {
        var auth = Guid.NewGuid().ToString();

        _controlador.AgregarLampara(null, auth);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConNombreNullOVacioLanzaExcepcion(string nombre)
    {
        _solicitud = new CrearSolicitudLampara
        {
            Nombre = nombre,
            Modelo = "AQWSDE",
            Descripcion = "lampara",
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = true }
            ]
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarLampara(_solicitud, auth);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConModeloNullOVacioLanzaExcepcion(string modelo)
    {
        _solicitud = new CrearSolicitudLampara
        {
            Nombre = "lampara",
            Modelo = modelo,
            Descripcion = "lampara",
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = true }
            ]
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarLampara(_solicitud, auth);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConDescripcionNullOVacioLanzaExcepcion(string descripcion)
    {
        _solicitud = new CrearSolicitudLampara
        {
            Nombre = "lampara",
            Modelo = "AQWSDE",
            Descripcion = descripcion,
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = true }
            ]
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarLampara(_solicitud, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConFotografiasNullLanzaExcepcion()
    {
        _solicitud = new CrearSolicitudLampara
        {
            Nombre = "lampara",
            Modelo = "AQWSDE",
            Descripcion = "lampara",
            Fotografias = null!
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarLampara(_solicitud, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearConFotografiasSinPrincipalLanzaExcepcion()
    {
        _solicitud = new CrearSolicitudLampara
        {
            Nombre = "lampara",
            Modelo = "AQWSDE",
            Descripcion = "lampara",
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = false }
            ]
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarLampara(_solicitud, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearConFotografiasMultiplesPrincipalesLanzaExcepcion()
    {
        _solicitud = new CrearSolicitudLampara
        {
            Nombre = "lampara",
            Modelo = "AQWSDE",
            Descripcion = "lampara",
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = true },

                new FotografiaDispositivo { Url = "/downloads/c410v2", EsPrincipal = true }
            ]
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarLampara(_solicitud, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SensorConUsuarioSinEmpresaLanzaExcepcion()
    {
        var dueñoEmpresa = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "pepe",
            Apellido = "Gomez",
            Email = "pepe@gmail.com",
            Contraseña = "pepe1234.",
            Empresa = null
        };

        _solicitud = new CrearSolicitudLampara
        {
            Nombre = "lampara",
            Modelo = "AQWSDE",
            Descripcion = "lampara inteligente",
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = true }
            ]
        };

        var auth = Guid.NewGuid().ToString();

        _logicaSesionMock
            .Setup(s => s.ObtenerUsuarioPorToken(It.IsAny<string>()))
            .Returns(dueñoEmpresa);

        _controlador.AgregarLampara(_solicitud, auth);

        _logicaSesionMock.Verify(s => s.ObtenerUsuarioPorToken(It.IsAny<string>()), Times.Once);
    }
    #endregion

    #region Exito
    [TestMethod]
    public void CrearConSolicitudValidaAgregaLampara()
    {
        var solicitud = new CrearSolicitudLampara
        {
            Nombre = "lampara",
            Modelo = "AQWSDE",
            Descripcion = "lampara inteligente",
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = true }
            ]
        };

        var dueñoEmpresa = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "pepe",
            Apellido = "Gomez",
            Email = "pepe@gmail.com",
            Contraseña = "pepe1234.",
            Empresa = new Empresa() { Nombre = "nombre", Logotipo = "logotipo", Rut = "rut", Validador = "Reflection.ValidadorAulas6Letras" }
        };

        var argsEsperados = new CrearLamparasArgs(
            solicitud.Nombre,
            solicitud.Modelo,
            solicitud.Descripcion,
            solicitud.Fotografias,
            dueñoEmpresa.Empresa);

        var sensor = new Dispositivo()
        {
            Nombre = solicitud.Nombre,
            Modelo = solicitud.Modelo,
            Descripcion = solicitud.Descripcion,
            Fotografias = solicitud.Fotografias
        };

        _logicaDispositivoMock.Setup(s => s.AgregarSensorVentana(It.IsAny<CrearSensoresArgs>())).Returns(sensor);
        _logicaDispositivoMock.Setup(s => s.GuardarCambios());

        _logicaSesionMock
            .Setup(s => s.ObtenerUsuarioPorToken(It.IsAny<string>()))
            .Returns(dueñoEmpresa);

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarLampara(solicitud, auth);

        _logicaDispositivoMock.Verify(s => s.AgregarLampara(It.Is<CrearLamparasArgs>(a =>
            a.Nombre == argsEsperados.Nombre &&
            a.Modelo == argsEsperados.Modelo &&
            a.Descripcion == argsEsperados.Descripcion &&
            a.Fotografias == argsEsperados.Fotografias &&
            a.Empresa == argsEsperados.Empresa)));

        _logicaDispositivoMock.Verify(s => s.GuardarCambios(), Times.Once);
    }
    #endregion
}
