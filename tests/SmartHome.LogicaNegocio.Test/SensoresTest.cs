using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public sealed class SensoresTest
{
    private Mock<IDispositivoRepositorio> _logicaDispositivoMock = null!;
    private Mock<IEmpresaRepositorio> _logicaEmpresaMock = null!;
    private DispositivoLogica _servicio = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaDispositivoMock = new Mock<IDispositivoRepositorio>(MockBehavior.Strict);
        _logicaEmpresaMock = new Mock<IEmpresaRepositorio>(MockBehavior.Strict);
        _servicio = new DispositivoLogica(_logicaDispositivoMock.Object, _logicaEmpresaMock.Object);
    }

    #region Create
    #region Error
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDispositivoConNombreNullOVacioLanzaExcepcion(string nombre)
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
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var args = new CrearSensoresArgs(
            nombre,
            "ASDFGH",
            "Dispositivo para videovigilancia",
            fotografias,
            empresa);

        _servicio.AgregarSensorVentana(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDispositivoConModeloNullOVacioLanzaExcepcion(string modelo)
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
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var args = new CrearSensoresArgs(
            "c410",
            modelo,
            "Dispositivo para videovigilancia",
            fotografias,
            empresa);

        _servicio.AgregarSensorVentana(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDispositivoConDescripcionNullOVacioLanzaExcepcion(string descripcion)
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
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var args = new CrearSensoresArgs(
            "c410",
            "ASDFGH",
            descripcion,
            fotografias,
            empresa);

        _servicio.AgregarSensorVentana(args);
    }

    [TestMethod]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDispositivoConFotografiasNullLanzaExcepcion(List<FotografiaDispositivo> fotografias)
    {
        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var args = new CrearSensoresArgs(
            "c410",
            "ASDFGH",
            "Dispositivo para videovigilancia",
            fotografias,
            empresa);

        _servicio.AgregarSensorVentana(args);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearDispositivoSinFotografiaPrincipalLanzaExcepcion()
    {
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410v2",
            EsPrincipal = false
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
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var args = new CrearSensoresArgs(
            "c410",
            "ASDFGH",
            "Dispositivo para videovigilancia",
            fotografias,
            empresa);

        _servicio.AgregarSensorVentana(args);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearDispositivoConMasDe1FotografiaPrincipalLanzaExcepcion()
    {
        var fotografia1 = new FotografiaDispositivo
        {
            Url = "/downloads/c410v2",
            EsPrincipal = true
        };

        var fotografia2 = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };

        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia1,
            fotografia2
        };

        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var args = new CrearSensoresArgs(
            "c410",
            "ASDFGH",
            "Dispositivo para videovigilancia",
            fotografias,
            empresa);

        _servicio.AgregarSensorVentana(args);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDispositivoConEmpresaNullLanzaExcepcion()
    {
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410v2",
            EsPrincipal = true
        };

        var fotografias = new List<FotografiaDispositivo>
        {
            fotografia
        };

        var args = new CrearSensoresArgs(
            "c410",
            "ASDFGH",
            "Dispositivo para videovigilancia",
            fotografias,
            null!);

        _servicio.AgregarSensorVentana(args);
    }

    [TestMethod]
    public void CrearDispositivoQueEmpresaYaRegistroLanzaExcepcion()
    {
        var fotografia = new FotografiaDispositivo
        {
            Url = "/downloads/c410",
            EsPrincipal = true
        };

        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var args = new CrearSensoresArgs(
            "c410",
            "ASDFGH",
            "Dispositivo para videovigilancia",
            [fotografia],
           empresa);

        var dispositivo = new Dispositivo
        {
            Id = Guid.NewGuid(),
            Nombre = args.Nombre,
            Modelo = args.Modelo,
            Descripcion = args.Descripcion,
            Fotografias = args.Fotografias,
            EmpresaId = empresa.Id
        };

        _logicaEmpresaMock.Setup(lh => lh.ObtenerPorId(empresa.Id)).Returns(empresa);

        _logicaDispositivoMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Dispositivo, bool>>>(e => e.Compile()(new Dispositivo
            {
                EmpresaId = empresa.Id,
                Nombre = args.Nombre,
                Modelo = args.Modelo
            }))))
            .Returns(true);

        var accion = () => _servicio.AgregarSensorVentana(args);

        accion.Should().Throw<ArgumentException>().WithMessage("La empresa ya registró un dispositivo con el mismo nombre y número de modelo.");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearSensorMovimientoConModeloDiferenteAlValidadorLanzaExcepcion()
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
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var args = new CrearSensoresArgs(
            "c410",
            "ASDCDXZSDX",
            "Lampara",
            fotografias,
            empresa);

        _logicaDispositivoMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Dispositivo, bool>>>(e => e.Compile()(new Dispositivo
            {
                EmpresaId = empresa.Id,
                Nombre = args.Nombre,
                Modelo = args.Modelo
            }))))
            .Returns(false);

        _logicaEmpresaMock.Setup(lh => lh.ObtenerPorId(empresa.Id)).Returns(empresa);

        _servicio.AgregarSensorMovimiento(args);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearSensorVentanaConModeloDiferenteAlValidadorLanzaExcepcion()
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
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var args = new CrearSensoresArgs(
            "c410",
            "ASDCDXZSDX",
            "Lampara",
            fotografias,
            empresa);

        _logicaDispositivoMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Dispositivo, bool>>>(e => e.Compile()(new Dispositivo
            {
                EmpresaId = empresa.Id,
                Nombre = args.Nombre,
                Modelo = args.Modelo
            }))))
            .Returns(false);

        _logicaEmpresaMock.Setup(lh => lh.ObtenerPorId(empresa.Id)).Returns(empresa);

        _servicio.AgregarSensorVentana(args);
    }
    #endregion

    #region Exito
    [TestMethod]
    public void CrearSensorExito()
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
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var args = new CrearSensoresArgs(
            "c410",
            "ASDFGH",
            "Dispositivo para videovigilancia",
            fotografias,
            empresa);

        _logicaDispositivoMock
            .Setup(i => i.Agregar(It.Is<Dispositivo>(d =>
                d.Id != Guid.Empty &&
                d.Nombre == args.Nombre &&
                d.Modelo == args.Modelo &&
                d.Descripcion == args.Descripcion &&
                d.Fotografias == args.Fotografias &&
                d.EmpresaId == args.Empresa.Id)));

        _logicaDispositivoMock
            .Setup(i => i.GuardarCambios());

        _logicaDispositivoMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Dispositivo, bool>>>(e => e.Compile()(new Dispositivo
            {
                EmpresaId = empresa.Id,
                Nombre = args.Nombre,
                Modelo = args.Modelo
            }))))
            .Returns(false);

        _logicaEmpresaMock.Setup(lh => lh.ObtenerPorId(empresa.Id)).Returns(empresa);

        var respuesta = _servicio.AgregarSensorVentana(args);
        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBeEmpty();
        respuesta.Id.Should().NotBe(Guid.Empty);

        respuesta.Tipo.Should().Be(TipoDispositivo.SensorVentana);
        respuesta.Nombre.Should().Be(args.Nombre);
        respuesta.Modelo.Should().Be(args.Modelo);
        respuesta.Descripcion.Should().Be(args.Descripcion);
        respuesta.Fotografias.Should().Equal(args.Fotografias);
        respuesta.EmpresaId.Should().Be(args.Empresa.Id);
    }

    [TestMethod]
    public void CrearSensorMovimientoExito()
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
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var args = new CrearSensoresArgs(
            "c410",
            "ASDFGH",
            "Dispositivo para detectar movimiento",
            fotografias,
            empresa);

        _logicaDispositivoMock
            .Setup(i => i.Agregar(It.Is<Dispositivo>(d =>
                d.Id != Guid.Empty &&
                d.Nombre == args.Nombre &&
                d.Modelo == args.Modelo &&
                d.Descripcion == args.Descripcion &&
                d.Fotografias == args.Fotografias &&
                d.EmpresaId == args.Empresa.Id)));

        _logicaDispositivoMock
            .Setup(i => i.GuardarCambios());

        _logicaDispositivoMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Dispositivo, bool>>>(e => e.Compile()(new Dispositivo
            {
                EmpresaId = empresa.Id,
                Nombre = args.Nombre,
                Modelo = args.Modelo
            }))))
            .Returns(false);

        _logicaEmpresaMock.Setup(lh => lh.ObtenerPorId(empresa.Id)).Returns(empresa);

        var respuesta = _servicio.AgregarSensorMovimiento(args);
        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBeEmpty();
        respuesta.Id.Should().NotBe(Guid.Empty);

        respuesta.Tipo.Should().Be(TipoDispositivo.SensorMovimiento);
        respuesta.Nombre.Should().Be(args.Nombre);
        respuesta.Modelo.Should().Be(args.Modelo);
        respuesta.Descripcion.Should().Be(args.Descripcion);
        respuesta.Fotografias.Should().Equal(args.Fotografias);
        respuesta.EmpresaId.Should().Be(args.Empresa.Id);
    }
    #endregion

    #region Metodos
    [TestMethod]
    public void ObtenerTodosLosDispositivosExito()
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
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var dispositivosEsperados = new List<Dispositivo>
        {
            new Dispositivo { Id = Guid.NewGuid(), Nombre = "c410", Modelo = "ASDFGH", Descripcion = "Dispositivo 1", Fotografias = fotografias, EmpresaId = empresa.Id },
            new Dispositivo { Id = Guid.NewGuid(), Nombre = "c411", Modelo = "QASWED", Descripcion = "Dispositivo 2", Fotografias = fotografias, EmpresaId = empresa.Id }
        };

        var obtenerDispositivos = new ObtenerDispositivosArgs(dispositivosEsperados, 1);

        _logicaDispositivoMock
            .Setup(i => i.ObtenerTodos(It.IsAny<ParametroPaginacion>(),
                It.IsAny<ParametroDispositivoFiltro>()))
            .Returns(obtenerDispositivos);

        var respuesta = _servicio.ObtenerTodos(It.IsAny<ParametroPaginacion>(),
            It.IsAny<ParametroDispositivoFiltro>());

        respuesta.Dispositivos.Should().NotBeNullOrEmpty();
        respuesta.Dispositivos.Should().HaveCount(dispositivosEsperados.Count);
        respuesta.Dispositivos.Should().BeEquivalentTo(dispositivosEsperados);
        respuesta.CantidadPaginas.Should().Be(1);
    }

    [TestMethod]
    public void ObtenerPorIdDevuelveSensorCorrecto()
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
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var sensorId = Guid.NewGuid();
        var sensor = new Dispositivo
        {
            Id = sensorId,
            Tipo = TipoDispositivo.SensorVentana,
            Nombre = "sensor",
            Modelo = "ASDFGH",
            Descripcion = "sensor1",
            Fotografias = fotografias,
            EmpresaId = empresa.Id
        };

        _logicaDispositivoMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Dispositivo, bool>>>(e => e.Compile()(new Dispositivo
            {
                Id = sensor.Id,
            }))))
            .Returns(true);

        _logicaDispositivoMock
            .Setup(i => i.ObtenerPorId(It.Is<Guid>(g => g == sensor.Id))).Returns(sensor);

        var resultado = _servicio.ObtenerPorId(sensorId.ToString());

        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(sensorId);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerPorIdLanzaExcepcionSiDispositivoNoExiste()
    {
        var guid = Guid.NewGuid();

        _logicaDispositivoMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Dispositivo, bool>>>(e => e.Compile()(new Dispositivo
            {
                Id = guid
            }))))
            .Returns(false);

        _servicio.ObtenerPorId(guid.ToString());
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ObtenerPorIdLanzaExcepcionSiIdNoEsGuid()
    {
        var obtenerDispositivos = new ObtenerDispositivosArgs([], 0);

        _logicaDispositivoMock
            .Setup(i => i.ObtenerTodos(It.IsAny<ParametroPaginacion>(),
                It.IsAny<ParametroDispositivoFiltro>()))
            .Returns(obtenerDispositivos);

        _servicio.ObtenerPorId("no guid");
    }

    [TestMethod]
    public void ObtenerTiposDeDispositivosExito()
    {
        var tiposEsperados = new List<TipoDispositivo>
        {
            TipoDispositivo.Camara,
            TipoDispositivo.SensorVentana,
            TipoDispositivo.SensorMovimiento,
            TipoDispositivo.Lampara
        };

        _logicaDispositivoMock
            .Setup(i => i.ObtenerTiposDeDispositivos())
            .Returns(Enum.GetValues(typeof(TipoDispositivo)).Cast<TipoDispositivo>().ToList());

        var respuesta = _servicio.ObtenerTiposDeDispositivos();

        respuesta.Should().NotBeNullOrEmpty();
        respuesta.Should().HaveCount(tiposEsperados.Count);
        respuesta.Should().BeEquivalentTo(tiposEsperados);
    }

    [TestMethod]
    [DataRow(null)]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ImportarLanzaExcepcionSiElEmpresaEsNula(Empresa empresa)
    {
        _servicio.ImportarDispositivos("ruta", "ImportadorJsonAulas", empresa);
    }

    [TestMethod]
    [DataRow("IdentificadorInvalido")]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ImportarLanzaExcepcionSiElImportadorNoExiste(string identificadorImportador)
    {
        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        _servicio.ImportarDispositivos("ruta", identificadorImportador, empresa);
    }

    [TestMethod]
    public void ImportarDispositivosLLamaAImportarYAgregarDispositivo()
    {
        var empresa = new Empresa { Id = Guid.NewGuid() };

        _logicaDispositivoMock
            .Setup(i => i.Agregar(It.IsAny<Dispositivo>()));

        var directorioActual = AppDomain.CurrentDomain.BaseDirectory;

        var directorioJson = Path.GetFullPath(Path.Combine(directorioActual, "../../../JsonTest"));

        var rutaJson = Path.GetFullPath(Path.Combine(directorioJson, "json-prueba.json"));

        _servicio.ImportarDispositivos(rutaJson, "ImportadorJsonAulas", empresa);

        _logicaDispositivoMock.Verify(i => i.Agregar(It.IsAny<Dispositivo>()), Times.Once);
    }

    [TestMethod]
    public void ObteneridentificadoresDeImportadoresRetornaLasImplementacionesExistentes()
    {
        var resultado = _servicio.ObtenerIdentificadoresDeImportadores();

        resultado.Should().Contain("ImportadorJsonAulas");
    }
    #endregion
}
#endregion
