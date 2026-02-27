using FluentAssertions;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Notificaciones.Entidades;

namespace SmartHome.Persistencia.Test;

[TestClass]
public class NotificacionRepositorioTest
{
    private readonly ContextoSql _contexto;
    private readonly NotificacionRepositorio _repositorio;

    public NotificacionRepositorioTest()
    {
        _contexto = ContextoSqlTests.CrearContextoMemoria();
        _repositorio = new NotificacionRepositorio(_contexto);
    }

    [TestInitialize]
    public void Setup()
    {
        _contexto.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _contexto.Database.EnsureDeleted();
    }

    #region Agregar
    [TestMethod]
    public void CuandoSeProporcionaInfoDeberiaAgregarseALaBaseDeDatos()
    {
        var notificacion = new Notificacion()
        {
            Id = Guid.NewGuid(),
            DispositivoHogar = new DispositivoHogar
            {
                Nombre = "dispositivo",
            },
            Miembro = new MiembroHogar(),
            Evento = "evento",
            FueLeida = false,
            FechaHora = DateTime.Now
        };

        _repositorio.Agregar(notificacion);
        _repositorio.GuardarCambios();

        var notificaciones = _contexto.Notificaciones.ToList();
        var notificacionGuardada = notificaciones[0];

        notificacionGuardada.Id.Should().Be(notificacion.Id);
    }
    #endregion

    #region ObtenerTodos
    [TestMethod]
    public void ObtenerTodosDeberiaRetornarTodasLasNotificaciones()
    {
        var miembro1 = new MiembroHogar { MiembroId = Guid.NewGuid() };
        var miembro2 = new MiembroHogar { MiembroId = Guid.NewGuid() };

        var dispositivoHogar = new DispositivoHogar { Id = Guid.NewGuid(), Nombre = "mi dispositivo" };

        var notificacion1 = new Notificacion()
        {
            Id = Guid.NewGuid(),
            DispositivoHogar = dispositivoHogar,
            Miembro = miembro1,
            Evento = "evento 1",
            FueLeida = false,
            FechaHora = DateTime.Now
        };

        var notificacion2 = new Notificacion()
        {
            Id = Guid.NewGuid(),
            DispositivoHogar = dispositivoHogar,
            Miembro = miembro2,
            Evento = "evento 2",
            FueLeida = true,
            FechaHora = DateTime.Now
        };

        _contexto.Notificaciones.AddRange(notificacion1, notificacion2);
        _contexto.SaveChanges();

        var resultado = _repositorio.ObtenerTodos(null);

        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(2);
        resultado[0].DispositivoHogar.Id.Should().Be(dispositivoHogar.Id);
        resultado[1].DispositivoHogar.Id.Should().Be(dispositivoHogar.Id);
    }

