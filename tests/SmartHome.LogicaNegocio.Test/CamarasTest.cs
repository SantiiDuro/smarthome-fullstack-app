using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public sealed class CamarasTest
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

        var args = new CrearCamarasArgs(
            nombre,
            "ASDFGH",
            "Dispositivo para videovigilancia",
            fotografias,
            empresa,
            true,
            true,
            true,
            true);

        _servicio.AgregarCamara(args);
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

        var args = new CrearCamarasArgs(
            "c410",
            modelo,
            "Dispositivo para videovigilancia",
            fotografias,
            empresa,
            true,
            true,
            true,
            true);

        _servicio.AgregarCamara(args);
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

        var args = new CrearCamarasArgs(
            "c410",
            "ASDFGH",
            descripcion,
            fotografias,
            empresa,
            true,
            true,
            true,
            true);

        _servicio.AgregarCamara(args);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearCamaraSinFotografiaPrincipalLanzaExcepcion()
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

        var args = new CrearCamarasArgs(
            "c410",
            "ASDFGH",
            "Dispositivo para videovigilancia",
            fotografias,
            empresa,
            true,
            true,
            true,
            true);

        _servicio.AgregarCamara(args);
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

        var args = new CrearCamarasArgs(
            "c410",
            "ASDFGH",
            "Dispositivo para videovigilancia",
            fotografias,
            empresa,
            true,
            false,
            false,
            false);

        _servicio.AgregarCamara(args);
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

        var args = new CrearCamarasArgs(
            "c410",
            "ASDFGH",
            "Dispositivo para videovigilancia",
            fotografias,
            null!,
            true,
            false,
            false,
            true);

        _servicio.AgregarCamara(args);
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

        var args = new CrearCamarasArgs(
            "c410",
            "ASDFGH",
            "Dispositivo para videovigilancia",
            [fotografia],
            empresa,
            true,
            false,
            false,
            true);

        var dispositivo = new Dispositivo
        {
            Id = Guid.NewGuid(),
            Tipo = TipoDispositivo.Camara,
            Nombre = "c410",
            Modelo = "ASDFGH",
            Descripcion = "Dispositivo 1",
            Fotografias = [fotografia],
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

        var accion = () => _servicio.AgregarCamara(args);

        accion.Should().Throw<ArgumentException>().WithMessage("La empresa ya registró un dispositivo con el mismo nombre y número de modelo.");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearCamaraConModeloDiferenteAlValidadorLanzaExcepcion()
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

        var args = new CrearCamarasArgs(
            "c410",
            "ASX",
            "Dispositivo para videovigilancia",
            fotografias,
            empresa,
            true,
            true,
            true,
            true);

        _logicaDispositivoMock
            .Setup(i => i.Existe(It.Is<Expression<Func<Dispositivo, bool>>>(e => e.Compile()(new Dispositivo
            {
                EmpresaId = empresa.Id,
                Nombre = args.Nombre,
                Modelo = args.Modelo
            }))))
            .Returns(false);

        _logicaEmpresaMock.Setup(lh => lh.ObtenerPorId(empresa.Id)).Returns(empresa);

        _servicio.AgregarCamara(args);
    }
    #endregion
    #region Success
    [TestMethod]
    public void CrearCamaraExito()
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

        var args = new CrearCamarasArgs(
            "c410",
            "ASDFGH",
            "Dispositivo para videovigilancia",
            fotografias,
            empresa,
            true,
            false,
            false,
            true);

        _logicaDispositivoMock
            .Setup(i => i.Agregar(It.Is<Dispositivo>(d =>
                d.Tipo == TipoDispositivo.Camara &&
                d.Nombre == "c410" &&
                d.Modelo == "ASDFGH" &&
                d.Descripcion == "Dispositivo para videovigilancia" &&
                d.Fotografias == fotografias &&
                d.EmpresaId == empresa.Id &&
                d.DetectaMovimiento == true &&
                d.DetectaPersona == false &&
                d.UsoExterior == false &&
                d.UsoInterior == true)));

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

        var respuesta = _servicio.AgregarCamara(args);
        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBeEmpty();
        respuesta.Id.Should().NotBe(Guid.Empty);

        respuesta.Tipo.Should().Be(TipoDispositivo.Camara);
        respuesta.Nombre.Should().Be(args.Nombre);
        respuesta.Modelo.Should().Be(args.Modelo);
        respuesta.Descripcion.Should().Be(args.Descripcion);
        respuesta.Fotografias.Should().Equal(args.Fotografias);
        respuesta.EmpresaId.Should().Be(args.Empresa.Id);
        respuesta.DetectaMovimiento.Should().Be(args.DetectaMovimiento);
        respuesta.DetectaPersona.Should().Be(args.DetectaPersona);
        respuesta.UsoExterior.Should().Be(args.UsoExterior);
        respuesta.UsoInterior.Should().Be(args.UsoInterior);
    }
    #endregion
}
#endregion
