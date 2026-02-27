using FluentAssertions;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.Persistencia.Test;

[TestClass]
public class DispositivoRepositorioTest
{
    private readonly ContextoSql _contexto;
    private readonly DispositivoRepositorio _repositorio;
    private readonly EmpresaRepositorio _repositorioEmpresa;

    public DispositivoRepositorioTest()
    {
        _contexto = ContextoSqlTests.CrearContextoMemoria();
        _repositorio = new DispositivoRepositorio(_contexto);
        _repositorioEmpresa = new EmpresaRepositorio(_contexto);
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
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };

        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia
        };

        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            NombreCreador = "pepe",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var dispositivo = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Nombre = "c410",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };

        _repositorio.Agregar(dispositivo);
        _repositorio.GuardarCambios();

        var dispositivosGuardados = _repositorio.ObtenerTodos(null, null);
        var dispositivoGuardado = dispositivosGuardados.Dispositivos[0];

        dispositivoGuardado.Id.Should().Be(dispositivo.Id);
        dispositivoGuardado.Nombre.Should().Be(dispositivo.Nombre);
        dispositivoGuardado.Modelo.Should().Be(dispositivo.Modelo);
        dispositivoGuardado.Descripcion.Should().Be(dispositivo.Descripcion);
        dispositivoGuardado.Fotografias.Should().Equal(dispositivo.Fotografias);
        dispositivoGuardado.EmpresaId.Should().Be(dispositivo.EmpresaId);
    }
    #endregion
    #endregion

    #region ObtenerTodos
    [TestMethod]
    public void ObtenerTodosCuandoExisteUnoDeberiaRetornarUno()
    {
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };

        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia
        };

        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            NombreCreador = "pepe",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var dispositivo = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Nombre = "c410",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };

        _repositorio.Agregar(dispositivo);
        _repositorio.GuardarCambios();

        var dispositivosGuardados = _repositorio.ObtenerTodos(null, null);

        dispositivosGuardados.Dispositivos.Count.Should().Be(1);
        dispositivosGuardados.CantidadPaginas.Should().Be(1);
    }

    [TestMethod]
    public void ObtenerTiposDispositivo()
    {
        var tipos = _repositorio.ObtenerTiposDeDispositivos();

        tipos.Count.Should().Be(Enum.GetNames(typeof(TipoDispositivo)).Length);
    }

    [TestMethod]
    public void ObtenerTodosDeberiaRetornar1DispositivoConFiltroNombreDispositivo()
    {
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };
        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia
        };
        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            NombreCreador = "pepe",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var dispositivo1 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Nombre = "c410",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };
        var dispositivo2 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Nombre = "F677",
            Modelo = "AQWSDS",
            Descripcion = "Dispositivo",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };

        _repositorio.Agregar(dispositivo1);
        _repositorio.Agregar(dispositivo2);
        _repositorio.GuardarCambios();

        var paginacion = new ParametroPaginacion(1, 10);
        var filtro = new ParametroDispositivoFiltro("c410", null, null, null);
        var dispositivosGuardados = _repositorio.ObtenerTodos(paginacion, filtro);

        dispositivosGuardados.Dispositivos.Count.Should().Be(1);
        dispositivosGuardados.Dispositivos.Should().ContainEquivalentOf(dispositivo1);
        dispositivosGuardados.Dispositivos.Should().NotContainEquivalentOf(dispositivo2);
        dispositivosGuardados.CantidadPaginas.Should().Be(1);
    }

    [TestMethod]
    public void ObtenerTodosDeberiaRetornar1DispositivoConFiltroNumeroModelo()
    {
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };
        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia
        };
        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            NombreCreador = "pepe",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var dispositivo1 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Nombre = "c410",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };
        var dispositivo2 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Nombre = "F677",
            Modelo = "AQWSDS",
            Descripcion = "Dispositivo",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };

        _repositorio.Agregar(dispositivo1);
        _repositorio.Agregar(dispositivo2);
        _repositorio.GuardarCambios();

        var paginacion = new ParametroPaginacion(1, 10);
        var filtro = new ParametroDispositivoFiltro(null, "AQWSDS", null, null);
        var dispositivosGuardados = _repositorio.ObtenerTodos(paginacion, filtro);

        dispositivosGuardados.Dispositivos.Count.Should().Be(1);
        dispositivosGuardados.Dispositivos.Should().ContainEquivalentOf(dispositivo2);
        dispositivosGuardados.Dispositivos.Should().NotContainEquivalentOf(dispositivo1);
        dispositivosGuardados.CantidadPaginas.Should().Be(1);
    }

    [TestMethod]
    public void ObtenerTodosDeberiaRetornar1DispositivoConFiltroTipoDispositivo()
    {
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };
        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia
        };
        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            NombreCreador = "pepe",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var dispositivo1 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Tipo = TipoDispositivo.Camara,
            Nombre = "c410",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };
        var dispositivo2 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Tipo = TipoDispositivo.SensorVentana,
            Nombre = "F677",
            Modelo = "AQWSDS",
            Descripcion = "Dispositivo",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };

        _repositorio.Agregar(dispositivo1);
        _repositorio.Agregar(dispositivo2);
        _repositorio.GuardarCambios();

        var paginacion = new ParametroPaginacion(1, 10);
        var filtro = new ParametroDispositivoFiltro(null, null, null, "SensorVentana");
        var dispositivosGuardados = _repositorio.ObtenerTodos(paginacion, filtro);

        dispositivosGuardados.Dispositivos.Count.Should().Be(1);
        dispositivosGuardados.Dispositivos.Should().ContainEquivalentOf(dispositivo2);
        dispositivosGuardados.Dispositivos.Should().NotContainEquivalentOf(dispositivo1);
        dispositivosGuardados.CantidadPaginas.Should().Be(1);
    }

    [TestMethod]
    public void ObtenerTodosDeberiaRetornar1DispositivoConFiltroNombreEmpresa()
    {
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };
        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia
        };
        var empresa1 = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "UM",
            Logotipo = "/downloads/UM",
            Rut = "123456",
            NombreCreador = "pepe",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var empresa2 = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "ORT",
            Logotipo = "/downloads/ORT",
            Rut = "1238-9",
            NombreCreador = "juan",
            Validador = "Reflection.ValidadorAulas6Letras"
        };
        var dispositivo1 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Nombre = "c410",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa2.Id
        };
        var dispositivo2 = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Nombre = "F677",
            Modelo = "AQWSDS",
            Descripcion = "Dispositivo",
            Fotografias = fotografias,
            EmpresaId = empresa1.Id
        };

        _repositorio.Agregar(dispositivo1);
        _repositorio.Agregar(dispositivo2);
        _repositorio.GuardarCambios();

        _repositorioEmpresa.Agregar(empresa1);
        _repositorioEmpresa.Agregar(empresa2);
        _repositorioEmpresa.GuardarCambios();

        var filtro = new ParametroDispositivoFiltro(null, null, "ORT", null);
        var parametros = new ParametroPaginacion(0, 10);
        var dispositivos = _repositorio.ObtenerTodos(parametros, filtro);

        dispositivos.Dispositivos.Count.Should().Be(1);
        dispositivos.Dispositivos.Should().ContainEquivalentOf(dispositivo1);
        dispositivos.Dispositivos.Should().NotContainEquivalentOf(dispositivo2);
        dispositivos.CantidadPaginas.Should().Be(1);
    }
    #endregion

    #region Existe
    [TestMethod]
    public void ExisteDeberiaRetornarTrueCuandoDispositivoExiste()
    {
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };

        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia
        };

        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            NombreCreador = "pepe",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var dispositivo = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Nombre = "c410",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };

        _repositorio.Agregar(dispositivo);
        _repositorio.GuardarCambios();

        var existe = _repositorio.Existe(d => d.Id == dispositivo.Id);

        existe.Should().BeTrue();
    }

    [TestMethod]
    public void ExisteDeberiaRetornarFalseCuandoDispositivoNoExiste()
    {
        var dispositivoIdInexistente = Guid.NewGuid();

        var existe = _repositorio.Existe(d => d.Id == dispositivoIdInexistente);

        existe.Should().BeFalse();
    }
    #endregion

    #region ObtenerPorId
    [TestMethod]
    public void ObtenerporIdRetornaDispositivo()
    {
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };

        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia
        };

        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            NombreCreador = "pepe",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var dispositivo = new Dispositivo()
        {
            Id = Guid.NewGuid(),
            Nombre = "c410",
            Modelo = "AQWSDE",
            Descripcion = "Dispositivo para videovigilancia",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };

        _repositorio.Agregar(dispositivo);
        _repositorio.GuardarCambios();

        var resultado = _repositorio.ObtenerPorId(dispositivo.Id);

        resultado.Should().NotBeNull();
        resultado.Should().Be(dispositivo);
    }
    #endregion
}
