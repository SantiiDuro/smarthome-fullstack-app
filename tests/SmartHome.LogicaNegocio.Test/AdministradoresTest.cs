using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public sealed class AdministradoresTest
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

    #region Crear
    #region Error
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearAdminConNombreNullOVacioLanzaExcepcion(string nombre)
    {
        var args = new CrearAdminsArgs(
            nombre,
            "Gomez",
            "pepeGomez@gmail.com",
            "pepe1234.");

        _servicio.AgregarAdmin(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearAdminConApellidoNullOVacioLanzaExcepcion(string apellido)
    {
        var args = new CrearAdminsArgs(
            "Pepe",
            apellido,
            "pepeGomez@gmail.com",
            "pepe1234.");

        _servicio.AgregarAdmin(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearAdminConEmailNullOVacioLanzaExcepcion(string email)
    {
        var args = new CrearAdminsArgs(
            "Pepe",
            "Gomez",
            email,
            "pepe1234.");

        _servicio.AgregarAdmin(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearAdminConContraseñaNullOVacioLanzaExcepcion(string contra)
    {
        var args = new CrearAdminsArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            contra);

        _servicio.AgregarAdmin(args);
    }

    [TestMethod]
    public void CrearAdminsConEmailDuplicadoLanzaExcepcion()
    {
        var args = new CrearAdminsArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe1234.");

        _logicaUsuarioMock
            .Setup(i => i.Existe(u => u.Email == args.Email))
            .Returns(true);

        var accion = () => _servicio.AgregarAdmin(args);

        accion.Should().Throw<ArgumentException>().WithMessage("El email ya está asociado a una cuenta.");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearAdminConEmailSinArrobaLanzaExcepcion()
    {
        var args = new CrearAdminsArgs(
            "Pepe",
            "Gomez",
            "pepegmail.com",
            "pepe1234.");

        _servicio.AgregarAdmin(args);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearAdminConEmailSinDominioLanzaExcepcion()
    {
        var args = new CrearAdminsArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail",
            "pepe1234.");

        _servicio.AgregarAdmin(args);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearAdminConContraseñaMenorA6CaracteresLanzaExcepcion()
    {
        var args = new CrearAdminsArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe");

        _servicio.AgregarAdmin(args);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearAdminConContraseñaSinCaracterEspecialLanzaExcepcion()
    {
        var args = new CrearAdminsArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe1234");

        _servicio.AgregarAdmin(args);
    }
    #endregion

    #region Success

    [TestMethod]
    public void CrearAdminExito()
    {
        var args = new CrearAdminsArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe1234.");

        var rolAdmin = RolesPredefinidos.Admin;

        _logicaUsuarioMock
            .Setup(i => i.ObtenerRolPorId(RolesPredefinidos.ID_ADMIN))
            .Returns(rolAdmin);

        _logicaUsuarioMock
            .Setup(i => i.Agregar(It.Is<Usuario>(u =>
                u.Id != Guid.Empty &&
                u.Nombre == args.Nombre &&
                u.Apellido == args.Apellido &&
                u.Email == args.Email &&
                u.Contraseña == args.Contraseña &&
                u.Rol == rolAdmin)));

        _logicaUsuarioMock
            .Setup(i => i.GuardarCambios());

        _logicaUsuarioMock
            .Setup(i => i.Existe(u => u.Email == args.Email))
            .Returns(false);

        var respuesta = _servicio.AgregarAdmin(args);

        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBe(Guid.Empty);
        respuesta.Id.Should().NotBeEmpty();

        respuesta.Nombre.Should().Be(args.Nombre);
        respuesta.Apellido.Should().Be(args.Apellido);
        respuesta.Email.Should().Be(args.Email);
        respuesta.Contraseña.Should().Be(args.Contraseña);
        respuesta.Rol.Should().Be(rolAdmin);
        respuesta.RolId.Should().Be(rolAdmin.Id);
        respuesta.Rol.Tipo.Should().Be("administrador");
    }
    #endregion
}
#endregion
