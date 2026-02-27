using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Notificaciones;
using SmartHome.LogicaNegocio.Notificaciones.Entidades;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.Notificaciones;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ControladorNotificacionTest
{
    private Mock<INotificacionLogica> _logicaNotificacionMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;

    private ControladorNotificacion _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaNotificacionMock = new Mock<INotificacionLogica>(MockBehavior.Default);
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);

        _controlador = new ControladorNotificacion(_logicaNotificacionMock.Object, _logicaSesionMock.Object);
    }

    #region Exito
    [TestMethod]
    public void ObtenerNotificacionesExito()
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

        var token = Guid.NewGuid().ToString();

        _logicaSesionMock.Setup(ls => ls.ObtenerUsuarioPorToken(token)).Returns(usuario);

        var notificacion1 = new Notificacion
        {
            Id = Guid.NewGuid(),
            Evento = "evento",
            DispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), DispositivoId = Guid.NewGuid() },
            FueLeida = true,
            FechaHora = DateTime.Now
        };

        var notificacion2 = new Notificacion
        {
            Id = Guid.NewGuid(),
            Evento = "evento2",
            DispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), DispositivoId = Guid.NewGuid() },
            FueLeida = false,
            FechaHora = DateTime.Now
        };

        var notificaciones = new List<Notificacion>()
        {
            notificacion1,
            notificacion2,
        };

        _logicaNotificacionMock
            .Setup(ln => ln.ObtenerNotificacionesPorUsuario(usuario, It.IsAny<ParametroNotificacionFiltro>()))
            .Returns(notificaciones);

        var filtrado = new ParametroNotificacionFiltro();

        var resultado = _controlador.ObtenerNotificaciones(token, filtrado);

        resultado.Count.Should().Be(2);
    }

    [TestMethod]
    public void MarcarComoLeidasExito()
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

        var token = Guid.NewGuid().ToString();

        _logicaSesionMock.Setup(ls => ls.ObtenerUsuarioPorToken(token)).Returns(usuario);

        var notificacion1 = new Notificacion
        {
            Id = Guid.NewGuid(),
            Evento = "evento",
            DispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), DispositivoId = Guid.NewGuid() },
            FueLeida = true,
            FechaHora = DateTime.Now
        };

        var notificacion2 = new Notificacion
        {
            Id = Guid.NewGuid(),
            Evento = "evento2",
            DispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), DispositivoId = Guid.NewGuid() },
            FueLeida = false,
            FechaHora = DateTime.Now
        };

        var notificaciones = new List<Notificacion>()
        {
            notificacion1,
            notificacion2,
        };

        _logicaNotificacionMock
            .Setup(ln => ln.ObtenerNotificacionesPorUsuario(usuario, It.IsAny<ParametroNotificacionFiltro>()))
            .Returns(notificaciones);

        _logicaNotificacionMock.Setup(ln => ln.MarcarComoLeidas(notificaciones));

        var filtrado = new ParametroNotificacionFiltro();

        _controlador.MarcarNotificacionesComoLeidas(token, filtrado);

        _logicaNotificacionMock.Verify(ln => ln.MarcarComoLeidas(notificaciones), Times.Once);
    }
    #endregion
}
