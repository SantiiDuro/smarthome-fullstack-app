using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Cuartos;
using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public class CuartosTest
{
    private Mock<ICuartoRepositorio> _logicaCuartoMock = null!;
    private Mock<IHogarLogica> _logicaHogarMock = null!;
    private CuartoLogica _servicio = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaCuartoMock = new Mock<ICuartoRepositorio>(MockBehavior.Strict);
        _logicaHogarMock = new Mock<IHogarLogica>(MockBehavior.Strict);
        _servicio = new CuartoLogica(_logicaCuartoMock.Object, _logicaHogarMock.Object);
    }

    #region Error
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearCuartoConNombreVacioONullLanzaExcepcion(string nombre)
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

        var args = new CrearCuartosArgs(
            nombre,
            new Hogar());

        _servicio.Agregar(args, usuario);
    }

    [TestMethod]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearCuartoConHogarNullLanzaExcepcion(Hogar hogar)
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

        var args = new CrearCuartosArgs(
            "Mi cuarto",
            hogar);

        _servicio.Agregar(args, usuario);
    }

    [TestMethod]
    public void CrearCuartoConNombreExistenteEnHogarLanzaExcepcion()
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
            Calle = "av italia",
            NumPuerta = 123,
            Latitud = 10,
            Longitud = 0,
            CantMiembrosSoportados = 5,
            DueñoId = Guid.NewGuid(),
            Miembros = []
        };

        var args = new CrearCuartosArgs(
            "Mi cuarto",
            hogar);

        _logicaCuartoMock
            .Setup(r => r.Existe(c => c.HogarId == args.Hogar.Id && c.Nombre == args.Nombre))
            .Returns(true);

        _logicaCuartoMock
            .Setup(r => r.Agregar(It.Is<Cuarto>(c =>
                c.Id != Guid.Empty &&
                c.Nombre == args.Nombre &&
                c.Hogar == args.Hogar &&
                c.HogarId == args.Hogar.Id)));

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AdministrarCuarto", usuario, args.Hogar.Id.ToString()))
            .Returns(true);

        var accion = () => _servicio.Agregar(args, usuario);

        accion.Should().Throw<InvalidOperationException>().WithMessage("Ya existe un cuarto con el mismo nombre en el hogar");
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ObtenerPorIdConIdInvalidoLanzaExcepcion()
    {
        _servicio.ObtenerPorId("invalido");
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void ObtenerPorIdConIdNoExistenteLanzaExcepcion()
    {
        var id = Guid.NewGuid();

        _logicaCuartoMock
            .Setup(r => r.Existe(It.IsAny<Expression<Func<Cuarto, bool>>>()))
            .Returns(false);

        _servicio.ObtenerPorId(id.ToString());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearCuartoSinPermisoLanzaExcepcion()
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
            Calle = "av italia",
            NumPuerta = 123,
            Latitud = 10,
            Longitud = 0,
            CantMiembrosSoportados = 5,
            DueñoId = Guid.NewGuid(),
            Miembros = []
        };

        var args = new CrearCuartosArgs(
            "Mi cuarto",
            hogar);

        _logicaCuartoMock
            .Setup(r => r.Agregar(It.Is<Cuarto>(c =>
                c.Id != Guid.Empty &&
                c.Nombre == args.Nombre &&
                c.Hogar == args.Hogar &&
                c.HogarId == args.Hogar.Id)));

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AdministrarCuarto", usuario, args.Hogar.Id.ToString()))
            .Returns(false);

        _servicio.Agregar(args, usuario);
    }
    #endregion

    #region Exito
    [TestMethod]
    public void CrearCuartoExito()
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
            Calle = "av italia",
            NumPuerta = 123,
            Latitud = 10,
            Longitud = 0,
            CantMiembrosSoportados = 5,
            DueñoId = Guid.NewGuid(),
            Miembros = []
        };

        var args = new CrearCuartosArgs(
            "Mi cuarto",
            hogar);

        _logicaCuartoMock
            .Setup(r => r.Agregar(It.Is<Cuarto>(c =>
                c.Id != Guid.Empty &&
                c.Nombre == args.Nombre &&
                c.Hogar == args.Hogar &&
                c.HogarId == args.Hogar.Id)));

        _logicaCuartoMock
            .Setup(r => r.Existe(c => c.HogarId == args.Hogar.Id && c.Nombre == args.Nombre))
            .Returns(false);

        _logicaCuartoMock
            .Setup(r => r.GuardarCambios());

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AdministrarCuarto", usuario, args.Hogar.Id.ToString()))
            .Returns(true);

        var respuesta = _servicio.Agregar(args, usuario);

        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBeEmpty();
        respuesta.Id.Should().NotBe(Guid.Empty);

        respuesta.Nombre.Should().Be(args.Nombre);
        respuesta.Hogar.Should().Be(args.Hogar);
    }

    [TestMethod]
    public void ObtenerPorIdConIdValidoRetornaCuarto()
    {
        var id = Guid.NewGuid();

        var cuarto = new Cuarto
        {
            Id = id,
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

        _logicaCuartoMock
            .Setup(r => r.Existe(It.IsAny<Expression<Func<Cuarto, bool>>>()))
            .Returns(true);

        _logicaCuartoMock
            .Setup(r => r.ObtenerPorId(It.Is<Guid>(g => g == id)))
            .Returns(cuarto);

        var respuesta = _servicio.ObtenerPorId(id.ToString());

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().Be(id);
        respuesta.Nombre.Should().Be(cuarto.Nombre);
        respuesta.Hogar.Should().Be(cuarto.Hogar);
    }
    #endregion
}
