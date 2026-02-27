using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Notificaciones.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public sealed class HogarTest
{
    private Mock<IHogarRepositorio> _logicaHogarMock = null!;
    private HogarLogica _servicio = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaHogarMock = new Mock<IHogarRepositorio>(MockBehavior.Strict);
        _servicio = new HogarLogica(_logicaHogarMock.Object);
    }

    #region Create
    #region Error
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearHogarConCalleNullOVacioLanzaExcepcion(string calle)
    {
        new CrearHogaresArgs(
            calle,
            1234,
            -60,
            15,
            5,
            "hogar",
            new Usuario());
    }

    [TestMethod]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearHogarConDueñoNullLanzaExcepcion(Usuario usuario)
    {
        new CrearHogaresArgs(
            "av italia",
            1234,
            -60,
            15,
            5,
            "hogar",
            usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearHogarConLatitudMenorAMenos90LanzaExcepcion()
    {
        new CrearHogaresArgs(
            "av italia",
            1234,
            -91,
            15,
            0,
            "hogar",
            new Usuario());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearHogarConLatitudMayorA90LanzaExcepcion()
    {
        new CrearHogaresArgs(
            "av italia",
            1234,
            91,
            15,
            0,
            "hogar",
            new Usuario());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearHogarConLongitudMenorAMenos180LanzaExcepcion()
    {
        new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            -181,
            0,
            "hogar",
            new Usuario());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearHogarConLongitudMayorA180LanzaExcepcion()
    {
        new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            181,
            0,
            "hogar",
            new Usuario());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearHogarConMiembrosSoportadosMenorA1LanzaExcepcion()
    {
        new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            15,
            0,
            "hogar",
            new Usuario());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearHogarConNumPuertaMenorA0LanzaExcepcion()
    {
        new CrearHogaresArgs(
            "av italia",
            -3,
            60,
            15,
            0,
            "hogar",
            new Usuario());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void VerificarPermisosLanzaExepcionConPermisoInexistente()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pepe@gmail.com"
        };

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            MiembroId = usuario.Id,
            HogarId = hogarId,
            Hogar = hogar
        };

        hogar.Miembros.Add(miembro);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        _servicio.VerificarPermiso("ComprarHogar", usuario, hogarId.ToString());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ListarMiembrosExepcionSinPermiso()
    {
        var dueño = new Usuario
        {
            Nombre = "juan",
            Apellido = "Gomez",
            Email = "juanGomez@gmail.com",
            Contraseña = "juan1234.",
            FotoPerfil = "/downloads/juanGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var args = new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            15,
            3,
            "hogar",
            new Usuario());

        _logicaHogarMock
            .Setup(i => i.Agregar(It.Is<Hogar>(
                h => h.Calle == args.Calle &&
                     h.NumPuerta == args.NumPuerta &&
                     h.Latitud == args.Latitud &&
                     h.Longitud == args.Longitud &&
                     h.CantMiembrosSoportados == args.CantMiembrosSoportados)));

        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = args.Calle,
            NumPuerta = args.NumPuerta,
            Latitud = args.Latitud,
            Longitud = args.Longitud,
            CantMiembrosSoportados = args.CantMiembrosSoportados,
            Miembros = [],
            Cuartos = [],
            DueñoId = dueño.Id
        };

        _logicaHogarMock
            .Setup(h => h.ObtenerTodos())
            .Returns([hogar]);

        _servicio.ObtenerMiembrosDeHogar(hogar.Id.ToString(), usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ActualizarAliasExepcionSinPermiso()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Alias = "Hogar de Pepe",
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = 15,
            CantMiembrosSoportados = 3,
            DueñoId = Guid.NewGuid(),
            Miembros = []
        };

        var nuevoAlias = "nuevo alias";

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        _logicaHogarMock
            .Setup(r => r.Actualizar(hogar));

        _servicio.ActualizarAlias(hogar.Id.ToString(), nuevoAlias, usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ListarCuartosExepcionSinPermiso()
    {
        var usuario = new Usuario
        {
            Nombre = "juan",
            Apellido = "Gomez",
            Email = "juanGomez@gmail.com",
            Contraseña = "juan1234.",
            FotoPerfil = "/downloads/juanGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var args = new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            15,
            3,
            "hogar",
            new Usuario());

        _logicaHogarMock
            .Setup(i => i.Agregar(It.Is<Hogar>(
                h => h.Calle == args.Calle &&
                     h.NumPuerta == args.NumPuerta &&
                     h.Latitud == args.Latitud &&
                     h.Longitud == args.Longitud &&
                     h.CantMiembrosSoportados == args.CantMiembrosSoportados)));

        var cuarto = new Cuarto
        {
            Id = Guid.NewGuid(),
            Nombre = "Mi cuarto",
            Hogar = new Hogar
            {
                Id = Guid.NewGuid(),
                Calle = "av italia",
                NumPuerta = 123,
                Latitud = 10,
                Longitud = 0,
                CantMiembrosSoportados = 5,
                DueñoId = Guid.NewGuid(),
                Miembros = []
            }
        };

        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = args.Calle,
            NumPuerta = args.NumPuerta,
            Latitud = args.Latitud,
            Longitud = args.Longitud,
            CantMiembrosSoportados = args.CantMiembrosSoportados,
            Miembros = [],
            Cuartos = [cuarto],
            DueñoId = Guid.NewGuid()
        };

        _logicaHogarMock
            .Setup(h => h.ObtenerTodos())
            .Returns([hogar]);

        _servicio.ObtenerCuartosDeHogar(hogar.Id.ToString(), usuario);
    }
    #endregion

    #region Exito
    [TestMethod]
    public void CrearHogarExito()
    {
        var args = new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            15,
            3,
            "hogar",
            new Usuario());

        _logicaHogarMock
            .Setup(i => i.Agregar(It.Is<Hogar>(h =>
                h.Id != Guid.Empty &&
                h.Calle == args.Calle &&
                h.NumPuerta == args.NumPuerta &&
                h.Latitud == args.Latitud &&
                h.Longitud == args.Longitud &&
                h.CantMiembrosSoportados == args.CantMiembrosSoportados)));

        _logicaHogarMock
            .Setup(i => i.GuardarCambios());

        var respuesta = _servicio.Agregar(args);
        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBeEmpty();
        respuesta.Id.Should().NotBe(Guid.Empty);

        respuesta.Calle.Should().Be(args.Calle);
        respuesta.NumPuerta.Should().Be(args.NumPuerta);
        respuesta.Latitud.Should().Be(args.Latitud);
        respuesta.Longitud.Should().Be(args.Longitud);
        respuesta.CantMiembrosSoportados.Should().Be(args.CantMiembrosSoportados);
    }

    [TestMethod]
    public void ObtenerTodosDevuelveListaDeHogares()
    {
        var hogares = new List<Hogar>
        {
            new Hogar
            {
                Id = Guid.NewGuid(),
                Calle = "av italia",
                NumPuerta = 1234
            },
            new Hogar
            {
                Id = Guid.NewGuid(),
                Calle = "av rivera",
                NumPuerta = 5678
            }
        };

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns(hogares);

        var resultado = _servicio.ObtenerTodos();

        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(2);
        resultado.Should().Contain(h => h.Calle == "av italia" && h.NumPuerta == 1234);
        resultado.Should().Contain(h => h.Calle == "av rivera" && h.NumPuerta == 5678);
    }

    [TestMethod]
    public void ObtenerPorIdDevuelveHogarCorrecto()
    {
        var hogarId = Guid.NewGuid();
        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            NumPuerta = 1234
        };

        var hogares = new List<Hogar>
        {
            hogar
        };

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns(hogares);

        var resultado = _servicio.ObtenerPorId(hogarId.ToString());

        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(hogarId);
        resultado.Calle.Should().Be("av italia");
        resultado.NumPuerta.Should().Be(1234);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void ObtenerPorIdLanzaExcepcionSiHogarNoExiste()
    {
        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([]);

        _servicio.ObtenerPorId(Guid.NewGuid().ToString());
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ObtenerPorIdLanzaExcepcionSiIdNoEsGuid()
    {
        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([]);

        _servicio.ObtenerPorId("no guid");
    }

    [TestMethod]
    public void EsDueñoHogarDevuelveTrueSiUsuarioEsDueño()
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };
        var hogarId = Guid.NewGuid();
        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = usuario.Id
        };

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.EsDueñoHogar(usuario, hogarId.ToString());

        resultado.Should().BeTrue();
    }

    [TestMethod]
    public void EsDueñoHogarDevuelveFalseSiUsuarioNoEsDueño()
    {
        var usuario = new Usuario
        {
            Email = "noDueno@gmail.com"
        };

        var hogarId = Guid.NewGuid();
        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id
        };

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.EsDueñoHogar(usuario, hogarId.ToString());

        resultado.Should().BeFalse();
    }

    [TestMethod]
    public void EsDueñoHogarDevuelveFalseSiIdNoEsValido()
    {
        var usuario = new Usuario
        {
            Email = "dueno@gmail.com"
        };

        var resultado = _servicio.EsDueñoHogar(usuario, "idInvalido");

        resultado.Should().BeFalse();
    }

    [TestMethod]
    public void ExisteHogarExito()
    {
        var idHogar = Guid.NewGuid();

        _logicaHogarMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Hogar, bool>>>(e => e.Compile()(new Hogar()
            {
                Id = idHogar
            }))))
            .Returns(true);

        var hogarExiste = _servicio.Existe(h => h.Id == idHogar);

        hogarExiste.Should().BeTrue();
    }
    #endregion

    #region Permisos

    [TestMethod]
    public void TienePermisoAsociarDispositivosDevuelveTrueSiTienePermiso()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pepe@gmail.com"
        };

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            MiembroId = usuario.Id,
            HogarId = hogarId,
            Hogar = hogar,
            PermisoAsociarDispositivos = true
        };

        hogar.Miembros.Add(miembro);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("AsociarDispositivo", usuario, hogarId.ToString());

        resultado.Should().BeTrue();
    }

    [TestMethod]
    public void TienePermisoAsociarDevuelveFalseSiUsuarioNoTienePermiso()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pepe@gmail.com"
        };

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            MiembroId = usuario.Id,
            HogarId = hogarId,
            Hogar = hogar
        };

        hogar.Miembros.Add(miembro);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("AsociarDispositivo", usuario, hogarId.ToString());

        resultado.Should().BeFalse();
    }

    [TestMethod]
    public void TienePermisoAsociarDevuelveFalseSiIdNoEsValido()
    {
        var usuario = new Usuario
        {
            Email = "pepe@gmail.com"
        };

        var resultado = _servicio.TienePermisoAsociarDispositivo(usuario, "idInvalido");

        resultado.Should().BeFalse();
    }

    [TestMethod]
    public void TienePermisoListarDispositivosDevuelveTrueSiTienePermiso()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pepe@gmail.com"
        };

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            MiembroId = usuario.Id,
            HogarId = hogarId,
            Hogar = hogar,
            PermisoListarDispositivos = true
        };

        hogar.Miembros.Add(miembro);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("ListarDispositivo", usuario, hogarId.ToString());

        resultado.Should().BeTrue();
    }

    [TestMethod]
    public void TienePermisoListarDevuelveFalseSiUsuarioNoTienePermiso()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pepe@gmail.com"
        };

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            HogarId = hogarId,
            Hogar = hogar
        };

        hogar.Miembros.Add(miembro);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("ListarDispositivo", usuario, hogarId.ToString());

        resultado.Should().BeFalse();
    }

    [TestMethod]
    public void TienePermisoListarDevuelveFalseSiIdNoEsValido()
    {
        var usuario = new Usuario
        {
            Email = "pepe@gmail.com"
        };

        var resultado = _servicio.TienePermisoListarDispositivos(usuario, "idInvalido");

        resultado.Should().BeFalse();
    }

    [TestMethod]
    public void TienePermisoCuartosDevuelveTrueSiTienePermiso()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pepe@gmail.com"
        };

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            MiembroId = usuario.Id,
            HogarId = hogarId,
            Hogar = hogar,
            PermisoAdministrarCuartos = true
        };

        hogar.Miembros.Add(miembro);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("AdministrarCuarto", usuario, hogarId.ToString());

        resultado.Should().BeTrue();
    }

    [TestMethod]
    public void TienePermisoCuartosDevuelveFalseSiUsuarioNoTienePermiso()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pepe@gmail.com"
        };

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            HogarId = hogarId,
            Hogar = hogar
        };

        hogar.Miembros.Add(miembro);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("AdministrarCuarto", usuario, hogarId.ToString());

        resultado.Should().BeFalse();
    }

    [TestMethod]
    public void TienePermisoCuartosDevuelveFalseSiIdNoEsValido()
    {
        var usuario = new Usuario
        {
            Email = "pepe@gmail.com"
        };

        var resultado = _servicio.TienePermisoAdministrarCuartos(usuario, "idInvalido");

        resultado.Should().BeFalse();
    }

    [TestMethod]
    public void ObtenerMiembrosHogarConNotificacionesDevuelveMiembrosConPermiso()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "miembro1@gmail.com"
        };

        var miembro1 = new MiembroHogar
        {
            Miembro = usuario1,
            MiembroId = usuario1.Id,
            HogarId = hogarId,
            Hogar = hogar,
            PermisoNotificaciones = true
        };

        var usuario2 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "miembro2@gmail.com"
        };

        var miembro2 = new MiembroHogar
        {
            Miembro = usuario2,
            MiembroId = usuario2.Id,
            HogarId = hogarId,
            Hogar = hogar,
            PermisoNotificaciones = false
        };

        hogar.Miembros.Add(miembro1);
        hogar.Miembros.Add(miembro2);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.ObtenerMiembrosHogarConNotificaciones(hogarId);

        resultado.Should().HaveCount(1);
        resultado.Should().Contain(miembro1);
        resultado.Should().NotContain(miembro2);
    }

    [TestMethod]
    public void AgregarNotificacionesAgregaNotificacionesAMiembrosConPermisos()
    {
        var miembro1 = new MiembroHogar { MiembroId = Guid.NewGuid(), Notificaciones = [], PermisoNotificaciones = true };
        var miembro2 = new MiembroHogar { MiembroId = Guid.NewGuid(), Notificaciones = [], PermisoNotificaciones = false };

        var notificacion1 = new Notificacion { Evento = "Alerta de sensor abierto", Miembro = miembro1 };
        var notificacion2 = new Notificacion { Evento = "Alerta de sensor abierto", Miembro = miembro1 };

        var notificaciones = new List<Notificacion>()
        {
            notificacion1,
            notificacion2
        };

        var hogarId = Guid.NewGuid();

        var hogar = new Hogar
        {
            Id = hogarId,
            Miembros = [miembro1, miembro2]
        };

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        _logicaHogarMock
            .Setup(r => r.GuardarCambios());

        _logicaHogarMock
            .Setup(r => r.ActualizarMiembro(It.IsAny<MiembroHogar>()));

        _servicio.ActualizarNotificacionesDeMiembros(hogarId, notificaciones);
        _servicio.GuardarCambios();

        miembro1.Notificaciones.Count.Should().Be(2);
        miembro2.Notificaciones.Count.Should().Be(0);
    }

    [TestMethod]
    public void ActualizarAliasLoModificaCorrectamente()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Alias = "Hogar de Pepe",
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = 15,
            CantMiembrosSoportados = 3,
            DueñoId = usuario.Id,
            Miembros = []
        };

        var nuevoAlias = "nuevo alias";

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        _logicaHogarMock
            .Setup(r => r.Actualizar(hogar));

        _servicio.ActualizarAlias(hogar.Id.ToString(), nuevoAlias, usuario);

        hogar.Alias.Should().Be(nuevoAlias);
    }

    [TestMethod]
    public void TienePermisoModificarNombreDispositivosDevuelveTrueSiTienePermiso()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pepe@gmail.com"
        };

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            MiembroId = usuario.Id,
            HogarId = hogarId,
            Hogar = hogar,
            PermisoModificarNombreDispositivos = true
        };

        hogar.Miembros.Add(miembro);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("ModificarNombreDispositivo", usuario, hogarId.ToString());

        resultado.Should().BeTrue();
    }

    [TestMethod]
    public void TienePermisoModificarNombreDispositivosDevuelveFalseSiNoTienePermiso()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pepe@gmail.com"
        };

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            MiembroId = usuario.Id,
            HogarId = hogarId,
            Hogar = hogar,
        };

        hogar.Miembros.Add(miembro);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("ModificarNombreDispositivo", usuario, hogarId.ToString());

        resultado.Should().BeFalse();
    }

    [TestMethod]
    public void TienePermisoAgregarMiembroDevuelveTrueSiTienePermiso()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("AgregarMiembro", dueño, hogarId.ToString());

        resultado.Should().BeTrue();
    }

    [TestMethod]
    public void TienePermisoAgregarMiembroDevuelveFalseSiUsuarioNoEsDueño()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pepe@gmail.com"
        };

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            MiembroId = usuario.Id,
            HogarId = hogarId,
            Hogar = hogar
        };

        hogar.Miembros.Add(miembro);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("AgregarMiembro", usuario, hogarId.ToString());

        resultado.Should().BeFalse();
    }

    [TestMethod]
    public void TienePermisoListarMiembroDevuelveTrueSiTienePermiso()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("ListarMiembro", dueño, hogarId.ToString());

        resultado.Should().BeTrue();
    }

    [TestMethod]
    public void TienePermisoListarMiembroDevuelveFalseSiUsuarioNoEsDueño()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pepe@gmail.com"
        };

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            MiembroId = usuario.Id,
            HogarId = hogarId,
            Hogar = hogar
        };

        hogar.Miembros.Add(miembro);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("ListarMiembro", usuario, hogarId.ToString());

        resultado.Should().BeFalse();
    }

    [TestMethod]
    public void TienePermisoModificarAliasDevuelveTrueSiTienePermiso()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("ModificarAlias", dueño, hogarId.ToString());

        resultado.Should().BeTrue();
    }

    [TestMethod]
    public void TienePermisoModificarAliasDevuelveFalseSiUsuarioNoEsDueño()
    {
        var hogarId = Guid.NewGuid();

        var dueño = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "dueno@gmail.com"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            DueñoId = dueño.Id,
            Miembros = []
        };

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pepe@gmail.com"
        };

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            MiembroId = usuario.Id,
            HogarId = hogarId,
            Hogar = hogar
        };

        hogar.Miembros.Add(miembro);

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns([hogar]);

        var resultado = _servicio.VerificarPermiso("ModificarAlias", usuario, hogarId.ToString());

        resultado.Should().BeFalse();
    }

    #endregion

    #region AgregarMiembro
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarMiembrosSinSerDueñoLanzaExcepcion()
    {
        var usuario = new Usuario
        {
            Nombre = "juan",
            Apellido = "Gomez",
            Email = "juanGomez@gmail.com",
            Contraseña = "juan1234.",
            FotoPerfil = "/downloads/juanGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var usuario1 = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var miembro1 = new MiembroHogar
        {
            Miembro = usuario1,
            MiembroId = usuario1.Id,
            PermisoNotificaciones = true,
            PermisoListarDispositivos = true,
            PermisoAsociarDispositivos = true
        };

        var usuario2 = new Usuario
        {
            Nombre = "Alen",
            Apellido = "Perez",
            Email = "alan@gmail.com",
            Contraseña = "alan1234.",
            FotoPerfil = "/downloads/foto",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var miembro2 = new MiembroHogar
        {
            Miembro = usuario2,
            MiembroId = usuario2.Id,
            PermisoNotificaciones = true,
            PermisoListarDispositivos = true,
            PermisoAsociarDispositivos = true
        };

        var args = new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            15,
            3,
            "hogar",
            new Usuario());

        _logicaHogarMock
            .Setup(i => i.Agregar(It.Is<Hogar>(
                h => h.Calle == args.Calle &&
                h.NumPuerta == args.NumPuerta &&
                h.Latitud == args.Latitud &&
                h.Longitud == args.Longitud &&
                h.CantMiembrosSoportados == args.CantMiembrosSoportados)));

        _logicaHogarMock
            .Setup(i => i.AgregarMiembro(It.IsAny<MiembroHogar>()));

        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = args.Calle,
            NumPuerta = args.NumPuerta,
            Latitud = args.Latitud,
            Longitud = args.Longitud,
            CantMiembrosSoportados = args.CantMiembrosSoportados,
            Miembros = [],
            Cuartos = [],
            DueñoId = args.DueñoId
        };

        _logicaHogarMock
            .Setup(h => h.ObtenerTodos())
            .Returns([hogar]);

        _servicio.AgregarMiembro(hogar.Id.ToString(), miembro1, usuario);

        hogar.Miembros.Should().HaveCount(2);
        hogar.Miembros.Should().Contain(miembro1);
        hogar.Miembros.Should().Contain(miembro2);
    }

    [TestMethod]
    public void AgregarMiembrosAlHogar()
    {
        var dueño = new Usuario
        {
            Nombre = "juan",
            Apellido = "Gomez",
            Email = "juanGomez@gmail.com",
            Contraseña = "juan1234.",
            FotoPerfil = "/downloads/juanGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var usuario1 = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var miembro1 = new MiembroHogar
        {
            Miembro = usuario1,
            MiembroId = usuario1.Id,
            PermisoNotificaciones = true,
            PermisoListarDispositivos = true,
            PermisoAsociarDispositivos = true
        };

        var usuario2 = new Usuario
        {
            Nombre = "Alen",
            Apellido = "Perez",
            Email = "alan@gmail.com",
            Contraseña = "alan1234.",
            FotoPerfil = "/downloads/foto",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var miembro2 = new MiembroHogar
        {
            Miembro = usuario2,
            MiembroId = usuario2.Id,
            PermisoNotificaciones = true,
            PermisoListarDispositivos = true,
            PermisoAsociarDispositivos = true
        };

        var args = new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            15,
            3,
            "hogar",
            dueño);

        _logicaHogarMock
            .Setup(i => i.Agregar(It.Is<Hogar>(
                h => h.Calle == args.Calle &&
                h.NumPuerta == args.NumPuerta &&
                h.Latitud == args.Latitud &&
                h.Longitud == args.Longitud &&
                h.CantMiembrosSoportados == args.CantMiembrosSoportados)));

        _logicaHogarMock
            .Setup(i => i.AgregarMiembro(It.IsAny<MiembroHogar>()));

        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = args.Calle,
            NumPuerta = args.NumPuerta,
            Latitud = args.Latitud,
            Longitud = args.Longitud,
            CantMiembrosSoportados = args.CantMiembrosSoportados,
            Miembros = [],
            Cuartos = [],
            DueñoId = args.DueñoId
        };

        _logicaHogarMock
            .Setup(h => h.ObtenerTodos())
            .Returns([hogar]);

        _servicio.AgregarMiembro(hogar.Id.ToString(), miembro1, dueño);
        _servicio.AgregarMiembro(hogar.Id.ToString(), miembro2, dueño);

        hogar.Miembros.Should().HaveCount(2);
        hogar.Miembros.Should().Contain(miembro1);
        hogar.Miembros.Should().Contain(miembro2);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarMiembroAHogarSinEspacioLanzaExcepcion()
    {
        var dueño = new Usuario
        {
            Nombre = "juan",
            Apellido = "Gomez",
            Email = "juanGomez@gmail.com",
            Contraseña = "juan1234.",
            FotoPerfil = "/downloads/juanGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var miembro1 = new MiembroHogar
        {
            Miembro = usuario1,
            MiembroId = usuario1.Id
        };

        var usuario2 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Alen",
            Apellido = "Perez",
            Email = "alan@gmail.com",
            Contraseña = "alan1234.",
            FotoPerfil = "/downloads/foto",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var miembro2 = new MiembroHogar
        {
            Miembro = usuario2,
            MiembroId = usuario2.Id
        };

        var args = new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            15,
            2,
            "hogar",
            new Usuario());

        _logicaHogarMock
            .Setup(i => i.Agregar(It.Is<Hogar>(
                h => h.Calle == args.Calle &&
                h.NumPuerta == args.NumPuerta &&
                h.Latitud == args.Latitud &&
                h.Longitud == args.Longitud &&
                h.CantMiembrosSoportados == args.CantMiembrosSoportados)));

        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = args.Calle,
            NumPuerta = args.NumPuerta,
            Latitud = args.Latitud,
            Longitud = args.Longitud,
            CantMiembrosSoportados = args.CantMiembrosSoportados,
            Miembros = [miembro1],
            Cuartos = []
        };

        _logicaHogarMock
            .Setup(h => h.ObtenerTodos())
            .Returns([hogar]);

        _servicio.AgregarMiembro(hogar.Id.ToString(), miembro1, dueño);
        _servicio.AgregarMiembro(hogar.Id.ToString(), miembro2, dueño);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarMiembroSinPermisoCrearHogarLanzaExcepcion()
    {
        var dueño = new Usuario
        {
            Nombre = "juan",
            Apellido = "Gomez",
            Email = "juanGomez@gmail.com",
            Contraseña = "juan1234.",
            FotoPerfil = "/downloads/juanGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = RolesPredefinidos.Admin
        };

        var miembro1 = new MiembroHogar
        {
            Miembro = usuario1,
            MiembroId = usuario1.Id
        };

        var args = new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            15,
            3,
            "hogar",
            new Usuario());

        _logicaHogarMock
            .Setup(i => i.Agregar(It.Is<Hogar>(
                h => h.Calle == args.Calle &&
                h.NumPuerta == args.NumPuerta &&
                h.Latitud == args.Latitud &&
                h.Longitud == args.Longitud &&
                h.CantMiembrosSoportados == args.CantMiembrosSoportados)));

        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = args.Calle,
            NumPuerta = args.NumPuerta,
            Latitud = args.Latitud,
            Longitud = args.Longitud,
            CantMiembrosSoportados = args.CantMiembrosSoportados,
            Miembros = [miembro1],
            Cuartos = []
        };

        _logicaHogarMock
            .Setup(h => h.ObtenerTodos())
            .Returns([hogar]);

        _servicio.AgregarMiembro(hogar.Id.ToString(), miembro1, dueño);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarMiembroConMismoIdQueDueñoLanzaExcepcion()
    {
        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = RolesPredefinidos.Admin
        };

        var miembro1 = new MiembroHogar
        {
            Miembro = usuario1,
            MiembroId = usuario1.Id
        };

        var args = new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            15,
            3,
            "hogar",
            new Usuario
            {
                Id = usuario1.Id
            });

        _logicaHogarMock
            .Setup(i => i.Agregar(It.Is<Hogar>(
                h => h.Calle == args.Calle &&
                h.NumPuerta == args.NumPuerta &&
                h.Latitud == args.Latitud &&
                h.Longitud == args.Longitud &&
                h.CantMiembrosSoportados == args.CantMiembrosSoportados)));

        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = args.Calle,
            NumPuerta = args.NumPuerta,
            Latitud = args.Latitud,
            Longitud = args.Longitud,
            CantMiembrosSoportados = args.CantMiembrosSoportados,
            Miembros = [],
            Cuartos = [],
            DueñoId = usuario1.Id
        };

        _logicaHogarMock
            .Setup(h => h.ObtenerTodos())
            .Returns([hogar]);

        _servicio.AgregarMiembro(hogar.Id.ToString(), miembro1, usuario1);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarMiembroConEmailDuplicadoLanzaExcepcion()
    {
        var dueño = new Usuario
        {
            Nombre = "juan",
            Apellido = "Gomez",
            Email = "juanGomez@gmail.com",
            Contraseña = "juan1234.",
            FotoPerfil = "/downloads/juanGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var miembro1 = new MiembroHogar
        {
            Miembro = usuario1,
            MiembroId = usuario1.Id
        };

        var args = new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            15,
            3,
            "hogar",
            new Usuario());

        _logicaHogarMock
            .Setup(i => i.Agregar(It.Is<Hogar>(
                h => h.Calle == args.Calle &&
                h.NumPuerta == args.NumPuerta &&
                h.Latitud == args.Latitud &&
                h.Longitud == args.Longitud &&
                h.CantMiembrosSoportados == args.CantMiembrosSoportados)));

        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = args.Calle,
            NumPuerta = args.NumPuerta,
            Latitud = args.Latitud,
            Longitud = args.Longitud,
            CantMiembrosSoportados = args.CantMiembrosSoportados,
            Miembros = [miembro1],
            Cuartos = []
        };

        _logicaHogarMock
            .Setup(h => h.ObtenerTodos())
            .Returns([hogar]);

        _servicio.AgregarMiembro(hogar.Id.ToString(), miembro1, dueño);
    }
    #endregion

    #region ListarHogaresUsuario
    [TestMethod]
    public void ListarHogaresUsuarioDevuelveHogaresCorrectos()
    {
        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var miembros = new MiembroHogar
        {
            Miembro = usuario
        };

        var hogar1 = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = "av italia",
            NumPuerta = 1234,
            DueñoId = usuario.Id,
            Miembros = []
        };

        var hogar2 = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = "av rivera",
            NumPuerta = 5678,
            DueñoId = usuario.Id,
            Miembros = []
        };

        var hogares = new List<Hogar>
        {
            hogar1,
            hogar2
        };

        _logicaHogarMock
            .Setup(r => r.ObtenerTodos())
            .Returns(hogares);

        var resultado = _servicio.ObtenerHogaresPorUsuario(usuario);

        resultado.Should().HaveCount(2);
        resultado.Should().Contain(h => h.Id == hogar1.Id);
        resultado.Should().Contain(h => h.Id == hogar2.Id);
    }

    [TestMethod]
    public void ListarMiembrosExito()
    {
        var dueño = new Usuario
        {
            Nombre = "juan",
            Apellido = "Gomez",
            Email = "juanGomez@gmail.com",
            Contraseña = "juan1234.",
            FotoPerfil = "/downloads/juanGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var miembro1 = new MiembroHogar
        {
            Miembro = usuario1,
            MiembroId = usuario1.Id
        };

        var args = new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            15,
            3,
            "hogar",
            new Usuario());

        _logicaHogarMock
            .Setup(i => i.Agregar(It.Is<Hogar>(
                h => h.Calle == args.Calle &&
                     h.NumPuerta == args.NumPuerta &&
                     h.Latitud == args.Latitud &&
                     h.Longitud == args.Longitud &&
                     h.CantMiembrosSoportados == args.CantMiembrosSoportados)));

        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = args.Calle,
            NumPuerta = args.NumPuerta,
            Latitud = args.Latitud,
            Longitud = args.Longitud,
            CantMiembrosSoportados = args.CantMiembrosSoportados,
            Miembros = [miembro1],
            Cuartos = [],
            DueñoId = dueño.Id
        };

        _logicaHogarMock
            .Setup(h => h.ObtenerTodos())
            .Returns([hogar]);

        var respuesta = _servicio.ObtenerMiembrosDeHogar(hogar.Id.ToString(), dueño);

        respuesta.Should().NotBeNull();
        respuesta.Count.Should().Be(1);
    }

    [TestMethod]
    public void ListarCuartosExito()
    {
        var dueño = new Usuario
        {
            Nombre = "juan",
            Apellido = "Gomez",
            Email = "juanGomez@gmail.com",
            Contraseña = "juan1234.",
            FotoPerfil = "/downloads/juanGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var args = new CrearHogaresArgs(
            "av italia",
            1234,
            60,
            15,
            3,
            "hogar",
            new Usuario());

        _logicaHogarMock
            .Setup(i => i.Agregar(It.Is<Hogar>(
                h => h.Calle == args.Calle &&
                     h.NumPuerta == args.NumPuerta &&
                     h.Latitud == args.Latitud &&
                     h.Longitud == args.Longitud &&
                     h.CantMiembrosSoportados == args.CantMiembrosSoportados)));

        var cuarto = new Cuarto
        {
            Id = Guid.NewGuid(),
            Nombre = "Mi cuarto",
            Hogar = new Hogar
            {
                Id = Guid.NewGuid(),
                Calle = "av italia",
                NumPuerta = 123,
                Latitud = 10,
                Longitud = 0,
                CantMiembrosSoportados = 5,
                DueñoId = Guid.NewGuid(),
                Miembros = []
            }
        };

        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = args.Calle,
            NumPuerta = args.NumPuerta,
            Latitud = args.Latitud,
            Longitud = args.Longitud,
            CantMiembrosSoportados = args.CantMiembrosSoportados,
            Miembros = [],
            Cuartos = [cuarto],
            DueñoId = dueño.Id
        };

        _logicaHogarMock
            .Setup(h => h.ObtenerTodos())
            .Returns([hogar]);

        var respuesta = _servicio.ObtenerCuartosDeHogar(hogar.Id.ToString(), dueño);

        respuesta.Should().NotBeNull();
        respuesta.Count.Should().Be(1);
    }
    #endregion
    #endregion
}
