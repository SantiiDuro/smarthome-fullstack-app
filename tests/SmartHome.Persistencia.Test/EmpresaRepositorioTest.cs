using FluentAssertions;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.Persistencia.Test;

[TestClass]
public class EmpresaRepositorioTest
{
    private readonly ContextoSql _contexto;
    private readonly EmpresaRepositorio _repositorio;

    public EmpresaRepositorioTest()
    {
        _contexto = ContextoSqlTests.CrearContextoMemoria();
        _repositorio = new EmpresaRepositorio(_contexto);
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
    #region Éxito
    [TestMethod]
    public void CuandoSeProporcionaInfoDeberiaAgregarseALaBaseDeDatos()
    {
        var empresa = new Empresa
        {
            Nombre = "ORT",
            Logotipo = "ort.png",
            Rut = "123456789",
            NombreCreador = "Juan",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        _repositorio.Agregar(empresa);
        _repositorio.GuardarCambios();

        var empresaEncontrada = _contexto.Empresas.Find(empresa.Id);

        empresaEncontrada.Should().NotBeNull();
        empresaEncontrada.Id.Should().Be(empresa.Id);
        empresaEncontrada.Nombre.Should().Be(empresa.Nombre);
        empresaEncontrada.Logotipo.Should().Be(empresa.Logotipo);
        empresaEncontrada.Rut.Should().Be(empresa.Rut);
        empresaEncontrada.NombreCreador.Should().Be(empresa.NombreCreador);
        empresaEncontrada.Validador.Should().Be(empresa.Validador);
    }
    #endregion
    #endregion

    #region ObtenerTodos
    #region Éxito
    [TestMethod]
    public void CuandoHayEmpresasDeberiaDevolverUnaListaConTodasLasEmpresas()
    {
        var empresa1 = new Empresa
        {
            Nombre = "ORT",
            Logotipo = "ort.png",
            Rut = "123456789",
            NombreCreador = "Juan",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var empresa2 = new Empresa
        {
            Nombre = "UTEC",
            Logotipo = "utec.png",
            Rut = "987654321",
            NombreCreador = "Pedro",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        _repositorio.Agregar(empresa1);
        _repositorio.Agregar(empresa2);
        _repositorio.GuardarCambios();

        var empresas = _repositorio.ObtenerTodos(null, null);

        empresas.Should().NotBeNull();
        empresas.Empresas.Should().HaveCount(2);
        empresas.Empresas.Should().ContainEquivalentOf(empresa1);
        empresas.Empresas.Should().ContainEquivalentOf(empresa2);
    }

    [TestMethod]
    public void CuandoHayEmpresasDeberiaDevolverUnaListaCon1Empresa()
    {
        var empresa1 = new Empresa
        {
            Nombre = "ORT",
            Logotipo = "ort.png",
            Rut = "123456789",
            NombreCreador = "Juan",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var empresa2 = new Empresa
        {
            Nombre = "UTEC",
            Logotipo = "utec.png",
            Rut = "987654321",
            NombreCreador = "Pedro",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        _repositorio.Agregar(empresa1);
        _repositorio.Agregar(empresa2);
        _repositorio.GuardarCambios();

        var parametros = new ParametroPaginacion(1, 1);
        var empresas = _repositorio.ObtenerTodos(parametros, null);

        empresas.Should().NotBeNull();
        empresas.Empresas.Should().HaveCount(1);
        empresas.Empresas.Should().ContainEquivalentOf(empresa1);
        empresas.Empresas.Should().NotContainEquivalentOf(empresa2);
        empresas.CantidadPaginas.Should().Be(2);
    }

    [TestMethod]
    public void CuandoHayEmpresasDeberiaDevolverUnaListaCon2EmpresasFiltradaPorNombreORT()
    {
        var empresa1 = new Empresa
        {
            Nombre = "ORT",
            Logotipo = "ort.png",
            Rut = "123456789",
            NombreCreador = "Juan",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var empresa2 = new Empresa
        {
            Nombre = "UTEC",
            Logotipo = "utec.png",
            Rut = "987654321",
            NombreCreador = "Pedro",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var empresa3 = new Empresa
        {
            Nombre = "ORT",
            Logotipo = "ort.png",
            Rut = "145asd6789",
            NombreCreador = "Juan Pedro",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        _repositorio.Agregar(empresa1);
        _repositorio.Agregar(empresa2);
        _repositorio.Agregar(empresa3);
        _repositorio.GuardarCambios();

        var filtro = new ParametroEmpresaFiltro("ORT", null);
        var empresas = _repositorio.ObtenerTodos(null, filtro);

        empresas.Should().NotBeNull();
        empresas.Empresas.Should().HaveCount(2);
        empresas.Empresas.Should().ContainEquivalentOf(empresa1);
        empresas.Empresas.Should().NotContainEquivalentOf(empresa2);
        empresas.Empresas.Should().ContainEquivalentOf(empresa3);
    }

    [TestMethod]
    public void CuandoHayEmpresasDeberiaDevolverUnaListaCon1EmpresaFiltradaPorNombreORTYNombreCreadorJuanPedro()
    {
        var empresa1 = new Empresa
        {
            Nombre = "ORT",
            Logotipo = "ort.png",
            Rut = "123456789",
            NombreCreador = "Juan",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var empresa2 = new Empresa
        {
            Nombre = "UTEC",
            Logotipo = "utec.png",
            Rut = "987654321",
            NombreCreador = "Pedro",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var empresa3 = new Empresa
        {
            Nombre = "ORT",
            Logotipo = "ort.png",
            Rut = "145asd6789",
            NombreCreador = "Juan Pedro",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        _repositorio.Agregar(empresa1);
        _repositorio.Agregar(empresa2);
        _repositorio.Agregar(empresa3);
        _repositorio.GuardarCambios();

        var filtro = new ParametroEmpresaFiltro("ORT", "Juan Pedro");
        var empresas = _repositorio.ObtenerTodos(null, filtro);

        empresas.Should().NotBeNull();
        empresas.Empresas.Should().HaveCount(1);
        empresas.Empresas.Should().ContainEquivalentOf(empresa3);
        empresas.Empresas.Should().NotContainEquivalentOf(empresa1);
        empresas.Empresas.Should().NotContainEquivalentOf(empresa2);
    }
    #endregion
    #region ObtenerPorId
    [TestMethod]
    public void ObtenerPorIdExito()
    {
        var empresa1 = new Empresa
        {
            Nombre = "ORT",
            Logotipo = "ort.png",
            Rut = "123456789",
            NombreCreador = "Juan",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var empresa2 = new Empresa
        {
            Nombre = "UTEC",
            Logotipo = "utec.png",
            Rut = "987654321",
            NombreCreador = "Pedro",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        _repositorio.Agregar(empresa1);
        _repositorio.Agregar(empresa2);
        _repositorio.GuardarCambios();

        var empresa = _repositorio.ObtenerPorId(empresa1.Id);

        empresa.Should().NotBeNull();
        empresa.Should().BeEquivalentTo(empresa1);
    }
    #endregion
    #endregion

    #region Existe
    [TestMethod]
    public void CuandoExisteEmpresaConNombreDeberiaRetornarTrue()
    {
        var empresa = new Empresa
        {
            Nombre = "ORT",
            Logotipo = "ort.png",
            Rut = "123456789",
            NombreCreador = "Juan",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        _repositorio.Agregar(empresa);
        _repositorio.GuardarCambios();

        var existe = _repositorio.Existe(e => e.Nombre == empresa.Nombre);

        existe.Should().BeTrue();
    }
    #endregion
}
