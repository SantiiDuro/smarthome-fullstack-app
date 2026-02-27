using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.WebApi.Controllers.Dispositivos;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ControladorDispositivoTest
{
    private Mock<IDispositivoLogica> _logicaDispositivoMock = null!;
    private Mock<IEmpresaLogica> _logicaEmpresaMock = null!;
    private ControladorDispositivo _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaDispositivoMock = new Mock<IDispositivoLogica>(MockBehavior.Default);
        _logicaEmpresaMock = new Mock<IEmpresaLogica>(MockBehavior.Default);
        _controlador = new ControladorDispositivo(_logicaDispositivoMock.Object, _logicaEmpresaMock.Object);
    }

    #region ObtenerTodos
    #region Exito
    [TestMethod]
    public void ObtenerTodosDevuelveDispositivos()
    {
        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9"
        };

        var fotografia1 = new FotografiaDispositivo
        {
            EsPrincipal = true,
            Url = "fotografia"
        };

        var fotografias1 = new List<FotografiaDispositivo>
        {
            fotografia1
        };

        var dispositivo1 = new Dispositivo
        {
            Id = Guid.NewGuid(),
            Nombre = "Camara de seguridad",
            Modelo = "AQWSDE",
            Descripcion = "Muy buen producto",
            EmpresaId = empresa.Id,
            Fotografias = fotografias1,
            Tipo = TipoDispositivo.Camara
        };

        var fotografia2 = new FotografiaDispositivo
        {
            EsPrincipal = true,
            Url = "fotografia2"
        };

        var fotografias2 = new List<FotografiaDispositivo>
        {
            fotografia2
        };

        var dispositivo2 = new Dispositivo
        {
            Id = Guid.NewGuid(),
            Nombre = "Sensor de movimiento",
            Modelo = "AQWSDS",
            Descripcion = "Muy buen producto",
            EmpresaId = empresa.Id,
            Fotografias = fotografias2,
            Tipo = TipoDispositivo.SensorVentana
        };

        var dispositivos = new List<Dispositivo> { dispositivo1, dispositivo2 };

        var obtenerDispositivos = new ObtenerDispositivosArgs(dispositivos, 1);

        _logicaDispositivoMock
            .Setup(l => l.ObtenerTodos(It.IsAny<ParametroPaginacion>(),
                It.IsAny<ParametroDispositivoFiltro>()))
            .Returns(obtenerDispositivos);

        _logicaEmpresaMock
            .Setup(l => l.ObtenerPorId(It.IsAny<Guid>())).Returns(empresa);

        var paginacion = new ParametroPaginacion(1, 2);
        var filtrado = new ParametroDispositivoFiltro();

        var resultado = _controlador.ObtenerTodos(paginacion, filtrado);

        dispositivos.Should().Contain(dispositivo1);
        dispositivos.Should().Contain(dispositivo2);

        resultado.Should().NotBeNull();
        resultado.Dispositivos.Should().HaveCount(2);
        resultado.Dispositivos.Should().Contain(d => d.Nombre == dispositivo1.Nombre);
        resultado.Dispositivos.Should().Contain(d => d.Nombre == dispositivo2.Nombre);
        resultado.Dispositivos.Should().Contain(d => d.Modelo == dispositivo1.Modelo);
        resultado.Dispositivos.Should().Contain(d => d.Modelo == dispositivo2.Modelo);
        resultado.Dispositivos.Should().Contain(d => d.Descripcion == dispositivo1.Descripcion);
        resultado.Dispositivos.Should().Contain(d => d.Descripcion == dispositivo2.Descripcion);
        resultado.Dispositivos.Should().Contain(d => d.FotoPrincipal == "fotografia");
        resultado.Dispositivos.Should().Contain(d => d.FotoPrincipal == "fotografia2");
        resultado.Dispositivos.Should().Contain(d => d.NombreEmpresa == empresa.Nombre);
        resultado.Dispositivos.Should().Contain(d => d.NombreEmpresa == empresa.Nombre);
        resultado.Dispositivos.Should().Contain(d => d.Tipo == dispositivo1.Tipo.ToString());
        resultado.Dispositivos.Should().Contain(d => d.Tipo == dispositivo2.Tipo.ToString());
    }
    #endregion
    #endregion
    #region ObtenerTiposDeDispositivos
    #region Exito
    [TestMethod]
    public void ObtenerTiposDeDispositivosDevuelveTiposDeDispositivos()
    {
        var dispositivo1 = new Dispositivo
        {
            Id = Guid.NewGuid(),
            Tipo = TipoDispositivo.SensorVentana,
            Nombre = "Camara de seguridad",
            Modelo = "AQWSDE",
            Descripcion = "Descripcion",
            Fotografias = [],
            EmpresaId = Guid.NewGuid()
        };

        var dispositivo2 = new Dispositivo
        {
            Id = Guid.NewGuid(),
            Tipo = TipoDispositivo.Camara,
            Nombre = "Camara de seguridad",
            Modelo = "AQWSDS",
            Descripcion = "Descripcion",
            Fotografias = [],
            EmpresaId = Guid.NewGuid()
        };

        var dispositivos = new List<Dispositivo>
        {
            dispositivo1,
            dispositivo2
        };

        _logicaDispositivoMock
            .Setup(m => m.ObtenerTiposDeDispositivos())
            .Returns(dispositivos.ConvertAll(d => d.Tipo));

        var resultado = _controlador.ObtenerTiposDeDispositivos();

        var tiposEsperados = dispositivos.Select(d => d.Tipo.ToString()).ToList();
        var tiposObtenidos = resultado.Select(r => r.Tipo).ToList();

        tiposObtenidos.Should().BeEquivalentTo(tiposEsperados);

        _logicaDispositivoMock.Verify(m => m.ObtenerTiposDeDispositivos(), Times.Once);
    }

    #endregion
}
#endregion
