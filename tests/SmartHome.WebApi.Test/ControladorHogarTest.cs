using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Cuartos;
using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.DispositivosHogar;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.Hogares;
using SmartHome.WebApi.Controllers.Hogares.Modelos;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ControladorHogarTest
{
    private CrearSolicitudHogar _solicitudCrearHogar = null!;
    private CrearSolicitudAgregarMiembro _solicitudAgregarMiembro = null!;
    private CrearSolicitudAsociarDispositivo _solicitudAsociarDispositivo = null!;
    private CrearSolicitudModificarHogar _solicitudModificarHogar = null!;
    private CrearSolicitudAgregarCuarto _solicitudAgregarCuarto = null!;
    private Mock<IHogarLogica> _logicaHogarMock = null!;
    private Mock<IUsuarioLogica> _logicaUsuarioMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;
    private Mock<IDispositivoLogica> _logicaDispositivoMock = null!;
    private Mock<IDispositivoHogarLogica> _logicaDispositivoHogarMock = null!;
    private Mock<IEmpresaLogica> _logicaEmpresaMock = null!;
    private Mock<ICuartoLogica> _logicaCuartoMock = null!;
    private ControladorHogar _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaHogarMock = new Mock<IHogarLogica>(MockBehavior.Default);
        _logicaUsuarioMock = new Mock<IUsuarioLogica>(MockBehavior.Default);
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);
        _logicaDispositivoMock = new Mock<IDispositivoLogica>(MockBehavior.Default);
        _logicaDispositivoHogarMock = new Mock<IDispositivoHogarLogica>(MockBehavior.Default);
        _logicaEmpresaMock = new Mock<IEmpresaLogica>(MockBehavior.Default);
        _logicaCuartoMock = new Mock<ICuartoLogica>(MockBehavior.Default);
        _controlador = new ControladorHogar(_logicaHogarMock.Object, _logicaUsuarioMock.Object, _logicaSesionMock.Object, _logicaDispositivoHogarMock.Object, _logicaDispositivoMock.Object, _logicaEmpresaMock.Object, _logicaCuartoMock.Object);
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
    public void CrearConCalleNullOVacioLanzaExcepcion(string calle)
    {
        _solicitudCrearHogar = new CrearSolicitudHogar
        {
            Calle = calle,
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = 15,
            CantMiembrosSoportados = 3
        };

        var auth = Guid.NewGuid().ToString();

        _controlador.Crear(_solicitudCrearHogar, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearConLatitudMenorAMenos90LanzaExcepcion()
    {
        _solicitudCrearHogar = new CrearSolicitudHogar
        {
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = -91,
            Longitud = 15,
            CantMiembrosSoportados = 3
        };

        var auth = Guid.NewGuid().ToString();

        _controlador.Crear(_solicitudCrearHogar, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearConLatitudMayorA90LanzaExcepcion()
    {
        _solicitudCrearHogar = new CrearSolicitudHogar
        {
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 91,
            Longitud = 15,
            CantMiembrosSoportados = 3
        };

        var auth = Guid.NewGuid().ToString();

        _controlador.Crear(_solicitudCrearHogar, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearConLongitudMenorAMenos180LanzaExcepcion()
    {
        _solicitudCrearHogar = new CrearSolicitudHogar
        {
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = -181,
            CantMiembrosSoportados = 3
        };

        var auth = Guid.NewGuid().ToString();

        _controlador.Crear(_solicitudCrearHogar, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearConLongitudMayorA180LanzaExcepcion()
    {
        _solicitudCrearHogar = new CrearSolicitudHogar
        {
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = 181,
            CantMiembrosSoportados = 3
        };

        var auth = Guid.NewGuid().ToString();

        _controlador.Crear(_solicitudCrearHogar, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearConMiembrosSoportadosMenorA1LanzaExcepcion()
    {
        _solicitudCrearHogar = new CrearSolicitudHogar
        {
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = -50,
            CantMiembrosSoportados = 0
        };

        var auth = Guid.NewGuid().ToString();

        _controlador.Crear(_solicitudCrearHogar, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearConNumPuertaMenorA0LanzaExcepcion()
    {
        _solicitudCrearHogar = new CrearSolicitudHogar
        {
            Calle = "av italia",
            NumPuerta = -1,
            Latitud = 60,
            Longitud = -50,
            CantMiembrosSoportados = 3
        };

        var auth = Guid.NewGuid().ToString();

        _controlador.Crear(_solicitudCrearHogar, auth);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ActualizarAliasConAliasNullLanzaExcepcion()
    {
        _solicitudModificarHogar = new CrearSolicitudModificarHogar
        {
        };

        var dueño = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var hogarId = Guid.NewGuid().ToString();
        var auth = Guid.NewGuid().ToString();

        _logicaSesionMock.
            Setup(l => l.ObtenerUsuarioPorToken(auth))
            .Returns(dueño);

        _logicaHogarMock
            .Setup(m => m.VerificarPermiso("ModificarAlias", dueño, hogarId))
            .Returns(true);

        _controlador.Modificar(_solicitudModificarHogar, hogarId, auth);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearCuartoConNombreNullOVacioLanzaExcepcion(string nombre)
    {
        var auth = Guid.NewGuid().ToString();
        var hogarId = Guid.NewGuid().ToString();

        var dueño = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        _logicaSesionMock.
            Setup(l => l.ObtenerUsuarioPorToken(auth))
            .Returns(dueño);

        _logicaHogarMock
            .Setup(m => m.VerificarPermiso("AdministrarCuarto", dueño, hogarId))
            .Returns(true);

        _solicitudAgregarCuarto = new CrearSolicitudAgregarCuarto
        {
            Nombre = nombre
        };

        _controlador.AgregarCuarto(_solicitudAgregarCuarto, hogarId, auth);
    }
    #endregion

    #region Exito

    [TestMethod]
    public void CrearConDatosValidosCreaHogarCorrectamente()
    {
        _solicitudCrearHogar = new CrearSolicitudHogar
        {
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = -50,
            CantMiembrosSoportados = 3,
            Alias = "Mi hogar"
        };

        var dueño = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var argsEsperados = new CrearHogaresArgs(
            _solicitudCrearHogar.Calle,
            _solicitudCrearHogar.NumPuerta,
            _solicitudCrearHogar.Latitud,
            _solicitudCrearHogar.Longitud,
            _solicitudCrearHogar.CantMiembrosSoportados,
            _solicitudCrearHogar.Alias,
            dueño);

        _logicaHogarMock.Setup(m => m.Agregar(It.IsAny<CrearHogaresArgs>()));
        _logicaHogarMock.Setup(m => m.GuardarCambios());

        var auth = Guid.NewGuid().ToString();

        _logicaSesionMock.
            Setup(l => l.ObtenerUsuarioPorToken(auth))
            .Returns(dueño);

        _controlador.Crear(_solicitudCrearHogar, auth);

        _logicaHogarMock.Verify(i => i.Agregar(It.Is<CrearHogaresArgs>(args =>
            args.Calle == argsEsperados.Calle &&
            args.NumPuerta == argsEsperados.NumPuerta &&
            args.Latitud == argsEsperados.Latitud &&
            args.Longitud == argsEsperados.Longitud &&
            args.CantMiembrosSoportados == argsEsperados.CantMiembrosSoportados)), Times.Once);

        _logicaHogarMock.Verify(i => i.GuardarCambios(), Times.Once);
    }

    [TestMethod]
    public void AgregarMiembroValidoLoAgregaCorrectamente()
    {
        var auth = Guid.NewGuid().ToString();
        var hogarId = Guid.NewGuid().ToString();
        var usuarioId = Guid.NewGuid();

        var usuario = new Usuario
        {
            Id = usuarioId,
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var hogar = new Hogar
        {
            Id = Guid.Parse(hogarId),
            Calle = "av italia",
            NumPuerta = 1,
            Latitud = 0,
            Longitud = 0,
            CantMiembrosSoportados = 4,
            Miembros = [],
            DueñoId = usuario.Id
        };

        _logicaSesionMock
            .Setup(m => m.ObtenerUsuarioPorToken(auth))
            .Returns(usuario);

        _logicaHogarMock
            .Setup(m => m.ObtenerPorId(hogarId))
            .Returns(hogar);

        _logicaUsuarioMock
            .Setup(m => m.ObtenerUsuarioPorEmail(It.IsAny<string>()))
            .Returns(usuario);

        var miembro = new MiembroHogar
        {
            Miembro = usuario,
            MiembroId = usuario.Id
        };

        _solicitudAgregarMiembro = new CrearSolicitudAgregarMiembro
        {
            Email = miembro.Miembro.Email
        };

        _controlador.AgregarMiembro(_solicitudAgregarMiembro, hogarId, auth);

        _logicaHogarMock.Verify(m => m.AgregarMiembro(hogarId, It.IsAny<MiembroHogar>(), usuario), Times.Once);
    }

    [TestMethod]
    public void AsociarDispositivoConDatosValidosDueñoDeberiaAsociarCorrectamente()
    {
        var auth = Guid.NewGuid().ToString();
        var hogarId = Guid.NewGuid().ToString();
        var dispositivoId = Guid.NewGuid().ToString();

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var hogar = new Hogar
        {
            Id = Guid.Parse(hogarId),
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = -50,
            CantMiembrosSoportados = 3
        };

        var dispositivo = new Dispositivo
        {
            Id = Guid.Parse(dispositivoId),
            Nombre = "Camara de seguridad",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia"
        };

        _solicitudAsociarDispositivo = new CrearSolicitudAsociarDispositivo
        {
            DispositivoId = dispositivoId
        };

        _logicaSesionMock.Setup(m => m.ObtenerUsuarioPorToken(auth)).Returns(usuario);
        _logicaHogarMock.Setup(m => m.VerificarPermiso("AsociarDispositivo", usuario, hogarId)).Returns(true);
        _logicaDispositivoMock.Setup(m => m.ObtenerPorId(dispositivoId)).Returns(dispositivo);
        _logicaHogarMock.Setup(m => m.ObtenerPorId(hogarId)).Returns(hogar);

        var argsEsperados = new CrearDispositivosHogarArgs(dispositivo, hogar);
        _logicaDispositivoHogarMock.Setup(m => m.Agregar(argsEsperados, usuario));

        _controlador.AsociarDispositivo(_solicitudAsociarDispositivo, hogarId, auth);

        _logicaDispositivoHogarMock.Verify(m => m.Agregar(It.Is<CrearDispositivosHogarArgs>(args =>
            args.Dispositivo.Id == dispositivo.Id &&
            args.Hogar.Id == hogar.Id), usuario), Times.Once);
    }

    [TestMethod]
    public void AsociarDispositivoConDatosValidosMiembroDeberiaAsociarCorrectamente()
    {
        var auth = Guid.NewGuid().ToString();
        var hogarId = Guid.NewGuid().ToString();
        var dispositivoId = Guid.NewGuid().ToString();

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var hogar = new Hogar
        {
            Id = Guid.Parse(hogarId),
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = -50,
            CantMiembrosSoportados = 3
        };

        var dispositivo = new Dispositivo
        {
            Id = Guid.Parse(dispositivoId),
            Nombre = "Camara de seguridad",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia"
        };

        _solicitudAsociarDispositivo = new CrearSolicitudAsociarDispositivo
        {
            DispositivoId = dispositivoId
        };

        _logicaSesionMock.Setup(m => m.ObtenerUsuarioPorToken(auth)).Returns(usuario);
        _logicaHogarMock.Setup(m => m.VerificarPermiso("AsociarDispositivo", usuario, hogarId)).Returns(true);
        _logicaDispositivoMock.Setup(m => m.ObtenerPorId(dispositivoId)).Returns(dispositivo);
        _logicaHogarMock.Setup(m => m.ObtenerPorId(hogarId)).Returns(hogar);

        var argsEsperados = new CrearDispositivosHogarArgs(dispositivo, hogar);
        _logicaDispositivoHogarMock.Setup(m => m.Agregar(argsEsperados, usuario));

        _controlador.AsociarDispositivo(_solicitudAsociarDispositivo, hogarId, auth);

        _logicaDispositivoHogarMock.Verify(m => m.Agregar(It.Is<CrearDispositivosHogarArgs>(args =>
            args.Dispositivo.Id == dispositivo.Id &&
            args.Hogar.Id == hogar.Id), usuario), Times.Once);
    }

    [TestMethod]
    public void ListarDispositivosDeberiaRetornarDispositivosCuandoEsDueñoHogar()
    {
        var auth = Guid.NewGuid().ToString();
        var hogarId = Guid.NewGuid().ToString();
        var usuario = new Usuario { Id = Guid.NewGuid() };

        var fotografia = new FotografiaDispositivo
        {
            EsPrincipal = true,
            Url = "foto"
        };

        var dispositivo1 = new Dispositivo
        {
            Id = Guid.NewGuid(),
            Nombre = "Dispositivo1",
            Fotografias = [fotografia]
        };

        var dispositivo2 = new Dispositivo
        {
            Id = Guid.NewGuid(),
            Nombre = "Dispositivo2",
            Fotografias = [fotografia]
        };

        var dispositivosHogar = new List<DispositivoHogar>
        {
            new DispositivoHogar { Id = Guid.NewGuid(), DispositivoId = dispositivo1.Id, Dispositivo = dispositivo1 },
            new DispositivoHogar { Id = Guid.NewGuid(), DispositivoId = dispositivo2.Id, Dispositivo = dispositivo2 }
        };

        _logicaEmpresaMock.Setup(m => m.ObtenerPorId(It.IsAny<Guid>())).Returns(new Empresa { Nombre = "empresa" });
        _logicaSesionMock.Setup(m => m.ObtenerUsuarioPorToken(auth)).Returns(usuario);
        _logicaHogarMock.Setup(m => m.VerificarPermiso("ListarDispositivo", usuario, hogarId)).Returns(true);
        _logicaDispositivoHogarMock.Setup(m => m.ObtenerDispositivosDeHogar(hogarId, usuario, It.IsAny<ParametroDispositivoHogarFiltro>())).Returns(dispositivosHogar);

        var dispositivosEsperados = dispositivosHogar.Select(dh => new InformacionRespuestaListarDispositivos(_logicaEmpresaMock.Object, dh)).ToList();

        var resultado = _controlador.ListarDispositivos(hogarId, auth, null);

        resultado.Should().NotBeNull();
        dispositivosEsperados[0].Nombre.Should().Be(resultado[0].Nombre);
        dispositivosEsperados[1].Nombre.Should().Be(resultado[1].Nombre);
    }

    [TestMethod]
    public void ListarDispositivosDeberiaRetornarDispositivosCuandoTienePermiso()
    {
        var auth = Guid.NewGuid().ToString();
        var hogarId = Guid.NewGuid().ToString();
        var usuario = new Usuario { Id = Guid.NewGuid() };

        var fotografia = new FotografiaDispositivo
        {
            EsPrincipal = true,
            Url = "foto"
        };

        var dispositivo1 = new Dispositivo
        {
            Id = Guid.NewGuid(),
            Nombre = "Dispositivo1",
            Fotografias = [fotografia],
            Tipo = TipoDispositivo.Lampara
        };

        var dispositivo2 = new Dispositivo
        {
            Id = Guid.NewGuid(),
            Nombre = "Dispositivo2",
            Fotografias = [fotografia],
            Tipo = TipoDispositivo.SensorVentana
        };

        var dispositivosHogar = new List<DispositivoHogar>
        {
            new DispositivoHogar { Id = Guid.NewGuid(), DispositivoId = dispositivo1.Id, Dispositivo = dispositivo1, EstaEncendida = true },
            new DispositivoHogar { Id = Guid.NewGuid(), DispositivoId = dispositivo2.Id, Dispositivo = dispositivo2, EstaAbierto = true }
        };

        _logicaEmpresaMock.Setup(m => m.ObtenerPorId(It.IsAny<Guid>())).Returns(new Empresa { Nombre = "empresa" });
        _logicaSesionMock.Setup(m => m.ObtenerUsuarioPorToken(auth)).Returns(usuario);
        _logicaHogarMock.Setup(m => m.VerificarPermiso("ListarDispositivo", usuario, hogarId)).Returns(true);
        _logicaDispositivoHogarMock.Setup(m => m.ObtenerDispositivosDeHogar(hogarId, usuario, It.IsAny<ParametroDispositivoHogarFiltro>())).Returns(dispositivosHogar);

        var dispositivosEsperados = dispositivosHogar.Select(dh => new InformacionRespuestaListarDispositivos(_logicaEmpresaMock.Object, dh)).ToList();

        var resultado = _controlador.ListarDispositivos(hogarId, auth, null);

        resultado.Should().NotBeNull();
        dispositivosEsperados[0].Nombre.Should().Be(resultado[0].Nombre);
        dispositivosEsperados[1].Nombre.Should().Be(resultado[1].Nombre);
        dispositivosEsperados[0].FotoPrincipal.Should().Be(resultado[0].FotoPrincipal);
        dispositivosEsperados[1].FotoPrincipal.Should().Be(resultado[1].FotoPrincipal);
        dispositivosEsperados[0].NombreEmpresa.Should().Be(resultado[0].NombreEmpresa);
        dispositivosEsperados[1].NombreEmpresa.Should().Be(resultado[1].NombreEmpresa);
        dispositivosEsperados[0].EstaEncendida.Should().Be(resultado[0].EstaEncendida);
        dispositivosEsperados[1].EstaAbierto.Should().Be(resultado[1].EstaAbierto);
    }

    [TestMethod]
    public void ListarMiembrosDueñoHogarRetornaListaDeMiembros()
    {
        var hogarId = Guid.NewGuid();
        var token = Guid.NewGuid().ToString();

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "usuario",
            Apellido = "x",
            Email = "email1@gmail.com"
        };

        var usuario2 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "usuario2",
            Apellido = "x",
            Email = "email2@gmail.com"
        };
        var miembros = new List<MiembroHogar>
        {
            new MiembroHogar { Id = Guid.NewGuid(), Miembro = usuario, MiembroId = usuario.Id },
            new MiembroHogar { Id = Guid.NewGuid(), Miembro = usuario2, MiembroId = usuario2.Id }
        };

        _logicaSesionMock.Setup(s => s.ObtenerUsuarioPorToken(token)).Returns(usuario);
        _logicaUsuarioMock.Setup(u => u.ObtenerUsuarioPorId(usuario.Id)).Returns(usuario);
        _logicaUsuarioMock.Setup(u => u.ObtenerUsuarioPorId(usuario2.Id)).Returns(usuario2);
        _logicaHogarMock.Setup(h => h.ObtenerMiembrosDeHogar(hogarId.ToString(), usuario)).Returns(miembros);

        var resultado = _controlador.ListarMiembros(hogarId.ToString(), token);

        resultado.Should().NotBeNull();
        resultado.Count.Should().Be(2);
        resultado[0].Email.Should().Be(usuario.Email);
        resultado[1].Email.Should().Be(usuario2.Email);
    }

    [TestMethod]
    public void ActualizarAliasDeberiaActualizarAliasCorrectamente()
    {
        var hogarId = Guid.NewGuid();
        var token = Guid.NewGuid().ToString();
        var auth = Guid.NewGuid().ToString();

        var hogar = new Hogar
        {
            Id = hogarId,
            Alias = "Hogar de Pepe",
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = 15,
            CantMiembrosSoportados = 3,
            DueñoId = Guid.NewGuid(),
            Miembros = []
        };

        _solicitudModificarHogar = new CrearSolicitudModificarHogar
        {
            Alias = "Mi hogar"
        };

        var dueño = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        _logicaSesionMock.
            Setup(l => l.ObtenerUsuarioPorToken(auth))
            .Returns(dueño);

        _logicaHogarMock
            .Setup(m => m.VerificarPermiso("ModificarAlias", dueño, hogarId.ToString()))
            .Returns(true);

        _logicaHogarMock.Setup(h => h.ActualizarAlias(hogarId.ToString(), _solicitudModificarHogar.Alias, dueño));

        _controlador.Modificar(_solicitudModificarHogar, hogarId.ToString(), auth);

        _logicaHogarMock.Verify(h => h.ActualizarAlias(hogarId.ToString(), _solicitudModificarHogar.Alias, dueño), Times.Once);
    }

    [TestMethod]
    public void AgregarCuartoConDatosYUsuarioValidosExito()
    {
        var auth = Guid.NewGuid().ToString();
        var hogarId = Guid.NewGuid();

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        _solicitudAgregarCuarto = new CrearSolicitudAgregarCuarto
        {
            Nombre = "cuarto"
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = 15,
            CantMiembrosSoportados = 3,
            DueñoId = usuario.Id,
            Miembros = []
        };

        var args = _solicitudAgregarCuarto.Args(hogar);

        _logicaSesionMock.
            Setup(l => l.ObtenerUsuarioPorToken(auth))
            .Returns(usuario);

        _logicaHogarMock
            .Setup(m => m.VerificarPermiso("AdministrarCuarto", usuario, hogarId.ToString()))
            .Returns(true);

        _logicaHogarMock.Setup(h => h.ObtenerPorId(hogarId.ToString())).Returns(hogar);

        _logicaCuartoMock.Setup(c => c.Agregar(args, usuario));

        _logicaHogarMock.Setup(h => h.GuardarCambios());

        _controlador.AgregarCuarto(_solicitudAgregarCuarto, hogarId.ToString(), auth);
    }

    [TestMethod]
    public void ListarHogaresUsaurioRetornaListaDeHogares()
    {
        var primerHogarId = Guid.NewGuid();
        var segundoHogarId = Guid.NewGuid();

        var token = Guid.NewGuid().ToString();

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "usuario",
            Apellido = "x",
            Email = "email1@gmail.com"
        };

        var hogar1 = new Hogar
        {
            Id = primerHogarId,
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = 15,
            CantMiembrosSoportados = 3,
            DueñoId = usuario.Id,
            Miembros = []
        };

        var miembros = new List<MiembroHogar>
        {
            new MiembroHogar { Id = Guid.NewGuid(), Miembro = usuario, MiembroId = usuario.Id },
        };

        var hogar2 = new Hogar
        {
            Id = segundoHogarId,
            Calle = "rivera",
            NumPuerta = 4321,
            Latitud = 65,
            Longitud = 38,
            CantMiembrosSoportados = 2,
            DueñoId = Guid.NewGuid(),
            Miembros = miembros
        };

        var hogares = new List<Hogar> { hogar1, hogar2 };

        _logicaSesionMock.Setup(s => s.ObtenerUsuarioPorToken(token)).Returns(usuario);
        _logicaHogarMock.Setup(h => h.ObtenerHogaresPorUsuario(usuario)).Returns(hogares);

        var result = _controlador.ListarHogaresDeUsuario(token);

        result.Should().NotBeNull();
        result.Count.Should().Be(2);
        result[0].Id.Should().Be(primerHogarId.ToString());
        result[1].Id.Should().Be(segundoHogarId.ToString());
    }

    [TestMethod]
    public void ListarCuartosConUsuarioValidoExito()
    {
        var auth = Guid.NewGuid().ToString();
        var hogarId = Guid.NewGuid();

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez",
            Rol = RolesPredefinidos.DueñoHogar
        };

        var hogar = new Hogar
        {
            Id = hogarId,
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = 15,
            CantMiembrosSoportados = 3,
            DueñoId = usuario.Id,
            Miembros = [],
            Cuartos = [
                new Cuarto { Id = Guid.NewGuid(), Nombre = "Cocina" },
                new Cuarto { Id = Guid.NewGuid(), Nombre = "Baño" }
            ]
        };

        _logicaSesionMock.
            Setup(l => l.ObtenerUsuarioPorToken(auth))
            .Returns(usuario);

        _logicaHogarMock.Setup(h => h.ObtenerCuartosDeHogar(hogarId.ToString(), usuario)).Returns(hogar.Cuartos);

        var respuesta = _controlador.ListarCuartos(hogarId.ToString(), auth);

        respuesta.Should().NotBeNull();
        respuesta.Count.Should().Be(2);
    }
    #endregion
}
#endregion
