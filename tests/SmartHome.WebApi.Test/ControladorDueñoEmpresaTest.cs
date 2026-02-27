using Moq;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.DueñosEmpresa;
using SmartHome.WebApi.Controllers.DueñosEmpresa.Modelos;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ControladorDueñoEmpresaTest
{
    private CrearSolicitudDueñoEmpresa _solicitud = null!;
    private Mock<IUsuarioLogica> _logicaUsuarioMock = null!;
    private ControladorDueñoEmpresa _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaUsuarioMock = new Mock<IUsuarioLogica>(MockBehavior.Default);
        _controlador = new ControladorDueñoEmpresa(_logicaUsuarioMock.Object);
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
        _solicitud = new CrearSolicitudDueñoEmpresa
        {
            Nombre = nombre,
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234."
        };

        _controlador.Crear(_solicitud);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConApellidoNullOVacioLanzaExcepcion(string apellido)
    {
        _solicitud = new CrearSolicitudDueñoEmpresa
        {
            Nombre = "pepe",
            Apellido = apellido,
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234."
        };

        _controlador.Crear(_solicitud);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConEmailNullOVacioLanzaExcepcion(string email)
    {
        _solicitud = new CrearSolicitudDueñoEmpresa
        {
            Nombre = "pepe",
            Apellido = "Gomez",
            Email = email,
            Contraseña = "pepe1234."
        };

        _controlador.Crear(_solicitud);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearConContraseñaNullOVacioLanzaExcepcion(string contra)
    {
        _solicitud = new CrearSolicitudDueñoEmpresa
        {
            Nombre = "pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = contra
        };

        _controlador.Crear(_solicitud);
    }
    #endregion

    #region Exito

    [TestMethod]
    public void CrearConDatosValidosCreaUsuarioCorrectamente()
    {
        var solicitud = new CrearSolicitudDueñoEmpresa
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234."
        };

        var argsEsperados = new CrearDueñosEmpresaArgs(
            solicitud.Nombre,
            solicitud.Apellido,
            solicitud.Email,
            solicitud.Contraseña);

        _logicaUsuarioMock.Setup(m => m.AgregarDueñoEmpresa(It.IsAny<CrearDueñosEmpresaArgs>()));
        _logicaUsuarioMock.Setup(m => m.GuardarCambios());

        _controlador.Crear(solicitud);

        _logicaUsuarioMock.Verify(i => i.AgregarDueñoEmpresa(It.Is<CrearDueñosEmpresaArgs>(args =>
            args.Nombre == argsEsperados.Nombre &&
            args.Apellido == argsEsperados.Apellido &&
            args.Email == argsEsperados.Email &&
            args.Contraseña == argsEsperados.Contraseña)), Times.Once);

        _logicaUsuarioMock.Verify(i => i.GuardarCambios(), Times.Once);
    }
    #endregion
}
#endregion

