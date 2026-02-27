using Moq;
using SmartHome.LogicaNegocio.Cuartos;
using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.DispositivosHogar;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.Cuartos;
using SmartHome.WebApi.Controllers.Cuartos.Modelos;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ControladorCuartoTest
{
    private CrearSolicitudAgregarDispositivoHogar _solicitud = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;
    private Mock<IDispositivoHogarLogica> _logicaDispositivoHogarMock = null!;
    private Mock<ICuartoLogica> _logicaCuartoMock = null!;
    private ControladorCuarto _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);
        _logicaDispositivoHogarMock = new Mock<IDispositivoHogarLogica>(MockBehavior.Default);
        _logicaCuartoMock = new Mock<ICuartoLogica>(MockBehavior.Default);
        _controlador = new ControladorCuarto(_logicaSesionMock.Object, _logicaDispositivoHogarMock.Object, _logicaCuartoMock.Object);
    }

    #region Exito
    [TestMethod]
    public void AgregarDispositivoACuartoConDatosValidos()
    {
        var token = Guid.NewGuid().ToString();

        var dispositivoHogarId = Guid.NewGuid();

        _solicitud = new CrearSolicitudAgregarDispositivoHogar
        {
            DispositivoHogarId = dispositivoHogarId.ToString()
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "usuario",
            Apellido = "x",
            Email = "email1@gmail.com"
        };

        var cuarto = new Cuarto
        {
            Id = Guid.NewGuid(),
            Nombre = "cuarto",
            HogarId = Guid.NewGuid()
        };

        var dispositivoHogar = new DispositivoHogar
        {
            Id = dispositivoHogarId,
            DispositivoId = Guid.NewGuid(),
            HogarId = Guid.NewGuid()
        };

        _logicaSesionMock
            .Setup(s => s.ObtenerUsuarioPorToken(token))
            .Returns(usuario);

        _logicaCuartoMock
            .Setup(s => s.ObtenerPorId(cuarto.Id.ToString()))
            .Returns(cuarto);

        _logicaDispositivoHogarMock
            .Setup(s => s.AgregarACuarto(dispositivoHogar.Id.ToString(), cuarto, usuario));

        _controlador.AgregarDispositivoHogar(_solicitud, cuarto.Id.ToString(), token);

        _logicaDispositivoHogarMock.Verify(s => s.AgregarACuarto(dispositivoHogar.Id.ToString(), cuarto, usuario), Times.Once);
    }
    #endregion
}
