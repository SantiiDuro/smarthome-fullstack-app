using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.WebApi.Filtros;

public class AutenticacionFiltro
    : Attribute, IAuthorizationFilter
{
    private const string AUTHORIZATION_HEADER = "Authorization";

    public virtual void OnAuthorization(AuthorizationFilterContext contexto)
    {
        var authorizationHeader = contexto.HttpContext.Request.Headers[AUTHORIZATION_HEADER];

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            contexto.Result = new ObjectResult(new
            {
                CodigoInterno = "NoAutenticado",
                Mensaje = "No te encuentras autenticado"
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var formatoAutorizationInvalido = !FormatoValidoAutorizacion(authorizationHeader!);
        if (formatoAutorizationInvalido)
        {
            contexto.Result = new ObjectResult(
                new
                {
                    CodigoInterno = "AutorizacionInvalida",
                    Mensaje = "El token de autorizacion es invalido"
                })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var autorizacionExpirada = AutorizacionExpirada(authorizationHeader!, contexto);
        if (autorizacionExpirada)
        {
            contexto.Result = new ObjectResult(
                new
                {
                    CodigoInterno = "AutorizacionInvalida",
                    Mensaje = "El token de autorizacion esta expirado"
                })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        try
        {
            var userOfAuthorization = ObtenerUsuarioDeAutorizacion(authorizationHeader!, contexto);

            contexto.HttpContext.Items[Items.UsuarioLoggeado] = userOfAuthorization;
        }
        catch (Exception)
        {
            contexto.Result = new ObjectResult(new
            {
                CodigoInterno = "ErrorInterno",
                Mensaje = "Error al procesar la solicitud"
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    private bool FormatoValidoAutorizacion(string authorization)
    {
        return Guid.TryParse(authorization, out _);
    }

    private bool AutorizacionExpirada(string authorization, AuthorizationFilterContext contexto)
    {
        var sesionLogica = contexto.HttpContext.RequestServices.GetRequiredService<ISesionLogica>();

        var sesionActiva = sesionLogica.SesionActiva(authorization);

        return sesionActiva is false;
    }

    private Usuario ObtenerUsuarioDeAutorizacion(string authorization, AuthorizationFilterContext contexto)
    {
        var sesionLogica = contexto.HttpContext.RequestServices.GetRequiredService<ISesionLogica>();

        var usuario = sesionLogica.ObtenerUsuarioPorToken(authorization);

        return usuario;
    }
}
