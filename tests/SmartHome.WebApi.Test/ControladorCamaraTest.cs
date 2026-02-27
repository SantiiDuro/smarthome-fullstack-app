using Moq;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.Camaras;
using SmartHome.WebApi.Controllers.Camaras.Modelos;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ControladorCamaraTest
{
    private CrearSolicitudCamara _solicitud = null!;
    private Mock<IDispositivoLogica> _logicaDispositivoMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;
    private ControladorCamara _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaDispositivoMock = new Mock<IDispositivoLogica>(MockBehavior.Default);
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);
        _controlador = new ControladorCamara(_logicaDispositivoMock.Object, _logicaSesionMock.Object);
    }
    #region Crear
    #region Error
    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void CrearConArgsNullLanzaExcepcion()
    {
        var auth = Guid.NewGuid().ToString();

        _controlador.AgregarCamara(null, auth);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConNombreNullOVacioLanzaExcepcion(string nombre)
    {
        _solicitud = new CrearSolicitudCamara()
        {
            Nombre = nombre,
            Modelo = "AQWSDE",
            Descripcion = "camara de vigilancia",
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = true }
            ],
            DetectaMovimiento = true,
            DetectaPersona = true,
            UsoExterior = false,
            UsoInterior = true
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarCamara(_solicitud, auth);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConModeloNullOVacioLanzaExcepcion(string modelo)
    {
        _solicitud = new CrearSolicitudCamara()
        {
            Nombre = "camara",
            Modelo = modelo,
            Descripcion = "camara de vigilancia",
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = true }
            ],
            DetectaMovimiento = true,
            DetectaPersona = true,
            UsoExterior = false,
            UsoInterior = true
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarCamara(_solicitud, auth);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConDescripcionNullOVacioLanzaExcepcion(string descripcion)
    {
        _solicitud = new CrearSolicitudCamara
        {
            Nombre = "camara",
            Modelo = "AQWSDE",
            Descripcion = descripcion,
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = true }
            ],
            DetectaMovimiento = true,
            DetectaPersona = true,
            UsoExterior = false,
            UsoInterior = true
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarCamara(_solicitud, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConFotografiasNullLanzaExcepcion()
    {
        _solicitud = new CrearSolicitudCamara()
        {
            Nombre = "camara",
            Modelo = "AQWSDE",
            Descripcion = "camara de vigilancia",
            Fotografias = null!,
            DetectaMovimiento = true,
            DetectaPersona = true,
            UsoExterior = false,
            UsoInterior = true
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarCamara(_solicitud, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearConFotografiasSinPrincipalLanzaExcepcion()
    {
        _solicitud = new CrearSolicitudCamara()
        {
            Nombre = "camara",
            Modelo = "AQWSDE",
            Descripcion = "camara de movimiento",
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = false }
            ],
            DetectaMovimiento = true,
            DetectaPersona = true,
            UsoExterior = false,
            UsoInterior = true
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarCamara(_solicitud, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearConFotografiasMultiplesPrincipalesLanzaExcepcion()
    {
        _solicitud = new CrearSolicitudCamara()
        {
            Nombre = "camara",
            Modelo = "AQWSDE",
            Descripcion = "camara de vigilancia",
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = true },

                new FotografiaDispositivo { Url = "/downloads/c410v2", EsPrincipal = true }
            ],
            DetectaMovimiento = true,
            DetectaPersona = true,
            UsoExterior = false,
            UsoInterior = true
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarCamara(_solicitud, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CamaraConUsuarioSinEmpresaLanzaExcepcion()
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

        _solicitud = new CrearSolicitudCamara()
        {
            Nombre = "camara",
            Modelo = "AQWSDE",
            Descripcion = "camara de vigilancia",
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = true }
            ],
            DetectaMovimiento = true,
            DetectaPersona = true,
            UsoExterior = false,
            UsoInterior = true
        };

        var auth = Guid.NewGuid().ToString();

        _logicaSesionMock
            .Setup(s => s.ObtenerUsuarioPorToken(It.IsAny<string>()))
            .Returns(dueñoEmpresa);

        _controlador.AgregarCamara(_solicitud, auth);

        _logicaSesionMock.Verify(s => s.ObtenerUsuarioPorToken(It.IsAny<string>()), Times.Once);
    }
    #endregion
    #region Exito
    [TestMethod]
    public void CrearConSolicitudValidaAgregaCamara()
    {
        var solicitud = new CrearSolicitudCamara()
        {
            Nombre = "camara",
            Modelo = "AQWSDE",
            Descripcion = "camara de vigilancia",
            Fotografias =
            [
                new FotografiaDispositivo { Url = "/downloads/c410", EsPrincipal = true }
            ],
            DetectaMovimiento = true,
            DetectaPersona = true,
            UsoExterior = false,
            UsoInterior = true
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

        var argsEsperados = new CrearCamarasArgs(
            solicitud.Nombre,
            solicitud.Modelo,
            solicitud.Descripcion,
            solicitud.Fotografias,
            dueñoEmpresa.Empresa,
            solicitud.DetectaMovimiento,
            solicitud.DetectaPersona,
            solicitud.UsoExterior,
            solicitud.UsoInterior);

        var camara = new Dispositivo
        {
            Nombre = solicitud.Nombre,
            Modelo = solicitud.Modelo,
            Descripcion = solicitud.Descripcion,
            Fotografias = solicitud.Fotografias,
            DetectaMovimiento = solicitud.DetectaMovimiento,
            DetectaPersona = solicitud.DetectaPersona,
            UsoExterior = solicitud.UsoExterior,
            UsoInterior = solicitud.UsoInterior
        };

        _logicaDispositivoMock.Setup(s => s.AgregarCamara(It.IsAny<CrearCamarasArgs>())).Returns(camara);
        _logicaDispositivoMock.Setup(s => s.GuardarCambios());

        _logicaSesionMock
            .Setup(s => s.ObtenerUsuarioPorToken(It.IsAny<string>()))
            .Returns(dueñoEmpresa);

        var auth = Guid.NewGuid().ToString();
        _controlador.AgregarCamara(solicitud, auth);

        _logicaDispositivoMock.Verify(s => s.AgregarCamara(It.Is<CrearCamarasArgs>(a =>
            a.Nombre == argsEsperados.Nombre &&
            a.Modelo == argsEsperados.Modelo &&
            a.Descripcion == argsEsperados.Descripcion &&
            a.Fotografias == argsEsperados.Fotografias &&
            a.Empresa == argsEsperados.Empresa)));

        _logicaDispositivoMock.Verify(s => s.GuardarCambios(), Times.Once);
    }
    #endregion
}
#endregion
