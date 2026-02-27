using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.Empresas;
using SmartHome.WebApi.Controllers.Empresas.Modelos;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ControladorEmpresaTest
{
    private CrearSolicitudEmpresa _solicitud = null!;
    private Mock<IEmpresaLogica> _logicaEmpresaMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;
    private ControladorEmpresa _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaEmpresaMock = new Mock<IEmpresaLogica>(MockBehavior.Default);
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);
        _controlador = new ControladorEmpresa(_logicaEmpresaMock.Object, _logicaSesionMock.Object);
    }

    #region Crear
    #region Error
    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void CrearConArgsNullLanzaExcepcion()
    {
        var auth = Guid.NewGuid().ToString();

        _controlador.Crear(null, auth);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConNombreNullOVacioLanzaExcepcion(string nombre)
    {
        _solicitud = new CrearSolicitudEmpresa
        {
            Nombre = nombre,
            Logotipo = "logotipo",
            Rut = "rut",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.Crear(_solicitud, auth);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConLogotipoNullOVacioLanzaExcepcion(string logotipo)
    {
        _solicitud = new CrearSolicitudEmpresa
        {
            Nombre = "nombre",
            Logotipo = logotipo,
            Rut = "rut",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.Crear(_solicitud, auth);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConRutNullOVacioLanzaExcepcion(string rut)
    {
        _solicitud = new CrearSolicitudEmpresa
        {
            Nombre = "nombre",
            Logotipo = "logotipo",
            Rut = rut,
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.Crear(_solicitud, auth);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConValidadorNullOVacioLanzaExcepcion(string validador)
    {
        _solicitud = new CrearSolicitudEmpresa
        {
            Nombre = "nombre",
            Logotipo = "logotipo",
            Rut = "rut",
            Validador = validador
        };

        var auth = Guid.NewGuid().ToString();
        _controlador.Crear(_solicitud, auth);
    }
    #endregion
    #region Exito
    [TestMethod]
    public void CrearConSolicitudValidaCreaEmpresa()
    {
        _solicitud = new CrearSolicitudEmpresa
        {
            Nombre = "ORT",
            Logotipo = "/img/ort.png",
            Rut = "038CFP928",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var argsEsperados = new CrearEmpresasArgs(
            _solicitud.Nombre,
            _solicitud.Logotipo,
            _solicitud.Rut,
            _solicitud.Validador);

        _logicaEmpresaMock.Setup(l => l.Agregar(It.IsAny<CrearEmpresasArgs>(), It.IsAny<Usuario>()));
        _logicaEmpresaMock.Setup(l => l.GuardarCambios());

        var auth = Guid.NewGuid().ToString();
        _controlador.Crear(_solicitud, auth);

        _logicaEmpresaMock.Verify(l => l.Agregar(It.Is<CrearEmpresasArgs>(args =>
            args.Nombre == argsEsperados.Nombre &&
            args.Logotipo == argsEsperados.Logotipo &&
            args.Rut == argsEsperados.Rut), It.IsAny<Usuario>()), Times.Once);

        _logicaEmpresaMock.Verify(l => l.GuardarCambios(), Times.Once);
    }
    #endregion
    #endregion
    #region ObtenerTodos
    #region Exito
    [TestMethod]
    public void ObtenerTodosDevuelveEmpresas()
    {
        var empresa1 = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "ORT",
            Logotipo = "/img/ort.png",
            Rut = "038CFP928"
        };

        var empresa2 = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "UTEC",
            Logotipo = "/img/utec.png",
            Rut = "038CFP928"
        };

        var empresas = new List<Empresa> { empresa1, empresa2 };

        var obtenerEmpresas = new ObtenerEmpresasArgs(empresas, 1);

        _logicaEmpresaMock
            .Setup(l => l.ObtenerTodos(It.IsAny<ParametroPaginacion>(),
                It.IsAny<ParametroEmpresaFiltro>()))
            .Returns(obtenerEmpresas);

        var paginacion = new ParametroPaginacion(1, 2);
        var filtrado = new ParametroEmpresaFiltro();

        var resultado = _controlador.ObtenerTodos(paginacion, filtrado);

        resultado.Empresas.Count.Should().Be(empresas.Count);
        resultado.Empresas.Should().Contain(e => e.Id == empresa1.Id.ToString());
        resultado.Empresas.Should().Contain(e => e.Id == empresa2.Id.ToString());
        resultado.Empresas.Should().Contain(e => e.Nombre == empresa1.Nombre);
        resultado.Empresas.Should().Contain(e => e.Nombre == empresa2.Nombre);
        resultado.Empresas.Should().Contain(e => e.Logotipo == empresa1.Logotipo);
        resultado.Empresas.Should().Contain(e => e.Logotipo == empresa2.Logotipo);
        resultado.Empresas.Should().Contain(e => e.Rut == empresa1.Rut);
        resultado.Empresas.Should().Contain(e => e.Rut == empresa2.Rut);
    }
    #endregion
    #endregion

    #region ObtenerValidadores
    [TestMethod]
    public void ObtenerValidadoresDevuelveListaDeInformacionRespuestaValidadores()
    {
        var validadoresMock = new List<string> { "Validador1", "Validador2" };
        _logicaEmpresaMock.Setup(l => l.ObtenerIdentificadoresDeImplementaciones()).Returns(validadoresMock);

        var resultado = _controlador.ObtenerValidadores();

        resultado.Should().NotBeNull();
        resultado.Count.Should().Be(validadoresMock.Count);
        resultado.Should().Contain(r => r.Validador == "Validador1");
        resultado.Should().Contain(r => r.Validador == "Validador2");

        _logicaEmpresaMock.Verify(l => l.ObtenerIdentificadoresDeImplementaciones(), Times.Once);
    }
    #endregion

}
