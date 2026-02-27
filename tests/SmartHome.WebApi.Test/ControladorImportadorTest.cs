using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.Importadores;
using SmartHome.WebApi.Controllers.Importadores.Modelos;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ControladorImportadorTest
{
    private Mock<IDispositivoLogica> _logicaDispositivoMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;
    private ControladorImportador _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaDispositivoMock = new Mock<IDispositivoLogica>(MockBehavior.Default);
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);
        _controlador = new ControladorImportador(_logicaDispositivoMock.Object, _logicaSesionMock.Object);
    }

    #region Importar
    [TestMethod]
    public void ImportarDispositivosExito()
    {
        var dueñoEmpresa = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "pepe",
            Apellido = "Gomez",
            Email = "pepe@gmail.com",
            Contraseña = "pepe1234.",
            Empresa = new Empresa() { Nombre = "nombre", Logotipo = "logotipo", Rut = "rut", Validador = "Reflection.ValidadorAulas6Letras" }
        };

        _logicaSesionMock
            .Setup(s => s.ObtenerUsuarioPorToken(It.IsAny<string>()))
            .Returns(dueñoEmpresa);

        var rutaJson = "dispositivos.json";
        var identificadorImportador = "ImportadorJson";

        _logicaDispositivoMock
            .Setup(l => l.ImportarDispositivos(rutaJson, identificadorImportador, dueñoEmpresa.Empresa));

        _logicaDispositivoMock
            .Setup(l => l.GuardarCambios());

        var solicitud = new CrearSolicitudImportacion
        {
            Ruta = rutaJson,
            IdentificadorImportador = identificadorImportador
        };

        _controlador.ImportarDispositivos(solicitud, "auth");

        _logicaDispositivoMock.Verify(l => l.ImportarDispositivos(rutaJson, identificadorImportador, dueñoEmpresa.Empresa), Times.Once);
        _logicaDispositivoMock.Verify(l => l.GuardarCambios(), Times.Once);
    }
    #endregion

    #region ObtenerImportadores
    [TestMethod]
    public void ObtenerImportadoresDevuelveListaDeInformacionRespuestaImportadores()
    {
        var importadoresMock = new List<string> { "Importador1", "Importador2" };
        _logicaDispositivoMock.Setup(l => l.ObtenerIdentificadoresDeImportadores()).Returns(importadoresMock);

        var resultado = _controlador.ObtenerImportadores();

        resultado.Should().NotBeNull();
        resultado.Count.Should().Be(importadoresMock.Count);
        resultado.Should().Contain(r => r.Importador == "Importador1");
        resultado.Should().Contain(r => r.Importador == "Importador2");

        _logicaDispositivoMock.Verify(l => l.ObtenerIdentificadoresDeImportadores(), Times.Once);
    }
    #endregion
}
