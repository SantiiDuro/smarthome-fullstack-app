using Moq;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.Administradores;
using SmartHome.WebApi.Controllers.Administradores.Modelos;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ControladorAdministradorTest
{
    private CrearSolicitudAdministrador _solicitud = null!;
    private Mock<IUsuarioLogica> _logicaUsuarioMock = null!;
    private Mock<ISesionLogica> _logicaSesionMock = null!;
    private ControladorAdministrador _controlador = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaSesionMock = new Mock<ISesionLogica>(MockBehavior.Default);
        _logicaUsuarioMock = new Mock<IUsuarioLogica>(MockBehavior.Default);
        _controlador = new ControladorAdministrador(_logicaUsuarioMock.Object, _logicaSesionMock.Object);
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
        _solicitud = new CrearSolicitudAdministrador
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
        _solicitud = new CrearSolicitudAdministrador
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
        _solicitud = new CrearSolicitudAdministrador
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
        _solicitud = new CrearSolicitudAdministrador
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
        var solicitud = new CrearSolicitudAdministrador
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepeGomez@gmail.com",
            Contraseña = "pepe1234."
        };

        var argsEsperados = new CrearAdminsArgs(
            solicitud.Nombre,
            solicitud.Apellido,
            solicitud.Email,
            solicitud.Contraseña);

        _logicaUsuarioMock.Setup(m => m.AgregarAdmin(It.IsAny<CrearAdminsArgs>()));
        _logicaUsuarioMock.Setup(m => m.GuardarCambios());

        _controlador.Crear(solicitud);

        _logicaUsuarioMock.Verify(i => i.AgregarAdmin(It.Is<CrearAdminsArgs>(args =>
            args.Nombre == argsEsperados.Nombre &&
            args.Apellido == argsEsperados.Apellido &&
            args.Email == argsEsperados.Email &&
            args.Contraseña == argsEsperados.Contraseña)), Times.Once);

        _logicaUsuarioMock.Verify(i => i.GuardarCambios(), Times.Once);
    }
    #endregion
    #region Eliminar
    [TestMethod]
    public void EliminarAdminEliminaCorrectamente()
    {
        var token = Guid.NewGuid().ToString();
        var email = "eliminado@gmail.com";

        var adminQueElimina = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_ADMIN,
            FotoPerfil = "/downloads/cocoPerez"
        };

        _logicaSesionMock.Setup(m => m.ObtenerUsuarioPorToken(It.Is<string>(t => t == token)))
                         .Returns(adminQueElimina);

        _logicaUsuarioMock.Setup(m => m.EliminarAdmin(adminQueElimina, email));

        _controlador.Eliminar(email, token);

        _logicaUsuarioMock.Verify(m => m.EliminarAdmin(adminQueElimina, email), Times.Once);
    }
    #endregion
}
#endregion
