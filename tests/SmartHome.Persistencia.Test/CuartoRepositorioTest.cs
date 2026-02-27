using FluentAssertions;
using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;

namespace SmartHome.Persistencia.Test;

[TestClass]
public class CuartoRepositorioTest
{
    private readonly ContextoSql _contexto;
    private readonly CuartoRepositorio _repositorio;

    public CuartoRepositorioTest()
    {
        _contexto = ContextoSqlTests.CrearContextoMemoria();
        _repositorio = new CuartoRepositorio(_contexto);
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

    #region Crear
    [TestMethod]
    public void CuandoSeProporcionaInfoDeberiaAgregarseALaBaseDeDatos()
    {
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

        var cuarto = new Cuarto
        {
            Nombre = "Habitacion",
            Hogar = hogar,
            HogarId = hogar.Id,
            DispositivosHogar = []
        };

        _repositorio.Agregar(cuarto);
        _repositorio.GuardarCambios();

        var cuartoEncontrado = _contexto.Cuartos.Find(cuarto.Id);

        cuartoEncontrado.Should().NotBeNull();
        cuartoEncontrado.Id.Should().Be(cuarto.Id);
        cuartoEncontrado.Nombre.Should().Be(cuarto.Nombre);
        cuartoEncontrado.Hogar.Should().Be(cuarto.Hogar);
        cuartoEncontrado.HogarId.Should().Be(cuarto.HogarId);
        cuartoEncontrado.DispositivosHogar.Should().Equal(cuarto.DispositivosHogar);
    }
    #endregion

    #region Existe
    [TestMethod]
    public void CuandoExisteCuartoConMismoNombreEnHogarDeberiaRetornarTrue()
    {
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

        var cuarto = new Cuarto
        {
            Nombre = "Habitacion",
            Hogar = hogar,
            HogarId = hogar.Id,
            DispositivosHogar = []
        };

        _repositorio.Agregar(cuarto);
        _repositorio.GuardarCambios();

        var existe = _repositorio.Existe(c => c.Nombre == cuarto.Nombre && c.HogarId == cuarto.HogarId);

        existe.Should().BeTrue();
    }
    #endregion

    #region ObtenerPorId
    [TestMethod]
    public void ObtenerPorIdExito()
    {
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

        var cuarto = new Cuarto
        {
            Nombre = "Habitacion",
            Hogar = hogar,
            HogarId = hogar.Id,
            DispositivosHogar = []
        };

        _repositorio.Agregar(cuarto);
        _repositorio.GuardarCambios();

        var cuartoEncontrado = _repositorio.ObtenerPorId(cuarto.Id);

        cuartoEncontrado.Should().NotBeNull();
        cuartoEncontrado.Id.Should().Be(cuarto.Id);
        cuartoEncontrado.Nombre.Should().Be(cuarto.Nombre);
        cuartoEncontrado.Hogar.Should().Be(cuarto.Hogar);
        cuartoEncontrado.HogarId.Should().Be(cuarto.HogarId);
        cuartoEncontrado.DispositivosHogar.Should().Equal(cuarto.DispositivosHogar);
    }
    #endregion
}
