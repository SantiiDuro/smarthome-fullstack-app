using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.DueñosHogar;
using SmartHome.WebApi.Controllers.DueñosHogar.Modelos;

namespace SmartHome.WebApi.Test;
[TestClass]
public class ControladorDueñoHogarTest
{
    private CrearSolicitudDueñoHogar _solicitud = null!;
    private Mock<IUsuarioLogica> _logicaUsuarioMock = null!;
    private Mock<IHogarLogica> _logicaHogarMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;
    private ControladorDueñoHogar _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaUsuarioMock = new Mock<IUsuarioLogica>(MockBehavior.Default);
        _logicaHogarMock = new Mock<IHogarLogica>(MockBehavior.Default);
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);
        _controlador = new ControladorDueñoHogar(_logicaUsuarioMock.Object, _logicaHogarMock.Object, _logicaSesionMock.Object);
    }

    #region Create
    #region Error
    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void CrearConArgsNullLanzaExcepcion()
    {
        _controlador.Crear(null);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConNombreNullOVacioLanzaExcepcion(string nombre)
    {
        _solicitud = new CrearSolicitudDueñoHogar
        {
            Nombre = nombre,
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez"
        };

        _controlador.Crear(_solicitud);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConApellidoNullOVacioLanzaExcepcion(string apellido)
    {
        _solicitud = new CrearSolicitudDueñoHogar
        {
            Nombre = "pepe",
            Apellido = apellido,
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez"
        };

        _controlador.Crear(_solicitud);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConEmailNullOVacioLanzaExcepcion(string email)
    {
        _solicitud = new CrearSolicitudDueñoHogar
        {
            Nombre = "pepe",
            Apellido = "gomez",
            Email = email,
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez"
        };

        _controlador.Crear(_solicitud);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConContraseñalNullOVacioLanzaExcepcion(string contraseña)
    {
        _solicitud = new CrearSolicitudDueñoHogar
        {
            Nombre = "pepe",
            Apellido = "gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = contraseña,
            FotoPerfil = "/downloads/pepeGomez"
        };

        _controlador.Crear(_solicitud);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConFotoPerfillNullOVacioLanzaExcepcion(string fotoPerfil)
    {
        _solicitud = new CrearSolicitudDueñoHogar
        {
            Nombre = "pepe",
            Apellido = "gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = fotoPerfil
        };

        _controlador.Crear(_solicitud);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearConEmailSinArrobaLanzaExcepcion()
    {
        _solicitud = new CrearSolicitudDueñoHogar
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez"
        };

        _controlador.Crear(_solicitud);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearConEmailSinDominioLanzaExcepcion()
    {
        _solicitud = new CrearSolicitudDueñoHogar
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepe@gmail",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez"
        };

        _controlador.Crear(_solicitud);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearConContraseñaMenorA6CaracteresLanzaExcepcion()
    {
        _solicitud = new CrearSolicitudDueñoHogar
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepe@gmail.com",
            Contraseña = "pepe.",
            FotoPerfil = "/downloads/pepeGomez"
        };

        _controlador.Crear(_solicitud);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearConContraseñaSinCaracterEspecialLanzaExcepcion()
    {
        _solicitud = new CrearSolicitudDueñoHogar
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepe@gmail.com",
            Contraseña = "pepe.",
            FotoPerfil = "/downloads/pepeGomez"
        };

        _controlador.Crear(_solicitud);
    }
    #endregion

    #region Exito

    [TestMethod]
    public void CrearConDatosValidosCreaUsuarioCorrectamente()
    {
        var solicitud = new CrearSolicitudDueñoHogar
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez"
        };

        var argsEsperados = new CrearDueñosHogarArgs(
            solicitud.Nombre,
            solicitud.Apellido,
            solicitud.Email,
            solicitud.Contraseña,
            solicitud.FotoPerfil);

        _logicaUsuarioMock.Setup(m => m.AgregarDueñoHogar(It.IsAny<CrearDueñosHogarArgs>()));
        _logicaUsuarioMock.Setup(m => m.GuardarCambios());

        _controlador.Crear(solicitud);

        _logicaUsuarioMock.Verify(i => i.AgregarDueñoHogar(It.Is<CrearDueñosHogarArgs>(args =>
            args.Nombre == argsEsperados.Nombre &&
            args.Apellido == argsEsperados.Apellido &&
            args.Email == argsEsperados.Email &&
            args.Contraseña == argsEsperados.Contraseña &&
            args.FotoPerfil == argsEsperados.FotoPerfil)), Times.Once);

        _logicaUsuarioMock.Verify(i => i.GuardarCambios(), Times.Once);
    }

    #endregion

    #region PermisosHogar
    [TestMethod]
    public void ObtenerPermisosDeMiembroHogarExito()
    {
        var token = Guid.NewGuid().ToString();

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepe@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez"
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

        _logicaSesionMock
            .Setup(ls => ls.ObtenerUsuarioPorToken(It.Is<string>(t => t == token)))
            .Returns(usuario);

        _logicaHogarMock
            .Setup(lh => lh.TienePermisoAsociarDispositivo(It.Is<Usuario>(u => u == usuario), It.Is<string>(h => h == hogar.Id.ToString())))
            .Returns(true);

        _logicaHogarMock
            .Setup(lh => lh.TienePermisoListarDispositivos(It.Is<Usuario>(u => u == usuario), It.Is<string>(h => h == hogar.Id.ToString())))
            .Returns(false);

        _logicaHogarMock
            .Setup(lh => lh.TienePermisoAdministrarCuartos(It.Is<Usuario>(u => u == usuario), It.Is<string>(h => h == hogar.Id.ToString())))
            .Returns(true);

        _logicaHogarMock
            .Setup(lh => lh.TienePermisoModificarNombreDispositivos(It.Is<Usuario>(u => u == usuario), It.Is<string>(h => h == hogar.Id.ToString())))
            .Returns(false);

        _logicaHogarMock
            .Setup(lh => lh.EsDueñoHogar(It.Is<Usuario>(u => u == usuario), It.Is<string>(h => h == hogar.Id.ToString())))
            .Returns(false);

        var permisos = _controlador.ObtenerPermisosSobreHogar(hogar.Id.ToString(), token);

        permisos.PermisoAsociarDispositivos.Should().BeTrue();
        permisos.PermisoListarDispositivos.Should().BeFalse();
        permisos.PermisoAdministrarCuartos.Should().BeTrue();
        permisos.PermisoModificarNombreDispositivos.Should().BeFalse();
        permisos.PermisoAgregarMiembros.Should().BeFalse();
        permisos.PermisoListarMiembros.Should().BeFalse();
        permisos.PermisoModificarAlias.Should().BeFalse();
    }

    [TestMethod]
    public void ObtenerPermisosDeDueñoHogarExito()
    {
        var token = Guid.NewGuid().ToString();

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepe@gmail.com",
            Contraseña = "pepe1234.",
            FotoPerfil = "/downloads/pepeGomez"
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

        _logicaSesionMock
            .Setup(ls => ls.ObtenerUsuarioPorToken(It.Is<string>(t => t == token)))
            .Returns(usuario);

        _logicaHogarMock
            .Setup(lh => lh.EsDueñoHogar(It.Is<Usuario>(u => u == usuario), It.Is<string>(h => h == hogar.Id.ToString())))
            .Returns(true);

        var permisos = _controlador.ObtenerPermisosSobreHogar(hogar.Id.ToString(), token);

        permisos.PermisoAsociarDispositivos.Should().BeTrue();
        permisos.PermisoListarDispositivos.Should().BeTrue();
        permisos.PermisoAdministrarCuartos.Should().BeTrue();
        permisos.PermisoModificarNombreDispositivos.Should().BeTrue();
        permisos.PermisoAgregarMiembros.Should().BeTrue();
        permisos.PermisoListarMiembros.Should().BeTrue();
        permisos.PermisoModificarAlias.Should().BeTrue();
    }
    #endregion
}
#endregion