    [TestMethod]
    public void ObtenerTodosDeberiaRetornarTodasLasNotificacionesDeUnTipoDispositivo()
    {
        var miembro1 = new MiembroHogar { MiembroId = Guid.NewGuid() };
        var miembro2 = new MiembroHogar { MiembroId = Guid.NewGuid() };

        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };
        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia
        };
        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            NombreCreador = "pepe",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var dispositivo1 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Tipo = TipoDispositivo.SensorVentana,
            Nombre = "c410",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };
        var dispositivo2 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Tipo = TipoDispositivo.Camara,
            Nombre = "c410",
            Modelo = "AQWSDS",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };
        var dispositivoHogar1 = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            Nombre = dispositivo1.Nombre,
            Dispositivo = dispositivo1
        };
        var dispositivoHogar2 = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            Nombre = "camara comedor",
            Dispositivo = dispositivo2
        };
        var notificacion1 = new Notificacion()
        {
            Id = Guid.NewGuid(),
            DispositivoHogar = dispositivoHogar1,
            Miembro = miembro1,
            Evento = "evento 1",
            FueLeida = false,
            FechaHora = DateTime.Now
        };
        var notificacion2 = new Notificacion()
        {
            Id = Guid.NewGuid(),
            DispositivoHogar = dispositivoHogar2,
            Miembro = miembro2,
            Evento = "evento 2",
            FueLeida = true,
            FechaHora = DateTime.Now
        };

        _contexto.Notificaciones.AddRange(notificacion1, notificacion2);
        _contexto.SaveChanges();

        var filtro = new ParametroNotificacionFiltro("SensorVentana", default, null);
        var resultado = _repositorio.ObtenerTodos(filtro);

        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(1);
        resultado[0].DispositivoHogar.Id.Should().Be(dispositivoHogar1.Id);
    }

    [TestMethod]
    public void ObtenerTodosDeberiaRetornarTodasLasNotificacionesDeUnaFecha()
    {
        var miembro1 = new MiembroHogar { MiembroId = Guid.NewGuid() };
        var miembro2 = new MiembroHogar { MiembroId = Guid.NewGuid() };
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };
        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia
        };
        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            NombreCreador = "pepe",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var dispositivo1 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Tipo = TipoDispositivo.SensorVentana,
            Nombre = "c410",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };
        var dispositivo2 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Tipo = TipoDispositivo.Camara,
            Nombre = "c410",
            Modelo = "AQWSDS",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };
        var dispositivoHogar1 = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            Nombre = dispositivo1.Nombre,
            Dispositivo = dispositivo1
        };
        var dispositivoHogar2 = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            Nombre = dispositivo2.Nombre,
            Dispositivo = dispositivo2
        };
        var notificacion1 = new Notificacion()
        {
            Id = Guid.NewGuid(),
            DispositivoHogar = dispositivoHogar1,
            Miembro = miembro1,
            Evento = "evento 1",
            FueLeida = false,
            FechaHora = DateTime.Today
        };
        var notificacion2 = new Notificacion()
        {
            Id = Guid.NewGuid(),
            DispositivoHogar = dispositivoHogar2,
            Miembro = miembro2,
            Evento = "evento 2",
            FueLeida = true
        };

        _contexto.Notificaciones.AddRange(notificacion1, notificacion2);
        _contexto.SaveChanges();

        var filtro = new ParametroNotificacionFiltro(null, DateTime.Today, null);
        var resultado = _repositorio.ObtenerTodos(filtro);

        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(1);
        resultado[0].DispositivoHogar.Id.Should().Be(dispositivoHogar1.Id);
    }

    [TestMethod]
    public void ObtenerTodosDeberiaRetornarTodasLasNotificacionesLeidas()
    {
        var miembro1 = new MiembroHogar { MiembroId = Guid.NewGuid() };
        var miembro2 = new MiembroHogar { MiembroId = Guid.NewGuid() };
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };
        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia
        };
        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            NombreCreador = "pepe",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var dispositivo1 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Tipo = TipoDispositivo.SensorVentana,
            Nombre = "c410",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };
        var dispositivo2 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Tipo = TipoDispositivo.Camara,
            Nombre = "c410",
            Modelo = "AQWSDS",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };
        var dispositivoHogar1 = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            Nombre = dispositivo1.Nombre,
            Dispositivo = dispositivo1
        };
        var dispositivoHogar2 = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            Nombre = dispositivo2.Nombre,
            Dispositivo = dispositivo2
        };
        var notificacion1 = new Notificacion()
        {
            Id = Guid.NewGuid(),
            DispositivoHogar = dispositivoHogar1,
            Miembro = miembro1,
            Evento = "evento 1",
            FueLeida = false,
            FechaHora = DateTime.Now
        };
        var notificacion2 = new Notificacion()
        {
            Id = Guid.NewGuid(),
            DispositivoHogar = dispositivoHogar2,
            Miembro = miembro2,
            Evento = "evento 2",
            FueLeida = true,
            FechaHora = DateTime.Now
        };

        _contexto.Notificaciones.AddRange(notificacion1, notificacion2);
        _contexto.SaveChanges();

        var filtro = new ParametroNotificacionFiltro(null, default, "True");
        var resultado = _repositorio.ObtenerTodos(filtro);

        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(1);
        resultado[0].DispositivoHogar.Id.Should().Be(dispositivoHogar2.Id);
    }
    #endregion

    #region Actualizar
    [TestMethod]
    public void ActualizarDeberiaModificarUnaNotificacionExistente()
    {
        var notificacion = new Notificacion
        {
            Id = Guid.NewGuid(),
            DispositivoHogar = new DispositivoHogar
            {
                Nombre = "Dispositivo",
            },
            Miembro = new MiembroHogar { MiembroId = Guid.NewGuid() },
            Evento = "Evento",
            FueLeida = false,
            FechaHora = DateTime.Now
        };

        _contexto.Notificaciones.Add(notificacion);
        _contexto.SaveChanges();

        notificacion.FueLeida = true;
        _repositorio.Actualizar(notificacion);
        _repositorio.GuardarCambios();

        var notificacionActualizada = _contexto.Notificaciones.First(n => n.Id == notificacion.Id);
        notificacionActualizada.FueLeida.Should().BeTrue();
    }
    #endregion
}
