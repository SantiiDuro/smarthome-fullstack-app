using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Test;
[TestClass]
public sealed class DueñoHogarTest
{
    private Mock<IUsuarioRepositorio> _logicaUsuarioMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;
    private UsuarioLogica _servicio = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaUsuarioMock = new Mock<IUsuarioRepositorio>(MockBehavior.Strict);
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);
        _servicio = new UsuarioLogica(_logicaUsuarioMock.Object, _logicaSesionMock.Object);
    }

    #region Create
    #region Error
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDueñoHogarConNombreNullOVacioLanzaExcepcion(string nombre)
    {
        var args = new CrearDueñosHogarArgs(
            nombre,
            "Gomez",
            "pepeGomez@gmail.com",
            "pepe1234.",
            "/downloads/pepeGomez");

        _servicio.AgregarDueñoHogar(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDueñoHogarConApellidoNullOVacioLanzaExcepcion(string apellido)
    {
        var args = new CrearDueñosHogarArgs(
            "Pepe",
            apellido,
            "pepeGomez@gmail.com",
            "pepe1234.",
            "/downloads/pepeGomez");

        _servicio.AgregarDueñoHogar(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDueñoHogarConEmailNullOVacioLanzaExcepcion(string email)
    {
        var args = new CrearDueñosHogarArgs(
            "Pepe",
            "Gomez",
            email,
            "pepe1234.",
            "/downloads/pepeGomez");

        _servicio.AgregarDueñoHogar(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDueñoHogarConContraseñaNullOVacioLanzaExcepcion(string contra)
    {
        var args = new CrearDueñosHogarArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            contra,
            "/downloads/pepeGomez");

        _servicio.AgregarDueñoHogar(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDueñoHogarConFotoDePerfilNullOVacioLanzaExcepcion(string fotoDePerfil)
    {
        var args = new CrearDueñosHogarArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe1234.",
            fotoDePerfil);

        _servicio.AgregarDueñoHogar(args);
    }

    [TestMethod]
    public void CrearDueñoHogarConEmailDuplicadoLanzaExcepcion()
    {
        var args = new CrearDueñosHogarArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe1234.",
            "/downloads/pepeGomez");

        _logicaUsuarioMock
            .Setup(i => i.Existe(u => u.Email == args.Email))
            .Returns(true);

        var accion = () => _servicio.AgregarDueñoHogar(args);

        accion.Should().Throw<ArgumentException>().WithMessage("El email ya está asociado a una cuenta.");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearDueñoHogarConEmailSinArrobaLanzaExcepcion()
    {
        var args = new CrearDueñosHogarArgs(
            "Pepe",
            "Gomez",
            "pepegmail.com",
            "pepe1234.",
            "/downloads/pepeGomez");

        _servicio.AgregarDueñoHogar(args);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearDueñoHogarConEmailSinDominioLanzaExcepcion()
    {
        var args = new CrearDueñosHogarArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail",
            "pepe1234.",
            "/downloads/pepeGomez");

        _servicio.AgregarDueñoHogar(args);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearDueñoHogarConContraseñaMenorA6CaracteresLanzaExcepcion()
    {
        var args = new CrearDueñosHogarArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe",
            "/downloads/pepeGomez");

        _servicio.AgregarDueñoHogar(args);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearDueñoHogarConContraseñaSinCaracterEspecialLanzaExcepcion()
    {
        var args = new CrearDueñosHogarArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe1234",
            "/downloads/pepeGomez");

        _servicio.AgregarDueñoHogar(args);
    }
    #endregion

    #region Exito

    [TestMethod]
    public void CrearDueñoHogarExito()
    {
        var args = new CrearDueñosHogarArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe1234.",
            "downloads/perfil");

        var rolDueñoHogar = RolesPredefinidos.DueñoHogar;

        _logicaUsuarioMock
            .Setup(i => i.ObtenerRolPorId(RolesPredefinidos.ID_DUEÑO_HOGAR))
            .Returns(rolDueñoHogar);

        _logicaUsuarioMock
            .Setup(i => i.Agregar(It.Is<Usuario>(u =>
                u.Id != Guid.Empty &&
                u.Nombre == args.Nombre &&
                u.Apellido == args.Apellido &&
                u.Email == args.Email &&
                u.Contraseña == args.Contraseña &&
                u.FotoPerfil == args.FotoPerfil &&
                u.Rol == rolDueñoHogar)));

        _logicaUsuarioMock
            .Setup(i => i.GuardarCambios());

        _logicaUsuarioMock
            .Setup(i => i.Existe(u => u.Email == args.Email))
            .Returns(false);

        var respuesta = _servicio.AgregarDueñoHogar(args);

        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBe(Guid.Empty);
        respuesta.Id.Should().NotBeEmpty();

        respuesta.Nombre.Should().Be(args.Nombre);
        respuesta.Apellido.Should().Be(args.Apellido);
        respuesta.Email.Should().Be(args.Email);
        respuesta.Contraseña.Should().Be(args.Contraseña);
        respuesta.FotoPerfil.Should().Be(args.FotoPerfil);
        respuesta.Rol.Should().Be(rolDueñoHogar);
        respuesta.RolId.Should().Be(rolDueñoHogar.Id);
        respuesta.Rol.Tipo.Should().Be("dueño hogar");
    }
    #endregion
}
#endregion
