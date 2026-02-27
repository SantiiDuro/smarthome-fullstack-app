using FluentAssertions;
using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;

namespace SmartHome.Persistencia.Test;

[TestClass]
public class DispositivoHogarRepositorioTest
{
    private readonly ContextoSql _contexto;
    private readonly DispositivoHogarRepositorio _repositorio;

    public DispositivoHogarRepositorioTest()
    {
        _contexto = ContextoSqlTests.CrearContextoMemoria();
        _repositorio = new DispositivoHogarRepositorio(_contexto);
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
        var dispositivoHogar = new DispositivoHogar()
        {
            Id = Guid.NewGuid(),
            Nombre = "dispositivo",
            DispositivoId = Guid.NewGuid(),
            HogarId = Guid.NewGuid(),
            EstaConectado = true
        };

        _repositorio.Agregar(dispositivoHogar);
        _repositorio.GuardarCambios();

        var dispositivoHogarEncontrado = _contexto.DispositivosHogar.Find(dispositivoHogar.Id);

        dispositivoHogarEncontrado.Id.Should().Be(dispositivoHogar.Id);
        dispositivoHogarEncontrado.DispositivoId.Should().Be(dispositivoHogar.DispositivoId);
        dispositivoHogarEncontrado.HogarId.Should().Be(dispositivoHogar.HogarId);
        dispositivoHogarEncontrado.EstaConectado.Should().Be(dispositivoHogar.EstaConectado);
    }
    #endregion

    #region ObtenerTodos
    [TestMethod]
    public void CuandoSeAgreganDispositivosHogarDeberiaRetornarlos()
    {
        var dispositivo = new Dispositivo
        {
            Nombre = "dispositivo",
            Modelo = "ASWQDE",
            Descripcion = "descripcion"
        };

        var hogar = new Hogar
        {
            Calle = "av italia"
        };

        var dh1 = new DispositivoHogar()
        {
            Hogar = hogar,
            Nombre = dispositivo.Nombre,
            Dispositivo = dispositivo,
            EstaConectado = false
        };

        var dh2 = new DispositivoHogar()
        {
            Hogar = hogar,
            Nombre = dispositivo.Nombre,
            Dispositivo = dispositivo,
            EstaConectado = true
        };

        _repositorio.Agregar(dh1);
        _repositorio.Agregar(dh2);

        _repositorio.GuardarCambios();

        var hogares = _repositorio.ObtenerTodos(null);

        hogares.Should().NotBeNull();
        hogares.Should().HaveCount(2);
        hogares.Should().ContainEquivalentOf(dh1);
        hogares.Should().ContainEquivalentOf(dh2);
    }

    [TestMethod]
    public void ObtenerTodosDeberiaRetornarDispositivosHogarConFiltroNombreCuarto()
    {
        var dispositivo = new Dispositivo
        {
            Nombre = "dispositivo",
            Modelo = "AQWSDE",
            Descripcion = "descripcion"
        };

        var cuarto1 = new Cuarto
        {
            Nombre = "sala"
        };

        var cuarto2 = new Cuarto
        {
            Nombre = "cocina"
        };

        var hogar = new Hogar
        {
            Calle = "av italia",
            Cuartos = [cuarto1, cuarto2]
        };

        var dh1 = new DispositivoHogar()
        {
            Hogar = hogar,
            Nombre = dispositivo.Nombre,
            Dispositivo = dispositivo,
            Cuarto = cuarto1,
            EstaConectado = false
        };

        var dh2 = new DispositivoHogar()
        {
            Hogar = hogar,
            Nombre = dispositivo.Nombre,
            Dispositivo = dispositivo,
            Cuarto = cuarto2,
            EstaConectado = true
        };

        _repositorio.Agregar(dh1);
        _repositorio.Agregar(dh2);
        _repositorio.GuardarCambios();

        var filtro = new ParametroDispositivoHogarFiltro()
        {
            NombreCuarto = "sala"
        };

        var hogares = _repositorio.ObtenerTodos(filtro);

        hogares.Should().NotBeNull();
        hogares.Should().HaveCount(1);
        hogares.Should().ContainEquivalentOf(dh1);
        hogares.Should().NotContainEquivalentOf(dh2);
    }
    #endregion

    #region Actualizar
    [TestMethod]
    public void CuandoSeActualizaDispositivoHogarDeberiaActualizarEnLaBaseDeDatos()
    {
        var dispositivoHogar = new DispositivoHogar()
        {
            Id = Guid.NewGuid(),
            Nombre = "dispositivo",
            DispositivoId = Guid.NewGuid(),
            HogarId = Guid.NewGuid(),
            EstaConectado = true
        };

        _repositorio.Agregar(dispositivoHogar);
        _repositorio.GuardarCambios();

        dispositivoHogar.EstaConectado = false;

        _repositorio.Actualizar(dispositivoHogar);

        var dispositivoHogarActualizado = _contexto.DispositivosHogar.Find(dispositivoHogar.Id);
        dispositivoHogarActualizado.Should().NotBeNull();
        dispositivoHogarActualizado.EstaConectado.Should().BeFalse();
    }
    #endregion
}
