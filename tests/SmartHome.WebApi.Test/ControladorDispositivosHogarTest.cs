using Moq;
using SmartHome.LogicaNegocio.DispositivosHogar;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Notificaciones;
using SmartHome.LogicaNegocio.Notificaciones.Entidades;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.DispositivosHogar;
using SmartHome.WebApi.Controllers.DispositivosHogar.Modelos;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ControladorDispositivosHogarTest
{
    private Mock<IDispositivoHogarLogica> _logicaDisositivoHogarMock = null!;
    private Mock<INotificacionLogica> _logicaNotificacionMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;
    private CrearSolicitudModificarNombreDh _solicitudModificarNombreDh = null!;

    private ControladorDispositivosHogar _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaDisositivoHogarMock = new Mock<IDispositivoHogarLogica>(MockBehavior.Default);
        _logicaNotificacionMock = new Mock<INotificacionLogica>(MockBehavior.Default);
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);

        _controlador = new ControladorDispositivosHogar(_logicaDisositivoHogarMock.Object, _logicaNotificacionMock.Object, _logicaSesionMock.Object);
    }

    [TestMethod]
    public void SensorSeAbreGeneraNotificacionYLaAgrega()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), HogarId = Guid.NewGuid(), DispositivoId = Guid.NewGuid() };

        var miembros = new List<MiembroHogar>
        {
            new MiembroHogar { Id = Guid.NewGuid() },
            new MiembroHogar { Id = Guid.NewGuid() }
        };

        var notificacion = new Notificacion { Evento = "El sensor se ha abierto", FechaHora = DateTime.Now };

        var notificaciones = new List<Notificacion>() { notificacion };

        _logicaDisositivoHogarMock.Setup(ldh => ldh.EjecutarOperacionDispositivo(dispositivoHogar.Id.ToString(), "Abre")).Returns(true);

        _logicaNotificacionMock.Setup(ln => ln.GenerarNotificaciones("Abierto", dispositivoHogar.Id.ToString()))
            .Returns(notificaciones);

        _controlador.SensorSeAbre(dispositivoHogar.Id.ToString());

        _logicaDisositivoHogarMock.Verify(ldh => ldh.EjecutarOperacionDispositivo(dispositivoHogar.Id.ToString(), "Abre"), Times.Once);
        _logicaNotificacionMock.Verify(ln => ln.GenerarNotificaciones("Abierto", dispositivoHogar.Id.ToString()), Times.Once);
    }

    [TestMethod]
    public void SensorSeCierraGeneraNotificacionYLaAgrega()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), HogarId = Guid.NewGuid(), DispositivoId = Guid.NewGuid() };

        var miembros = new List<MiembroHogar>
        {
            new MiembroHogar { Id = Guid.NewGuid() },
            new MiembroHogar { Id = Guid.NewGuid() }
        };

        var notificacion = new Notificacion { Evento = "El sensor se ha cerrado", FechaHora = DateTime.Now };

        var notificaciones = new List<Notificacion>() { notificacion };

        _logicaDisositivoHogarMock.Setup(ldh => ldh.EjecutarOperacionDispositivo(dispositivoHogar.Id.ToString(), "Cierra")).Returns(true);

        _logicaNotificacionMock.Setup(ln => ln.GenerarNotificaciones("Cerrado", dispositivoHogar.Id.ToString()))
            .Returns(notificaciones);

        _controlador.SensorSeCierra(dispositivoHogar.Id.ToString());

        _logicaDisositivoHogarMock.Verify(ldh => ldh.EjecutarOperacionDispositivo(dispositivoHogar.Id.ToString(), "Cierra"), Times.Once);
        _logicaNotificacionMock.Verify(ln => ln.GenerarNotificaciones("Cerrado", dispositivoHogar.Id.ToString()), Times.Once);
    }

    [TestMethod]
    public void SensorDetectaMovimientoGeneraNotificacionYLaAgrega()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), HogarId = Guid.NewGuid(), DispositivoId = Guid.NewGuid() };

        var miembros = new List<MiembroHogar>
        {
            new MiembroHogar { Id = Guid.NewGuid() },
            new MiembroHogar { Id = Guid.NewGuid() }
        };

        var notificacion = new Notificacion { Evento = "Deteccion movimiento", FechaHora = DateTime.Now };

        var notificaciones = new List<Notificacion>() { notificacion };

        _logicaDisositivoHogarMock.Setup(ldh => ldh.SensorDetectaMovimiento(dispositivoHogar.Id.ToString()));

        _logicaNotificacionMock.Setup(ln => ln.GenerarNotificaciones("Detección movimiento", dispositivoHogar.Id.ToString()))
            .Returns(notificaciones);

        _controlador.SensorDetectaMovimiento(dispositivoHogar.Id.ToString());

        _logicaDisositivoHogarMock.Verify(ldh => ldh.SensorDetectaMovimiento(dispositivoHogar.Id.ToString()), Times.Once);
        _logicaNotificacionMock.Verify(ln => ln.GenerarNotificaciones("Detección movimiento", dispositivoHogar.Id.ToString()), Times.Once);
    }

    [TestMethod]
    public void DispositivoHogarConectadoCuandoElUsuarioEsDueñoLlamaAConectar()
    {
        var dispositivoHogarId = Guid.NewGuid().ToString();
        var usuarioToken = Guid.NewGuid().ToString();
        var usuario = new Usuario { Id = Guid.NewGuid(), Nombre = "Usuario1" };
        var dispositivoHogar = new DispositivoHogar { Id = Guid.Parse(dispositivoHogarId), EstaConectado = false, HogarId = Guid.NewGuid() };

        _logicaSesionMock.Setup(ls => ls.ObtenerUsuarioPorToken(usuarioToken)).Returns(usuario);

        _controlador.DispositivoHogarConectado(dispositivoHogarId, usuarioToken);

        _logicaDisositivoHogarMock.Verify(ldh => ldh.Conectar(dispositivoHogar.Id.ToString(), usuario), Times.Once);
    }

    [TestMethod]
    public void DispositivoHogarConectadoCuandoElUsuarioTienePermisoLlamaAConectar()
    {
        var dispositivoHogarId = Guid.NewGuid().ToString();
        var usuarioToken = Guid.NewGuid().ToString();
        var usuario = new Usuario { Id = Guid.NewGuid(), Nombre = "Usuario2" };
        var dispositivoHogar = new DispositivoHogar { Id = Guid.Parse(dispositivoHogarId), EstaConectado = false, HogarId = Guid.NewGuid() };

        _logicaSesionMock.Setup(ls => ls.ObtenerUsuarioPorToken(usuarioToken)).Returns(usuario);
        _logicaDisositivoHogarMock.Setup(ldh => ldh.ObtenerDispositivoHogarPorId(dispositivoHogarId)).Returns(dispositivoHogar);

        _controlador.DispositivoHogarConectado(dispositivoHogarId, usuarioToken);

        _logicaDisositivoHogarMock.Verify(ldh => ldh.Conectar(dispositivoHogar.Id.ToString(), usuario), Times.Once);
    }

    [TestMethod]
    public void DispositivoHogarDesconectadoCuandoElUsuarioEsDueñoLlamaAConectar()
    {
        var dispositivoHogarId = Guid.NewGuid().ToString();
        var usuarioToken = Guid.NewGuid().ToString();
        var usuario = new Usuario { Id = Guid.NewGuid(), Nombre = "Usuario1" };
        var dispositivoHogar = new DispositivoHogar { Id = Guid.Parse(dispositivoHogarId), EstaConectado = false, HogarId = Guid.NewGuid() };

        _logicaSesionMock.Setup(ls => ls.ObtenerUsuarioPorToken(usuarioToken)).Returns(usuario);
        _logicaDisositivoHogarMock.Setup(ldh => ldh.ObtenerDispositivoHogarPorId(dispositivoHogarId)).Returns(dispositivoHogar);

        _controlador.DispositivoHogarDesconectado(dispositivoHogarId, usuarioToken);

        _logicaDisositivoHogarMock.Verify(ldh => ldh.Desconectar(dispositivoHogar.Id.ToString(), usuario), Times.Once);
    }

    [TestMethod]
    public void DispositivoHogarDesconectadoCuandoElUsuarioTienePermisoLlamaADesconectar()
    {
        var dispositivoHogarId = Guid.NewGuid().ToString();
        var usuarioToken = Guid.NewGuid().ToString();
        var usuario = new Usuario { Id = Guid.NewGuid(), Nombre = "Usuario2" };
        var dispositivoHogar = new DispositivoHogar { Id = Guid.Parse(dispositivoHogarId), EstaConectado = false, HogarId = Guid.NewGuid() };

        _logicaSesionMock.Setup(ls => ls.ObtenerUsuarioPorToken(usuarioToken)).Returns(usuario);
        _logicaDisositivoHogarMock.Setup(ldh => ldh.ObtenerDispositivoHogarPorId(dispositivoHogarId)).Returns(dispositivoHogar);

        _controlador.DispositivoHogarDesconectado(dispositivoHogarId, usuarioToken);

        _logicaDisositivoHogarMock.Verify(ldh => ldh.Desconectar(dispositivoHogar.Id.ToString(), usuario), Times.Once);
    }

    [TestMethod]
    public void CamaraDetectaMovimientoGeneraNotificacionYLaAgrega()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), HogarId = Guid.NewGuid(), DispositivoId = Guid.NewGuid() };

        var miembros = new List<MiembroHogar>
        {
            new MiembroHogar { Id = Guid.NewGuid() },
            new MiembroHogar { Id = Guid.NewGuid() }
        };

        var notificacion = new Notificacion { Evento = "Movimiento detectado", FechaHora = DateTime.Now };

        var notificaciones = new List<Notificacion>() { notificacion };

        _logicaDisositivoHogarMock.Setup(ldh => ldh.CamaraDetectaMovimiento(dispositivoHogar.Id.ToString()));

        _logicaNotificacionMock.Setup(ln => ln.GenerarNotificaciones("Detección movimiento", dispositivoHogar.Id.ToString()))
            .Returns(notificaciones);

        _controlador.CamaraDetectaMovimiento(dispositivoHogar.Id.ToString());

        _logicaDisositivoHogarMock.Verify(ldh => ldh.CamaraDetectaMovimiento(dispositivoHogar.Id.ToString()), Times.Once);
        _logicaNotificacionMock.Verify(ln => ln.GenerarNotificaciones("Detección movimiento", dispositivoHogar.Id.ToString()), Times.Once);
    }

    [TestMethod]
    public void CamaraDetectaPersonaGeneraNotificacionYLaAgrega()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), HogarId = Guid.NewGuid(), DispositivoId = Guid.NewGuid() };

        var miembros = new List<MiembroHogar>
        {
            new MiembroHogar { Id = Guid.NewGuid() },
            new MiembroHogar { Id = Guid.NewGuid() }
        };

        var notificacion = new Notificacion { Evento = "Persona detectada", FechaHora = DateTime.Now };

        var notificaciones = new List<Notificacion>() { notificacion };

        _logicaDisositivoHogarMock.Setup(ldh => ldh.CamaraDetectaPersona(dispositivoHogar.Id.ToString()));

        _logicaNotificacionMock.Setup(ln => ln.GenerarNotificaciones("Detección persona", dispositivoHogar.Id.ToString()))
            .Returns(notificaciones);

        _controlador.CamaraDetectaPersona(dispositivoHogar.Id.ToString());

        _logicaDisositivoHogarMock.Verify(ldh => ldh.CamaraDetectaPersona(dispositivoHogar.Id.ToString()), Times.Once);
        _logicaNotificacionMock.Verify(ln => ln.GenerarNotificaciones("Detección persona", dispositivoHogar.Id.ToString()), Times.Once);
    }

    [TestMethod]
    public void LamparaEncendidaGeneraNotificacionYLaAgrega()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), HogarId = Guid.NewGuid(), DispositivoId = Guid.NewGuid() };

        var miembros = new List<MiembroHogar>
        {
            new MiembroHogar { Id = Guid.NewGuid() },
            new MiembroHogar { Id = Guid.NewGuid() }
        };

        var notificacion = new Notificacion { Evento = "Encendida", FechaHora = DateTime.Now };

        var notificaciones = new List<Notificacion>() { notificacion };

        _logicaDisositivoHogarMock.Setup(ldh => ldh.EjecutarOperacionDispositivo(dispositivoHogar.Id.ToString(), "Encender")).Returns(true);

        _logicaNotificacionMock.Setup(ln => ln.GenerarNotificaciones("Encendida", dispositivoHogar.Id.ToString()))
            .Returns(notificaciones);

        _controlador.EncenderLampara(dispositivoHogar.Id.ToString());

        _logicaDisositivoHogarMock.Verify(ldh => ldh.EjecutarOperacionDispositivo(dispositivoHogar.Id.ToString(), "Encender"), Times.Once);
        _logicaNotificacionMock.Verify(ln => ln.GenerarNotificaciones("Encendida", dispositivoHogar.Id.ToString()), Times.Once);
    }

    [TestMethod]
    public void LamparaApagadaGeneraNotificacionYLaAgrega()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), HogarId = Guid.NewGuid(), DispositivoId = Guid.NewGuid() };

        var miembros = new List<MiembroHogar>
        {
            new MiembroHogar { Id = Guid.NewGuid() },
            new MiembroHogar { Id = Guid.NewGuid() }
        };

        var notificacion = new Notificacion { Evento = "Apagada", FechaHora = DateTime.Now };

        var notificaciones = new List<Notificacion>() { notificacion };

        _logicaDisositivoHogarMock.Setup(ldh => ldh.EjecutarOperacionDispositivo(dispositivoHogar.Id.ToString(), "Apagar")).Returns(true);

        _logicaNotificacionMock.Setup(ln => ln.GenerarNotificaciones("Apagada", dispositivoHogar.Id.ToString()))
            .Returns(notificaciones);

        _controlador.ApagarLampara(dispositivoHogar.Id.ToString());

        _logicaDisositivoHogarMock.Verify(ldh => ldh.EjecutarOperacionDispositivo(dispositivoHogar.Id.ToString(), "Apagar"), Times.Once);
        _logicaNotificacionMock.Verify(ln => ln.GenerarNotificaciones("Apagada", dispositivoHogar.Id.ToString()), Times.Once);
    }

    [TestMethod]
    public void ApagarLamparaApagadaNoGeneraNotificacion()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), HogarId = Guid.NewGuid(), DispositivoId = Guid.NewGuid() };

        _logicaDisositivoHogarMock.Setup(ldh => ldh.EjecutarOperacionDispositivo(dispositivoHogar.Id.ToString(), "Apagar")).Returns(false);

        _controlador.ApagarLampara(dispositivoHogar.Id.ToString());

        _logicaNotificacionMock.Verify(n => n.GenerarNotificaciones(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void EncenderLamparaEncendidaNoGeneraNotificacion()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), HogarId = Guid.NewGuid(), DispositivoId = Guid.NewGuid() };

        _logicaDisositivoHogarMock.Setup(ldh => ldh.EjecutarOperacionDispositivo(dispositivoHogar.Id.ToString(), "Encender")).Returns(false);

        _controlador.EncenderLampara(dispositivoHogar.Id.ToString());

        _logicaNotificacionMock.Verify(n => n.GenerarNotificaciones(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void AbrirSensorAbiertoNoGeneraNotificacion()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), HogarId = Guid.NewGuid(), DispositivoId = Guid.NewGuid() };

        _logicaDisositivoHogarMock.Setup(ldh => ldh.EjecutarOperacionDispositivo(dispositivoHogar.Id.ToString(), "Abre")).Returns(false);

        _controlador.SensorSeAbre(dispositivoHogar.Id.ToString());

        _logicaNotificacionMock.Verify(n => n.GenerarNotificaciones(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void CerrarSensorCerradoNoGeneraNotificacion()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), HogarId = Guid.NewGuid(), DispositivoId = Guid.NewGuid() };

        _logicaDisositivoHogarMock.Setup(ldh => ldh.EjecutarOperacionDispositivo(dispositivoHogar.Id.ToString(), "Cierra")).Returns(false);

        _controlador.SensorSeCierra(dispositivoHogar.Id.ToString());

        _logicaNotificacionMock.Verify(n => n.GenerarNotificaciones(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void ActualizarNombreDispositivoHogar()
    {
        var dispositivoHogarId = Guid.NewGuid().ToString();
        var usuarioToken = Guid.NewGuid().ToString();

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(dispositivoHogarId),
            Nombre = "NombreAnterior",
            HogarId = Guid.NewGuid()
        };

        _logicaSesionMock
            .Setup(ls => ls.ObtenerUsuarioPorToken(usuarioToken))
            .Returns(usuario);

        _solicitudModificarNombreDh = new CrearSolicitudModificarNombreDh
        {
            Nombre = "Porton frente"
        };

        _controlador.ActualizarNombreDispositivoHogar(dispositivoHogarId, _solicitudModificarNombreDh, usuarioToken);

        _logicaDisositivoHogarMock.Verify(ldh => ldh.ActualizarNombreDispositivoHogar(dispositivoHogarId, "Porton frente", usuario), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ActualizarNombreDispositivoHogarCuandoSinNombreUsuarioLanzaExcepcion()
    {
        var dispositivoHogarId = Guid.NewGuid().ToString();
        var usuarioToken = Guid.NewGuid().ToString();

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(dispositivoHogarId),
            Nombre = "NombreAnterior",
            HogarId = Guid.NewGuid()
        };

        _logicaSesionMock
            .Setup(ls => ls.ObtenerUsuarioPorToken(usuarioToken))
            .Returns(usuario);

        _solicitudModificarNombreDh = new CrearSolicitudModificarNombreDh { };

        _controlador.ActualizarNombreDispositivoHogar(dispositivoHogarId, _solicitudModificarNombreDh, usuarioToken);

        _logicaDisositivoHogarMock.Verify(ldh => ldh.ActualizarNombreDispositivoHogar(dispositivoHogarId, "Porton frente", usuario), Times.Once);
    }
}
