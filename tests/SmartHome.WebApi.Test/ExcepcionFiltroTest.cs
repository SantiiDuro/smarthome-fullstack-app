using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ExcepcionFiltroTest
{
    private ExceptionContext _contexto = null!;
    private readonly ExcepcionFiltro _atributo;

    public ExcepcionFiltroTest()
    {
        _atributo = new ExcepcionFiltro();
    }

    [TestInitialize]
    public void Initialize()
    {
        _contexto = new ExceptionContext(
            new ActionContext(
                new Mock<HttpContext>().Object,
                new RouteData(),
                new ActionDescriptor()),
            []);
    }

    [TestMethod]
    public void ExcepcionNoRegistradaRespondeErrorInterno()
    {
        _contexto.Exception = new Exception("Excepcion no registrada");

        _atributo.OnException(_contexto);

        var respuesta = _contexto.Result;

        respuesta.Should().NotBeNull();

        var respuestaConcreta = respuesta as ObjectResult;

        respuestaConcreta.Should().NotBeNull();
        respuestaConcreta.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        respuestaConcreta.Value.GetType().GetProperty("CodigoInterno").GetValue(respuestaConcreta.Value).ToString().Should().Be("ErrorInterno");
        respuestaConcreta.Value.GetType().GetProperty("Mensaje").GetValue(respuestaConcreta.Value).ToString().Should().Be("Error al procesar la solicitud");
    }

    [TestMethod]
    public void ArgumentNullExceptionRespondeBadRequest()
    {
        _contexto.Exception = new ArgumentNullException("Mensaje de prueba");

        _atributo.OnException(_contexto);

        var respuesta = _contexto.Result as ObjectResult;

        respuesta.Should().NotBeNull();
        respuesta.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        respuesta.Value.GetType().GetProperty("CodigoInterno").GetValue(respuesta.Value).ToString().Should().Be("ArgumentoInvalido");
        respuesta.Value.GetType().GetProperty("Mensaje").GetValue(respuesta.Value).ToString().Should().Be("El argumento no puede ser nulo ni vacio");
        respuesta.Value.GetType().GetProperty("Argumento").GetValue(respuesta.Value).ToString().Should().Be("Mensaje de prueba");
    }

    [TestMethod]
    public void ArgumentExceptionRespondeBadRequest()
    {
        _contexto.Exception = new ArgumentException("Mensaje de prueba");

        _atributo.OnException(_contexto);

        var respuesta = _contexto.Result as ObjectResult;

        respuesta.Should().NotBeNull();
        respuesta.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        respuesta.Value.GetType().GetProperty("CodigoInterno").GetValue(respuesta.Value).ToString().Should().Be("ArgumentoInvalido");
        respuesta.Value.GetType().GetProperty("Mensaje").GetValue(respuesta.Value).ToString().Should().Be("Mensaje de prueba");
    }

    [TestMethod]
    public void InvalidOperationRespondeBadRequest()
    {
        _contexto.Exception = new InvalidOperationException("Mensaje de prueba");

        _atributo.OnException(_contexto);

        var respuesta = _contexto.Result as ObjectResult;

        respuesta.Should().NotBeNull();
        respuesta.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        respuesta.Value.GetType().GetProperty("CodigoInterno").GetValue(respuesta.Value).ToString().Should().Be("OperacionInvalida");
        respuesta.Value.GetType().GetProperty("Mensaje").GetValue(respuesta.Value).ToString().Should().Be("Mensaje de prueba");
    }

    [TestMethod]
    public void FileNotFoundRespondeBadRequest()
    {
        _contexto.Exception = new FileNotFoundException("Mensaje de prueba");

        _atributo.OnException(_contexto);

        var respuesta = _contexto.Result as ObjectResult;

        respuesta.Should().NotBeNull();
        respuesta.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        respuesta.Value.GetType().GetProperty("CodigoInterno").GetValue(respuesta.Value).ToString().Should().Be("OperacionInvalida");
        respuesta.Value.GetType().GetProperty("Mensaje").GetValue(respuesta.Value).ToString().Should().Be("Mensaje de prueba");
    }

    [TestMethod]
    public void FormatExceptionRespondeBadRequest()
    {
        _contexto.Exception = new FormatException("Mensaje de prueba");

        _atributo.OnException(_contexto);

        var respuesta = _contexto.Result as ObjectResult;

        respuesta.Should().NotBeNull();
        respuesta.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        respuesta.Value.GetType().GetProperty("CodigoInterno").GetValue(respuesta.Value).ToString().Should().Be("FormatoInvalido");
        respuesta.Value.GetType().GetProperty("Mensaje").GetValue(respuesta.Value).ToString().Should().Be("Mensaje de prueba");
    }

    [TestMethod]
    public void KeyNotFoundExceptionRespondeNotFound()
    {
        _contexto.Exception = new KeyNotFoundException("Mensaje de prueba");

        _atributo.OnException(_contexto);

        var respuesta = _contexto.Result as ObjectResult;

        respuesta.Should().NotBeNull();
        respuesta.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        respuesta.Value.GetType().GetProperty("CodigoInterno").GetValue(respuesta.Value).ToString().Should().Be("ElementoNoEncontrado");
        respuesta.Value.GetType().GetProperty("Mensaje").GetValue(respuesta.Value).ToString().Should().Be("Mensaje de prueba");
    }
}
