using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public sealed class DueñosEmpresaTest
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
    public void CrearDueñoEmpresaConNombreNullOVacioLanzaExcepcion(string nombre)
    {
        var args = new CrearDueñosEmpresaArgs(
            nombre,
            "Gomez",
            "pepeGomez@gmail.com",
            "pepe1234.");

        _servicio.AgregarDueñoEmpresa(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDueñoEmpresaConApellidoNullOVacioLanzaExcepcion(string apellido)
    {
        var args = new CrearDueñosEmpresaArgs(
            "Pepe",
            apellido,
            "pepeGomez@gmail.com",
            "pepe1234.");

        _servicio.AgregarDueñoEmpresa(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDueñoEmpresaConEmailNullOVacioLanzaExcepcion(string email)
    {
        var args = new CrearDueñosEmpresaArgs(
            "Pepe",
            "Gomez",
            email,
            "pepe1234.");

        _servicio.AgregarDueñoEmpresa(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDueñoEmpresaConContraseñaNullOVacioLanzaExcepcion(string contra)
    {
        var args = new CrearDueñosEmpresaArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            contra);

        _servicio.AgregarDueñoEmpresa(args);
    }

    [TestMethod]
    public void CrearDueñoEmpresaConEmailDuplicadoLanzaExcepcion()
    {
        var args = new CrearDueñosEmpresaArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe1234.");

        _logicaUsuarioMock
            .Setup(i => i.Existe(u => u.Email == args.Email))
            .Returns(true);

        var accion = () => _servicio.AgregarDueñoEmpresa(args);

        accion.Should().Throw<ArgumentException>().WithMessage("El email ya está asociado a una cuenta.");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearDueñoEmpresaConEmailSinArrobaLanzaExcepcion()
    {
        var args = new CrearDueñosEmpresaArgs(
            "Pepe",
            "Gomez",
            "pepegmail.com",
            "pepe1234.");

        _servicio.AgregarDueñoEmpresa(args);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearDueñoEmpresaConEmailSinDominioLanzaExcepcion()
    {
        var args = new CrearDueñosEmpresaArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail",
            "pepe1234.");

        _servicio.AgregarDueñoEmpresa(args);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearDueñoEmpresaConContraseñaMenorA6CaracteresLanzaExcepcion()
    {
        var args = new CrearDueñosEmpresaArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe");

        _servicio.AgregarDueñoEmpresa(args);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearDueñoEmpresaConContraseñaSinCaracterEspecialLanzaExcepcion()
    {
        var args = new CrearDueñosEmpresaArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe1234");

        _servicio.AgregarDueñoEmpresa(args);
    }
    #endregion

    #region Exito
    [TestMethod]
    public void CrearDueñoEmpresaExito()
    {
        var args = new CrearDueñosEmpresaArgs(
            "Pepe",
            "Gomez",
            "pepe@gmail.com",
            "pepe1234.");

        var rolDueñoEmpresa = RolesPredefinidos.DueñoEmpresa;

        _logicaUsuarioMock
            .Setup(i => i.ObtenerRolPorId(RolesPredefinidos.ID_DUEÑO_EMPRESA))
            .Returns(rolDueñoEmpresa);

        _logicaUsuarioMock
            .Setup(i => i.Agregar(It.Is<Usuario>(u =>
                u.Id != Guid.Empty &&
                u.Nombre == args.Nombre &&
                u.Apellido == args.Apellido &&
                u.Email == args.Email &&
                u.Contraseña == args.Contraseña &&
                u.Rol == rolDueñoEmpresa)));

        _logicaUsuarioMock
            .Setup(i => i.GuardarCambios());

        _logicaUsuarioMock
            .Setup(i => i.Existe(u => u.Email == args.Email))
            .Returns(false);

        var respuesta = _servicio.AgregarDueñoEmpresa(args);

        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBe(Guid.Empty);
        respuesta.Id.Should().NotBeEmpty();

        respuesta.Nombre.Should().Be(args.Nombre);
        respuesta.Apellido.Should().Be(args.Apellido);
        respuesta.Email.Should().Be(args.Email);
        respuesta.Contraseña.Should().Be(args.Contraseña);
        respuesta.Rol.Should().Be(rolDueñoEmpresa);
        respuesta.RolId.Should().Be(rolDueñoEmpresa.Id);
        respuesta.Rol.Tipo.Should().Be("dueño empresa");
    }
    #endregion
}
#endregion
