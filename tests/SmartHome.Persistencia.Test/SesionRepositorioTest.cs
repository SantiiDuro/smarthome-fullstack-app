using FluentAssertions;
using SmartHome.LogicaNegocio.Sesiones.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.Persistencia.Test;

[TestClass]
public class SesionRepositorioTest
{
    private readonly ContextoSql _contexto;
    private readonly SesionRepositorio _repositorio;

    public SesionRepositorioTest()
    {
        _contexto = ContextoSqlTests.CrearContextoMemoria();
        _repositorio = new SesionRepositorio(_contexto);
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
    #region Exito

    [TestMethod]
    public void CuandoSeProporcionaInfoDeberiaAgregarseALaBaseDeDatos()
    {
        var rol = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = []
        };

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol,
            RolId = rol.Id,
            FotoPerfil = "/downloads/pepeGomez"
        };

        var sesion = new Sesion
        {
            Id = Guid.NewGuid(),
            Token = Guid.NewGuid().ToString(),
            Usuario = usuario
        };

        _repositorio.Agregar(sesion);
        _repositorio.GuardarCambios();

        var sesionEncontrada = _contexto.Sesiones.Find(sesion.Id);

        sesionEncontrada.Id.Should().Be(sesion.Id);
        sesionEncontrada.Token.Should().Be(sesion.Token);
    }
    #endregion
    #endregion

    #region ObtenerTodos

    [TestMethod]
    public void ObtenerTodosDeberiaRetornarSesiones()
    {
        var rol = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = []
        };

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol,
            RolId = rol.Id,
            FotoPerfil = "/downloads/pepeGomez"
        };

        var sesion = new Sesion
        {
            Id = Guid.NewGuid(),
            Token = Guid.NewGuid().ToString(),
            Usuario = usuario
        };

        _contexto.Sesiones.Add(sesion);
        _contexto.SaveChanges();

        var sesiones = _repositorio.ObtenerTodos();

        sesiones.Should().ContainSingle(s => s.Id == sesion.Id);
    }

    #endregion

    #region Eliminar
    [TestMethod]
    public void EliminarDeberiaEliminarSesion()
    {
        var rol = new Rol
        {
            Id = Guid.NewGuid(),
            Tipo = "dueño hogar",
            Permisos = []
        };

        var usuario = new Usuario
        {
            Nombre = "Pepe",
            Apellido = "Gomez",
            Email = "pepegomez@gmail.com",
            Contraseña = "pepe1234.",
            Rol = rol,
            RolId = rol.Id,
            FotoPerfil = "/downloads/pepeGomez"
        };

        var sesion = new Sesion
        {
            Id = Guid.NewGuid(),
            Token = Guid.NewGuid().ToString(),
            Usuario = usuario
        };

        _contexto.Sesiones.Add(sesion);
        _contexto.SaveChanges();

        var sesionAntesDeEliminar = _contexto.Sesiones.FirstOrDefault(s => s.Token == sesion.Token);

        sesionAntesDeEliminar.Should().NotBeNull();

        _repositorio.Eliminar(sesion.Token);
        _repositorio.GuardarCambios();

        var sesionDespuesDeEliminar = _contexto.Sesiones.FirstOrDefault(s => s.Token == sesion.Token);

        sesionDespuesDeEliminar.Should().BeNull();
    }
    #endregion
}
