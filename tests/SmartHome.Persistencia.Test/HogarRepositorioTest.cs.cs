using FluentAssertions;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Notificaciones.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.Persistencia.Test;

[TestClass]
public class HogarRepositorioTest
{
    private readonly ContextoSql _contexto;
    private readonly HogarRepositorio _repositorio;

    public HogarRepositorioTest()
    {
        _contexto = ContextoSqlTests.CrearContextoMemoria();
        _repositorio = new HogarRepositorio(_contexto);
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
    [TestMethod]
    public void CuandoSeProporcionaInfoDeberiaAgregarseALaBaseDeDatos()
    {
        var hogar = new Hogar()
        {
            Id = Guid.NewGuid(),
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = -15,
            CantMiembrosSoportados = 3
        };

        _repositorio.Agregar(hogar);
        _repositorio.GuardarCambios();

        var hogarEncontrado = _contexto.Hogares.Find(hogar.Id);

        hogarEncontrado.Id.Should().Be(hogar.Id);
        hogarEncontrado.Calle.Should().Be(hogar.Calle);
        hogarEncontrado.NumPuerta.Should().Be(hogar.NumPuerta);
        hogarEncontrado.Latitud.Should().Be(hogar.Latitud);
        hogarEncontrado.Longitud.Should().Be(hogar.Longitud);
        hogarEncontrado.CantMiembrosSoportados.Should().Be(hogar.CantMiembrosSoportados);
    }
    #endregion

    #region ObtenerTodos

    [TestMethod]
    public void CuandoSeAgreganHogaresDeberiaRetornarlos()
    {
        var hogar = new Hogar()
        {
            Id = Guid.NewGuid(),
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = -15,
            CantMiembrosSoportados = 3
        };

        var hogar2 = new Hogar()
        {
            Id = Guid.NewGuid(),
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = -15,
            CantMiembrosSoportados = 3
        };

        _repositorio.Agregar(hogar);
        _repositorio.Agregar(hogar2);

        _repositorio.GuardarCambios();

        var hogares = _repositorio.ObtenerTodos();

        hogares.Should().NotBeNull();
        hogares.Should().HaveCount(2);
        hogares.Should().ContainEquivalentOf(hogar);
        hogares.Should().ContainEquivalentOf(hogar2);
    }
    #endregion

    #region ModificarMiembros
    [TestMethod]
    public void CuandoSeAgregaMiembroDeberiaAgregarseAlHogar()
    {
        var hogar = new Hogar()
        {
            Id = Guid.NewGuid(),
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = -15,
            CantMiembrosSoportados = 3,
            Miembros = []
        };

        _repositorio.Agregar(hogar);
        _repositorio.GuardarCambios();

        var usuario = new Usuario
        {
            Nombre = "pepe",
            Apellido = "perez",
            Email = "pepe@gmail.com",
            Contraseña = "pepe1234.",
        };

        var nuevoMiembro = new MiembroHogar()
        {
            Miembro = usuario,
            Hogar = hogar,
            HogarId = hogar.Id
        };

        _repositorio.AgregarMiembro(nuevoMiembro);

        hogar.Miembros.Should().Contain(miembro => miembro.Miembro == usuario);
        hogar.Miembros.Should().HaveCount(1);
    }

    [TestMethod]
    public void CuandoSeActualizaMiembroDeberiaActualizarseEnElHogar()
    {
        var hogar = new Hogar()
        {
            Id = Guid.NewGuid(),
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = -15,
            CantMiembrosSoportados = 3,
            Miembros = []
        };

        _repositorio.Agregar(hogar);
        _repositorio.GuardarCambios();

        var usuario = new Usuario
        {
            Nombre = "pepe",
            Apellido = "perez",
            Email = "pepe@gmail.com",
            Contraseña = "pepe1234.",
        };

        var nuevoMiembro = new MiembroHogar()
        {
            Miembro = usuario,
            Hogar = hogar,
            HogarId = hogar.Id,
            Notificaciones = []
        };

        hogar.Miembros.Add(nuevoMiembro);

        hogar.Miembros.Where(m => m.Id == nuevoMiembro.Id).First().Notificaciones.Count.Should().Be(0);

        nuevoMiembro.Notificaciones.Add(new Notificacion { Evento = "evento" });

        _contexto.MiembrosHogar.Add(nuevoMiembro);
        _contexto.SaveChanges();

        _repositorio.ActualizarMiembro(nuevoMiembro);

        hogar.Miembros.Where(m => m.Id == nuevoMiembro.Id).First().Notificaciones.Count.Should().Be(1);
    }

    #endregion

    #region Existe
    [TestMethod]
    public void ExisteDeberiaRetornarTrueCuandoHogarExiste()
    {
        var hogar = new Hogar
        {
            Id = Guid.NewGuid(),
            Calle = "av italia",
            NumPuerta = 1234,
            Latitud = 60,
            Longitud = -15,
            CantMiembrosSoportados = 3
        };

        _repositorio.Agregar(hogar);
        _repositorio.GuardarCambios();

        var existe = _repositorio.Existe(h => h.Id == hogar.Id);

        existe.Should().BeTrue();
    }

    [TestMethod]
    public void ExisteDeberiaRetornarFalseCuandoHogarNoExiste()
    {
        var hogarIdInexistente = Guid.NewGuid();

        var existe = _repositorio.Existe(h => h.Id == hogarIdInexistente);

        existe.Should().BeFalse();
    }
    #endregion

    #region Actualizar
    [TestMethod]
    public void CuandoSeActualizaHogarDeberiaActualizarEnLaBaseDeDatos()
    {
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

        _repositorio.Agregar(hogar);
        _repositorio.GuardarCambios();

        hogar.Alias = "Hogar de Juan";

        _repositorio.Actualizar(hogar);

        var hogarActualizado = _contexto.Hogares.Find(hogar.Id);
        hogarActualizado.Should().NotBeNull();
        hogarActualizado.Alias.Should().Be("Hogar de Juan");
    }
    #endregion
}
