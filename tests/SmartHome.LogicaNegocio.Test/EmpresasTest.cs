using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public sealed class EmpresasTest
{
    private Mock<IEmpresaRepositorio> _logicaEmpresaMock = null!;
    private Mock<IUsuarioRepositorio> _logicaUsuarioMock = null!;
    private EmpresaLogica _servicio = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaEmpresaMock = new Mock<IEmpresaRepositorio>(MockBehavior.Strict);
        _logicaUsuarioMock = new Mock<IUsuarioRepositorio>(MockBehavior.Strict);
        _servicio = new EmpresaLogica(_logicaEmpresaMock.Object, _logicaUsuarioMock.Object);
    }

    #region Create
    #region Error
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearEmpresaConNombreNullOVacioLanzaExcepcion(string nombre)
    {
        new CrearEmpresasArgs(
            nombre,
            "/downloads/ORT",
            "12345670",
            "Reflection.ValidadorAulas6Letras");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearEmpresaConLogotipoNullOVacioLanzaExcepcion(string logotipo)
    {
        new CrearEmpresasArgs(
            "ORT",
            logotipo,
            "12345670",
            "Reflection.ValidadorAulas6Letras");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearEmpresaConRutNullOVacioLanzaExcepcion(string rut)
    {
        new CrearEmpresasArgs(
            "ORT",
            "/downloads/ORT",
            rut,
            "Reflection.ValidadorAulas6Letras");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearEmpresaConValidadorNullOVacioLanzaExcepcion(string validador)
    {
        new CrearEmpresasArgs(
            "ORT",
            "/downloads/ORT",
            "038CFP928",
            validador);
    }

    [TestMethod]
    public void CrearEmpresaConNombreYaExistenteLanzaExepcion()
    {
        var args = new CrearEmpresasArgs(
            "ORT",
            "/downloads/ORT",
            "DFK46FYGI",
            "Reflection.ValidadorAulas6Letras");

        _logicaEmpresaMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Empresa, bool>>>(e => e.Compile()(new Empresa { Nombre = "ORT" }))))
            .Returns(true);

        _logicaEmpresaMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Empresa, bool>>>(e => e.Compile()(new Empresa { Rut = "DFK46FYGI" }))))
            .Returns(false);

        var accion = () => _servicio.Agregar(args, new Usuario());

        accion.Should().Throw<ArgumentException>().WithMessage("El nombre de la empresa ya está en uso. Debe ser único.");
    }

    [TestMethod]
    public void CrearEmpresaConRutYaExistenteLanzaExepcion()
    {
        var args = new CrearEmpresasArgs(
            "ORT",
            "/downloads/ORT",
            "DFK46FYGI",
            "Reflection.ValidadorAulas6Letras");

        _logicaEmpresaMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Empresa, bool>>>(e => e.Compile()(new Empresa { Nombre = "ORT" }))))
            .Returns(false);

        _logicaEmpresaMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Empresa, bool>>>(e => e.Compile()(new Empresa { Rut = "DFK46FYGI" }))))
            .Returns(true);

        var accion = () => _servicio.Agregar(args, new Usuario());

        accion.Should().Throw<ArgumentException>().WithMessage("El RUT de la empresa ya está registrado. Debe ser único.");
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearSiYaTieneEmpresaLanzaExcepcion()
    {
        var args = new CrearEmpresasArgs(
            "ORT",
            "/downloads/ORT",
            "125347",
            "Reflection.ValidadorAulas6Letras");

        _logicaEmpresaMock
            .Setup(i => i.Agregar(It.Is<Empresa>(e =>
                e.Id != Guid.Empty &&
                e.Nombre == args.Nombre &&
                e.Logotipo == args.Logotipo &&
                e.Rut == args.Rut)));

        var obtenerEmpresas = new ObtenerEmpresasArgs([], 0);

        _logicaEmpresaMock
            .Setup(i => i.ObtenerTodos(It.IsAny<ParametroPaginacion>(),
                It.IsAny<ParametroEmpresaFiltro>()))
            .Returns(obtenerEmpresas);

        _logicaUsuarioMock
            .Setup(i => i.Actualizar(It.IsAny<Usuario>()));

        var usuario = new Usuario
        {
            Nombre = "pepe",
            Apellido = "perez",
            Empresa = new Empresa()
        };

        var respuesta = _servicio.Agregar(args, usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(FileNotFoundException))]
    public void CrearEmpresaConValidadorNoExistenteLanzaExcepcion()
    {
        new CrearEmpresasArgs(
            "ORT",
            "/downloads/ORT",
            "038CFP928",
            "validadorNoExistente");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CrearEmpresaExito()
    {
        var args = new CrearEmpresasArgs(
            "ORT",
            "/downloads/ORT",
            "125347",
            "Reflection.ValidadorAulas6Letras");

        _logicaEmpresaMock
            .Setup(i => i.Agregar(It.Is<Empresa>(e =>
                e.Id != Guid.Empty &&
                e.Nombre == args.Nombre &&
                e.Logotipo == args.Logotipo &&
                e.Rut == args.Rut)));

        _logicaEmpresaMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Empresa, bool>>>(e => e.Compile()(new Empresa { Nombre = "ORT" }))))
            .Returns(false);

        _logicaEmpresaMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Empresa, bool>>>(e => e.Compile()(new Empresa { Rut = "125347" }))))
            .Returns(false);

        var usuario = new Usuario
        {
            Nombre = "pepe",
            Apellido = "perez"
        };

        _logicaUsuarioMock
            .Setup(i => i.Actualizar(It.Is<Usuario>(u => u == usuario)));

        var respuesta = _servicio.Agregar(args, usuario);

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBeEmpty();
        respuesta.Id.Should().NotBe(Guid.Empty);

        respuesta.Nombre.Should().Be(args.Nombre);
        respuesta.Logotipo.Should().Be(args.Logotipo);
        respuesta.Rut.Should().Be(args.Rut);
        respuesta.NombreCreador.Should().Be("pepe perez");
        respuesta.Validador.Should().Be(args.Validador);
    }

    [TestMethod]
    public void GuardarCambiosExito()
    {
        _logicaEmpresaMock.Setup(i => i.GuardarCambios());

        _servicio.GuardarCambios();
    }

    [TestMethod]
    public void ObtenerTodosExito()
    {
        var empresa1 = new Empresa
        {
            Nombre = "ORT",
            Logotipo = "/img/ort.png",
            Rut = "038CFP928"
        };

        var empresa2 = new Empresa
        {
            Nombre = "UTEC",
            Logotipo = "/img/utec.png",
            Rut = "038CFP928"
        };

        var empresas = new List<Empresa> { empresa1, empresa2 };

        var obtenerEmpresas = new ObtenerEmpresasArgs(empresas, 1);

        var paginacion = new ParametroPaginacion(1, 2);

        _logicaEmpresaMock.Setup(i => i.ObtenerTodos(It.Is<ParametroPaginacion>(p => p == paginacion),
            It.IsAny<ParametroEmpresaFiltro>())).Returns(obtenerEmpresas);

        var respuesta = _servicio.ObtenerTodos(paginacion,
            It.IsAny<ParametroEmpresaFiltro>());

        respuesta.Should().NotBeNull();
        respuesta.Empresas.Should().NotBeEmpty();
        respuesta.Empresas.Should().HaveCount(2);

        respuesta.Empresas.Should().Contain(empresa1);
        respuesta.Empresas.Should().Contain(empresa2);
        respuesta.Empresas.Should().BeEquivalentTo(empresas);

        respuesta.CantidadPaginas.Should().Be(1);
    }

    [TestMethod]
    public void ObtenerPorIdExito()
    {
        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "ORT",
            Logotipo = "/img/ort.png",
            Rut = "038CFP928"
        };

        _logicaEmpresaMock.Setup(i => i.ObtenerPorId(empresa.Id)).Returns(empresa);

        var respuesta = _servicio.ObtenerPorId(empresa.Id);

        respuesta.Should().NotBeNull();
        respuesta.Should().Be(empresa);
    }

    [TestMethod]
    public void ObteneridentificadoresDeImplementacionesRetornaLasImplementacionesExistentes()
    {
        var resultado = _servicio.ObtenerIdentificadoresDeImplementaciones();

        resultado.Should().Contain("Reflection.ValidadorAulas6Letras");
        resultado.Should().Contain("Reflection.ValidadorAulas3Letras3Numeros");
    }
    #endregion
    #endregion
}
