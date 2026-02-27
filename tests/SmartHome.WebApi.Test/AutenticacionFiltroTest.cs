using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Test;

[TestClass]
public class AutenticacionFiltroTest
{
    private Mock<HttpContext> _httpContextMock = null!;
    private AuthorizationFilterContext _contexto = null!;
    private readonly AutenticacionFiltro _atributo;

    public AutenticacionFiltroTest()
    {
        _atributo = new AutenticacionFiltro();
    }

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);

        _contexto = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()),
            []);
    }

    #region Error
    [TestMethod]
    public void OnAuthorizationConHeaderVacioDeberiaRetornarNoAutenticado()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary());

        _atributo.OnAuthorization(_contexto);

        var response = _contexto.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        concreteResponse.Value.GetType().GetProperty("CodigoInterno").GetValue(concreteResponse.Value).ToString().Should().Be("NoAutenticado");
        concreteResponse.Value.GetType().GetProperty("Mensaje").GetValue(concreteResponse.Value).ToString().Should().Be("No te encuentras autenticado");
    }

    [TestMethod]
    public void OnAuthorizationConAutorizacionVaciaDeberiaRetornarNoAutenticado()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "Authorization", string.Empty }
            }));

        _atributo.OnAuthorization(_contexto);

        var response = _contexto.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        concreteResponse.Value.GetType().GetProperty("CodigoInterno").GetValue(concreteResponse.Value).ToString().Should().Be("NoAutenticado");
        concreteResponse.Value.GetType().GetProperty("Mensaje").GetValue(concreteResponse.Value).ToString().Should().Be("No te encuentras autenticado");
    }

    [TestMethod]
    public void OnAuthorizationConFormatoAutorizacionInvalidoDeberiaRetornarAutorizacionInvalida()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "Authorization", "invalid_format" }
            }));

        _atributo.OnAuthorization(_contexto);

        var response = _contexto.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        concreteResponse.Value.GetType().GetProperty("CodigoInterno").GetValue(concreteResponse.Value).ToString().Should().Be("AutorizacionInvalida");
        concreteResponse.Value.GetType().GetProperty("Mensaje").GetValue(concreteResponse.Value).ToString().Should().Be("El token de autorizacion es invalido");
    }

    [TestMethod]
    public void OnAuthorizationConAutorizacionExpiradaDeberiaRetornarAutorizacionInvalida()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "Authorization", "3F2504E0-4F89-11D3-9A0C-0305E82C3301" }
            }));

        var sesionLogicaMock = new Mock<ISesionLogica>();
        sesionLogicaMock.Setup(s => s.SesionActiva("3F2504E0-4F89-11D3-9A0C-0305E82C3301")).Returns(false);

        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(ISesionLogica))).Returns(sesionLogicaMock.Object);

        _atributo.OnAuthorization(_contexto);

        var response = _contexto.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        concreteResponse.Value.GetType().GetProperty("CodigoInterno").GetValue(concreteResponse.Value).ToString().Should().Be("AutorizacionInvalida");
        concreteResponse.Value.GetType().GetProperty("Mensaje").GetValue(concreteResponse.Value).ToString().Should().Be("El token de autorizacion esta expirado");
    }

    [TestMethod]
    public void OnAuthorization_WhenAuthorizationIsValid_ShouldStoreUserInHttpContext()
    {
        var validGuid = Guid.NewGuid().ToString();

        _httpContextMock.Setup(h => h.Items).Returns(new Dictionary<object, object>());

        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
        {
            { "Authorization", validGuid }
        }));

        var sesionLogicaMock = new Mock<ISesionLogica>();
        var usuarioMock = new Usuario();

        sesionLogicaMock.Setup(s => s.SesionActiva(validGuid)).Returns(true);
        sesionLogicaMock.Setup(s => s.ObtenerUsuarioPorToken(validGuid)).Returns(usuarioMock);

        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(ISesionLogica))).Returns(sesionLogicaMock.Object);

        _atributo.OnAuthorization(_contexto);

        var usuarioAlmacenado = _contexto.HttpContext.Items[Items.UsuarioLoggeado];
        usuarioAlmacenado.Should().Be(usuarioMock);
    }

    [TestMethod]
    public void OnAuthorization_WhenObtenerUsuarioDeAutorizacionThrowsException_ShouldReturnInternalServerError()
    {
        var validGuid = Guid.NewGuid().ToString();

        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "Authorization", validGuid }
            }));

        var sesionLogicaMock = new Mock<ISesionLogica>();

        sesionLogicaMock.Setup(s => s.SesionActiva(validGuid)).Returns(true);
        sesionLogicaMock.Setup(s => s.ObtenerUsuarioPorToken(validGuid)).Throws(new Exception("Error de prueba"));

        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(ISesionLogica))).Returns(sesionLogicaMock.Object);

        _atributo.OnAuthorization(_contexto);

        var response = _contexto.Result as ObjectResult;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        response.Value.Should().NotBeNull();

        var codigoInterno = response.Value.GetType().GetProperty("CodigoInterno")?.GetValue(response.Value);
        var mensaje = response.Value.GetType().GetProperty("Mensaje")?.GetValue(response.Value);

        codigoInterno.Should().Be("ErrorInterno");
        mensaje.Should().Be("Error al procesar la solicitud");
    }

    #endregion
}
