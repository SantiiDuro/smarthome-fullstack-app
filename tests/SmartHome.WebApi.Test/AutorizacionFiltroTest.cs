using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Test;

[TestClass]
public class AutorizacionFiltroTests
{
    private Mock<HttpContext> _httpContextMock = null!;
    private AuthorizationFilterContext _contexto = null!;
    private AutorizacionFiltro _atributo;

    public AutorizacionFiltroTests()
    {
        _atributo = new AutorizacionFiltro();
    }

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);

        _contexto = new AuthorizationFilterContext(
            new ActionContext
            {
                HttpContext = _httpContextMock.Object,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new ActionDescriptor()
            },
            []);
    }

    #region Error Handling Tests

    [TestMethod]
    public void OnAuthorizationUsuarioSinLoguearRetornaNoAutorizado()
    {
        var items = new Dictionary<object, object>
            {
                { Items.UsuarioLoggeado, null }
            };

        _httpContextMock.Setup(h => h.Items).Returns(items);

        _atributo.OnAuthorization(_contexto);

        var respuesta = _contexto.Result;

        respuesta.Should().NotBeNull();
        var respuestaConcreta = respuesta as ObjectResult;
        respuestaConcreta.Should().NotBeNull();
        respuestaConcreta.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        respuestaConcreta.Value.GetType().GetProperty("CodigoInterno").GetValue(respuestaConcreta.Value).ToString().Should().Be("NoAutorizado");
        respuestaConcreta.Value.GetType().GetProperty("Message").GetValue(respuestaConcreta.Value).ToString().Should().Be("No autenticado");
    }

    [TestMethod]
    public void OnAuthorizationUsuarioSinPermisoRetornaProhibido()
    {
        var usuarioLoggeado = new Usuario
        {
            Rol = new Rol()
        };

        var items = new Dictionary<object, object>
    {
        { Items.UsuarioLoggeado, usuarioLoggeado }
    };

        _httpContextMock.Setup(h => h.Items).Returns(items);
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary());

        var permisoRequerido = "permiso";
        _atributo = new AutorizacionFiltro(permisoRequerido);

        _atributo.OnAuthorization(_contexto);

        var respuesta = _contexto.Result;

        respuesta.Should().NotBeNull();
        var respuestaConcreta = respuesta as ObjectResult;
        respuestaConcreta.Should().NotBeNull();
        respuestaConcreta.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        respuestaConcreta.Value.GetType().GetProperty("CodigoInterno").GetValue(respuestaConcreta.Value).ToString().Should().Be("Prohibido");
        respuestaConcreta.Value.GetType().GetProperty("Message").GetValue(respuestaConcreta.Value).ToString().Should().Be($"Falta permiso {permisoRequerido}");
    }

    [TestMethod]
    public void OnAuthorizationSiResultEsNullNoLoModifica()
    {
        var result = new ObjectResult(new { Message = "Already set" })
        {
            StatusCode = (int)HttpStatusCode.OK
        };

        _contexto.Result = result;

        _atributo.OnAuthorization(_contexto);

        _contexto.Result.Should().Be(result);
    }
    #endregion

    #region Success Tests

    [TestMethod]
    public void OnAuthorizationUsuarioConPermisoPasa()
    {
        var usuarioLoggeado = new Usuario
        {
            Rol = new Rol
            {
                Tipo = "dueño hogar",
                Permisos = [PermisoUsuario.CrearHogar]
            }
        };

        var items = new Dictionary<object, object>
    {
        { Items.UsuarioLoggeado, usuarioLoggeado }
    };

        _httpContextMock.Setup(h => h.Items).Returns(items);
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary());

        var permisoRequerido = "CrearHogar";
        _atributo = new AutorizacionFiltro(permisoRequerido);

        _atributo.OnAuthorization(_contexto);

        _contexto.Result.Should().BeNull();
    }

    #endregion
}
