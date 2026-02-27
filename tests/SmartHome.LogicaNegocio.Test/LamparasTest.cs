using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public sealed class LamparasTest
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

    #region Error
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearLamparaConNombreNullOVacioLanzaExcepcion(string nombre)
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

        var args = new CrearLamparasArgs(
            nombre,
            "ASDFGH",
            "Lampara inteligente",
            fotografias,
            empresa);

        _servicio.AgregarLampara(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearLamparaConModeloNullOVacioLanzaExcepcion(string modelo)
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

        var args = new CrearLamparasArgs(
            "c410",
            modelo,
            "Lampara inteligente",
            fotografias,
            empresa);

        _servicio.AgregarLampara(args);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearLamparaConDescripcionNullOVacioLanzaExcepcion(string descripcion)
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

        var args = new CrearLamparasArgs(
            "c410",
            "ASDFGH",
            descripcion,
            fotografias,
            empresa);

        _servicio.AgregarLampara(args);
    }

    [TestMethod]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearLamparaConFotografiasNullLanzaExcepcion(List<FotografiaDispositivo> fotografias)
    {
        var empresa = new Empresa
        {
            Id = Guid.NewGuid(),
            Nombre = "Vidly",
            Logotipo = "/downloads/vidly",
            Rut = "12345678-9",
            Validador = "Reflection.ValidadorAulas6Letras"
        };

        var args = new CrearLamparasArgs(
            "c410",
            "ASDFGH",
            "Lampara inteligente",
            fotografias,
            empresa);

        _servicio.AgregarLampara(args);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearLamparaSinFotografiaPrincipalLanzaExcepcion()
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

        var args = new CrearLamparasArgs(
            "c410",
            "ASDFGH",
            "Lampara inteligente",
            fotografias,
            empresa);

        _servicio.AgregarLampara(args);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearLamparaConMasDe1FotografiaPrincipalLanzaExcepcion()
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

        var args = new CrearLamparasArgs(
            "c410",
            "ASDFGH",
            "Lampara inteligente",
            fotografias,
            empresa);

        _servicio.AgregarLampara(args);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearLamparaConEmpresaNullLanzaExcepcion()
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

        var args = new CrearLamparasArgs(
            "c410",
            "ASDFGH",
            "Lampara inteligente",
            fotografias,
            null!);

        _servicio.AgregarLampara(args);
    }

    [TestMethod]
    public void CrearLamparaQueEmpresaYaRegistroLanzaExcepcion()
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

        var args = new CrearLamparasArgs(
            "c410",
            "ASDFGH",
            "Lampara inteligente",
            [fotografia],
            empresa);

        var dispositivo = new Dispositivo
        {
            Id = Guid.NewGuid(),
            Nombre = args.Nombre,
            Modelo = args.Modelo,
            Descripcion = args.Descripcion,
            Fotografias = args.Fotografias,
            EmpresaId = args.Empresa.Id
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

        var accion = () => _servicio.AgregarLampara(args);

        accion.Should().Throw<ArgumentException>().WithMessage("La empresa ya registró un dispositivo con el mismo nombre y número de modelo.");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearLamparaConModeloDiferenteAlValidadorLanzaExcepcion()
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

        var args = new CrearLamparasArgs(
            "c410",
            "ASDFGHA23",
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

        _servicio.AgregarLampara(args);
    }
    #endregion

    #region Exito
    [TestMethod]
    public void CrearLamparaExito()
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

        var args = new CrearLamparasArgs(
            "c410",
            "ASDFGH",
            "Lampara",
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

        var respuesta = _servicio.AgregarLampara(args);
        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBeEmpty();
        respuesta.Id.Should().NotBe(Guid.Empty);

        respuesta.Tipo.Should().Be(TipoDispositivo.Lampara);
        respuesta.Nombre.Should().Be(args.Nombre);
        respuesta.Modelo.Should().Be(args.Modelo);
        respuesta.Descripcion.Should().Be(args.Descripcion);
        respuesta.Fotografias.Should().Equal(args.Fotografias);
        respuesta.EmpresaId.Should().Be(args.Empresa.Id);
    }
    #endregion
}
