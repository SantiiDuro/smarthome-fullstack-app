using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.DispositivosHogar;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Notificaciones;
using SmartHome.LogicaNegocio.Notificaciones.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public sealed class NotificacionTest
{
    private Mock<INotificacionRepositorio> _logicaNotificacionMock = null!;
    private Mock<IHogarLogica> _logicaHogarMock = null!;
    private Mock<IDispositivoHogarLogica> _logicaDispositivoHogarMock = null!;
    private NotificacionLogica _servicio = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaNotificacionMock = new Mock<INotificacionRepositorio>(MockBehavior.Strict);
        _logicaHogarMock = new Mock<IHogarLogica>(MockBehavior.Strict);
        _logicaDispositivoHogarMock = new Mock<IDispositivoHogarLogica>(MockBehavior.Strict);
        _servicio = new NotificacionLogica(_logicaNotificacionMock.Object, _logicaHogarMock.Object, _logicaDispositivoHogarMock.Object);
    }

    #region Error
    [TestMethod]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearNotificacionConDispositivoHogarNullLanzaExcepcion(DispositivoHogar dh)
    {
        new CrearNotificacionesArgs(
            dh,
            new MiembroHogar(),
            "Evento");
    }

    [TestMethod]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearNotificacionConMiembroNullLanzaExcepcion(MiembroHogar miembro)
    {
        new CrearNotificacionesArgs(
            new DispositivoHogar(),
            miembro,
            "Evento");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearNotificacionConEventoNullOVacioLanzaExcepcion(string evento)
    {
        new CrearNotificacionesArgs(
            new DispositivoHogar(),
            new MiembroHogar(),
            evento);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void GenerarNotificacionSensorAbiertoNoEnLineaLanzaExcepcion()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), EstaConectado = false, HogarId = Guid.NewGuid() };
        var miembro = new MiembroHogar();

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerDispositivoHogarPorId(dispositivoHogar.Id.ToString()))
            .Returns(dispositivoHogar);

        _logicaHogarMock
            .Setup(h => h.ObtenerMiembrosHogarConNotificaciones(dispositivoHogar.HogarId))
            .Returns([miembro]);

        _servicio.GenerarNotificaciones("Abierto", dispositivoHogar.Id.ToString());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void GenerarNotificacionSensorCerradoNoEnLineaLanzaExcepcion()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), EstaConectado = false, HogarId = Guid.NewGuid() };
        var miembro = new MiembroHogar();

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerDispositivoHogarPorId(dispositivoHogar.Id.ToString()))
            .Returns(dispositivoHogar);

        _logicaHogarMock
            .Setup(h => h.ObtenerMiembrosHogarConNotificaciones(dispositivoHogar.HogarId))
            .Returns([miembro]);

        _servicio.GenerarNotificaciones("Cerrado", dispositivoHogar.Id.ToString());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void GenerarNotificacionCamaraDetectaMovimientoNoEnLineaLanzaExcepcion()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), EstaConectado = false, HogarId = Guid.NewGuid() };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerDispositivoHogarPorId(dispositivoHogar.Id.ToString()))
            .Returns(dispositivoHogar);

        _logicaHogarMock
            .Setup(h => h.ObtenerMiembrosHogarConNotificaciones(dispositivoHogar.HogarId))
            .Returns([]);

        _servicio.GenerarNotificaciones("Detección movimiento", dispositivoHogar.Id.ToString());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void GenerarNotificacionCamaraDetectaPersonaNoEnLineaLanzaExcepcion()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), EstaConectado = false, HogarId = Guid.NewGuid() };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerDispositivoHogarPorId(dispositivoHogar.Id.ToString()))
            .Returns(dispositivoHogar);

        _logicaHogarMock
            .Setup(h => h.ObtenerMiembrosHogarConNotificaciones(dispositivoHogar.HogarId))
            .Returns([]);

        _servicio.GenerarNotificaciones("Detección persona", dispositivoHogar.Id.ToString());
    }
    #endregion

    #region Exito
    [TestMethod]
    public void CrearNotificacionExito()
    {
        var args = new CrearNotificacionesArgs(
            new DispositivoHogar(),
            new MiembroHogar(),
            "Apertura");

        _logicaNotificacionMock
            .Setup(i => i.Agregar(It.Is<Notificacion>(n =>
                n.Id != Guid.Empty &&
                n.DispositivoHogar == args.DispositivoHogar &&
                n.Miembro == args.Miembro &&
                n.FueLeida == args.FueLeida &&
                n.FechaHora == args.FechaHora)));

        _logicaNotificacionMock.Setup(i => i.GuardarCambios());

        var respuesta = _servicio.Agregar(args);
        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBeEmpty();
        respuesta.Id.Should().NotBe(Guid.Empty);

        respuesta.DispositivoHogar.Should().Be(args.DispositivoHogar);
        respuesta.Miembro.Should().Be(args.Miembro);
        respuesta.FueLeida.Should().Be(args.FueLeida);
        respuesta.FechaHora.Should().Be(args.FechaHora);
    }

    [TestMethod]
    public void GenerarNotificacionSensorAbiertoCreaNotificacionCorrectamente()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), EstaConectado = true, HogarId = Guid.NewGuid() };
        var miembro = new MiembroHogar();

        var notificacionEsperada = new Notificacion
        {
            DispositivoHogar = dispositivoHogar,
            Miembro = miembro,
            Evento = "Abierto"
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerDispositivoHogarPorId(dispositivoHogar.Id.ToString()))
            .Returns(dispositivoHogar);

        _logicaHogarMock
            .Setup(h => h.ObtenerMiembrosHogarConNotificaciones(dispositivoHogar.HogarId))
            .Returns([miembro]);

        _logicaNotificacionMock
            .Setup(i => i.Agregar(It.Is<Notificacion>(n =>
                n.Id != Guid.Empty &&
                n.DispositivoHogar == notificacionEsperada.DispositivoHogar &&
                n.Miembro == notificacionEsperada.Miembro &&
                n.Evento == notificacionEsperada.Evento)));

        _logicaNotificacionMock
            .Setup(n => n.GuardarCambios());

        _logicaHogarMock
            .Setup(h => h.ActualizarNotificacionesDeMiembros(dispositivoHogar.HogarId, It.IsAny<List<Notificacion>>()));

        var resultado = _servicio.GenerarNotificaciones("Abierto", dispositivoHogar.Id.ToString());

        resultado.Should().NotBeNull();
        resultado[0].DispositivoHogar.Should().Be(dispositivoHogar);
        resultado[0].Miembro.Should().Be(miembro);
        resultado[0].Evento.Should().Be("Abierto");
    }

    [TestMethod]
    public void GenerarNotificacionSensorCerradoCreaNotificacionCorrectamente()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), EstaConectado = true, HogarId = Guid.NewGuid() };
        var miembro = new MiembroHogar();

        var notificacionEsperada = new Notificacion
        {
            DispositivoHogar = dispositivoHogar,
            Miembro = miembro,
            Evento = "Cerrado"
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerDispositivoHogarPorId(dispositivoHogar.Id.ToString()))
            .Returns(dispositivoHogar);

        _logicaHogarMock
            .Setup(h => h.ObtenerMiembrosHogarConNotificaciones(dispositivoHogar.HogarId))
            .Returns([miembro]);

        _logicaNotificacionMock
            .Setup(i => i.Agregar(It.Is<Notificacion>(n =>
                n.Id != Guid.Empty &&
                n.DispositivoHogar == notificacionEsperada.DispositivoHogar &&
                n.Miembro == notificacionEsperada.Miembro &&
                n.Evento == notificacionEsperada.Evento)));

        _logicaNotificacionMock
            .Setup(n => n.GuardarCambios());

        _logicaHogarMock
            .Setup(h => h.ActualizarNotificacionesDeMiembros(dispositivoHogar.HogarId, It.IsAny<List<Notificacion>>()));

        var resultado = _servicio.GenerarNotificaciones("Cerrado", dispositivoHogar.Id.ToString());

        resultado.Should().NotBeNull();
        resultado[0].DispositivoHogar.Should().Be(dispositivoHogar);
        resultado[0].Miembro.Should().Be(miembro);
        resultado[0].Evento.Should().Be("Cerrado");
    }

    [TestMethod]
    public void GenerarNotificacionSensorMovimientoCreaNotificacionCorrectamente()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), EstaConectado = true, HogarId = Guid.NewGuid() };
        var miembro = new MiembroHogar();

        var notificacionEsperada = new Notificacion
        {
            DispositivoHogar = dispositivoHogar,
            Miembro = miembro,
            Evento = "Detección movimiento"
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerDispositivoHogarPorId(dispositivoHogar.Id.ToString()))
            .Returns(dispositivoHogar);

        _logicaHogarMock
            .Setup(h => h.ObtenerMiembrosHogarConNotificaciones(dispositivoHogar.HogarId))
            .Returns([miembro]);

        _logicaNotificacionMock
            .Setup(i => i.Agregar(It.Is<Notificacion>(n =>
                n.Id != Guid.Empty &&
                n.DispositivoHogar == notificacionEsperada.DispositivoHogar &&
                n.Miembro == notificacionEsperada.Miembro &&
                n.Evento == notificacionEsperada.Evento)));

        _logicaNotificacionMock
            .Setup(n => n.GuardarCambios());

        _logicaHogarMock
            .Setup(h => h.ActualizarNotificacionesDeMiembros(dispositivoHogar.HogarId, It.IsAny<List<Notificacion>>()));

        var resultado = _servicio.GenerarNotificaciones("Detección movimiento", dispositivoHogar.Id.ToString());

        resultado.Should().NotBeNull();
        resultado[0].DispositivoHogar.Should().Be(dispositivoHogar);
        resultado[0].Miembro.Should().Be(miembro);
        resultado[0].Evento.Should().Be("Detección movimiento");
    }

    [TestMethod]
    public void GenerarNotificacionCamaraDetectaMovimientoCreaNotificacionCorrectamente()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), EstaConectado = true, HogarId = Guid.NewGuid() };
        var miembro = new MiembroHogar();

        var notificacionEsperada = new Notificacion
        {
            DispositivoHogar = dispositivoHogar,
            Miembro = miembro,
            Evento = "Detección movimiento"
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerDispositivoHogarPorId(dispositivoHogar.Id.ToString()))
            .Returns(dispositivoHogar);

        _logicaHogarMock
            .Setup(h => h.ObtenerMiembrosHogarConNotificaciones(dispositivoHogar.HogarId))
            .Returns([miembro]);

        _logicaNotificacionMock
            .Setup(i => i.Agregar(It.Is<Notificacion>(n =>
                n.Id != Guid.Empty &&
                n.DispositivoHogar == notificacionEsperada.DispositivoHogar &&
                n.Miembro == notificacionEsperada.Miembro &&
                n.Evento == notificacionEsperada.Evento)));

        _logicaNotificacionMock
            .Setup(n => n.GuardarCambios());

        _logicaHogarMock
            .Setup(h => h.ActualizarNotificacionesDeMiembros(dispositivoHogar.HogarId, It.IsAny<List<Notificacion>>()));

        var resultado = _servicio.GenerarNotificaciones("Detección movimiento", dispositivoHogar.Id.ToString());

        resultado.Should().NotBeNull();
        resultado[0].DispositivoHogar.Should().Be(dispositivoHogar);
        resultado[0].Miembro.Should().Be(miembro);
        resultado[0].Evento.Should().Be("Detección movimiento");
    }

    [TestMethod]
    public void GenerarNotificacionCamaraDetectaPersonaCreaNotificacionCorrectamente()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), EstaConectado = true, HogarId = Guid.NewGuid() };
        var miembro = new MiembroHogar();

        var notificacionEsperada = new Notificacion
        {
            DispositivoHogar = dispositivoHogar,
            Miembro = miembro,
            Evento = "Detección persona"
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerDispositivoHogarPorId(dispositivoHogar.Id.ToString()))
            .Returns(dispositivoHogar);

        _logicaHogarMock
            .Setup(h => h.ObtenerMiembrosHogarConNotificaciones(dispositivoHogar.HogarId))
            .Returns([miembro]);

        _logicaNotificacionMock
            .Setup(i => i.Agregar(It.Is<Notificacion>(n =>
                n.Id != Guid.Empty &&
                n.DispositivoHogar == notificacionEsperada.DispositivoHogar &&
                n.Miembro == notificacionEsperada.Miembro &&
                n.Evento == notificacionEsperada.Evento)));

        _logicaNotificacionMock
            .Setup(n => n.GuardarCambios());

        _logicaHogarMock
            .Setup(h => h.ActualizarNotificacionesDeMiembros(dispositivoHogar.HogarId, It.IsAny<List<Notificacion>>()));

        var resultado = _servicio.GenerarNotificaciones("Detección persona", dispositivoHogar.Id.ToString());

        resultado.Should().NotBeNull();
        resultado[0].DispositivoHogar.Should().Be(dispositivoHogar);
        resultado[0].Miembro.Should().Be(miembro);
        resultado[0].Evento.Should().Be("Detección persona");
    }

    [TestMethod]
    public void GenerarNotificacionLamparaEncendidaCreaNotificacionCorrectamente()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), EstaConectado = true, HogarId = Guid.NewGuid() };
        var miembro = new MiembroHogar();

        var notificacionEsperada = new Notificacion
        {
            DispositivoHogar = dispositivoHogar,
            Miembro = miembro,
            Evento = "Encendida"
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerDispositivoHogarPorId(dispositivoHogar.Id.ToString()))
            .Returns(dispositivoHogar);

        _logicaHogarMock
            .Setup(h => h.ObtenerMiembrosHogarConNotificaciones(dispositivoHogar.HogarId))
            .Returns([miembro]);

        _logicaNotificacionMock
            .Setup(i => i.Agregar(It.Is<Notificacion>(n =>
                n.Id != Guid.Empty &&
                n.DispositivoHogar == notificacionEsperada.DispositivoHogar &&
                n.Miembro == notificacionEsperada.Miembro &&
                n.Evento == notificacionEsperada.Evento)));

        _logicaNotificacionMock
            .Setup(n => n.GuardarCambios());

        _logicaHogarMock
            .Setup(h => h.ActualizarNotificacionesDeMiembros(dispositivoHogar.HogarId, It.IsAny<List<Notificacion>>()));

        var resultado = _servicio.GenerarNotificaciones("Encendida", dispositivoHogar.Id.ToString());

        resultado.Should().NotBeNull();
        resultado[0].DispositivoHogar.Should().Be(dispositivoHogar);
        resultado[0].Miembro.Should().Be(miembro);
        resultado[0].Evento.Should().Be("Encendida");
    }

    [TestMethod]
    public void GenerarNotificacionLamparaApagadaCreaNotificacionCorrectamente()
    {
        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), EstaConectado = true, HogarId = Guid.NewGuid() };
        var miembro = new MiembroHogar();

        var notificacionEsperada = new Notificacion
        {
            DispositivoHogar = dispositivoHogar,
            Miembro = miembro,
            Evento = "Apagada"
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerDispositivoHogarPorId(dispositivoHogar.Id.ToString()))
            .Returns(dispositivoHogar);

        _logicaHogarMock
            .Setup(h => h.ObtenerMiembrosHogarConNotificaciones(dispositivoHogar.HogarId))
            .Returns([miembro]);

        _logicaNotificacionMock
            .Setup(i => i.Agregar(It.Is<Notificacion>(n =>
                n.Id != Guid.Empty &&
                n.DispositivoHogar == notificacionEsperada.DispositivoHogar &&
                n.Miembro == notificacionEsperada.Miembro &&
                n.Evento == notificacionEsperada.Evento)));

        _logicaNotificacionMock
            .Setup(n => n.GuardarCambios());

        _logicaHogarMock
            .Setup(h => h.ActualizarNotificacionesDeMiembros(dispositivoHogar.HogarId, It.IsAny<List<Notificacion>>()));

        var resultado = _servicio.GenerarNotificaciones("Apagada", dispositivoHogar.Id.ToString());

        resultado.Should().NotBeNull();
        resultado[0].DispositivoHogar.Should().Be(dispositivoHogar);
        resultado[0].Miembro.Should().Be(miembro);
        resultado[0].Evento.Should().Be("Apagada");
    }

    [TestMethod]
    public void ObtenerNotificacionesPorUsuarioRetornaNotificacionesCorrectas()
    {
        var usuario = new Usuario { Id = Guid.NewGuid() };

        var notificacion1 = new Notificacion
        {
            Id = Guid.NewGuid(),
            Miembro = new MiembroHogar()
            {
                Miembro = usuario,
                MiembroId = usuario.Id
            }
        };

        var notificacion2 = new Notificacion
        {
            Id = Guid.NewGuid(),
            Miembro = new MiembroHogar()
        };

        var notificaciones = new List<Notificacion> { notificacion1, notificacion2 };

        _logicaNotificacionMock
            .Setup(repo => repo.ObtenerTodos(null))
            .Returns(notificaciones);

        var resultado = _servicio.ObtenerNotificacionesPorUsuario(usuario, null);

        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(1);
        resultado.Should().ContainSingle(n => n.Id == notificacion1.Id);
    }

    [TestMethod]
    public void ObtenerNotificacionesPorUsuarioSinNotificacionesRetornaListaVacia()
    {
        var usuario = new Usuario { Id = Guid.NewGuid() };

        var notificacion = new Notificacion
        {
            Id = Guid.NewGuid(),
            Miembro = new MiembroHogar()
        };

        var notificaciones = new List<Notificacion> { notificacion };

        _logicaNotificacionMock
            .Setup(repo => repo.ObtenerTodos(null))
            .Returns(notificaciones);

        var resultado = _servicio.ObtenerNotificacionesPorUsuario(usuario, It.IsAny<ParametroNotificacionFiltro>());

        resultado.Should().NotBeNull();
        resultado.Should().BeEmpty();
    }

    [TestMethod]
    public void MarcarComoLeidasDeberiaMarcarTodasLasNotificacionesComoLeidas()
    {
        var notificaciones = new List<Notificacion>
        {
            new Notificacion { Id = Guid.NewGuid(), FueLeida = false },
            new Notificacion { Id = Guid.NewGuid(), FueLeida = false },
            new Notificacion { Id = Guid.NewGuid(), FueLeida = false }
        };

        _logicaNotificacionMock
            .Setup(repo => repo.Actualizar(It.IsAny<Notificacion>()));

        _servicio.MarcarComoLeidas(notificaciones);

        notificaciones[0].FueLeida.Should().BeTrue();
        notificaciones[1].FueLeida.Should().BeTrue();
        notificaciones[2].FueLeida.Should().BeTrue();
    }
    #endregion
}
