using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.WebApi.Filtros;

public class AutorizacionFiltro(string? permiso = null!)
    : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext contexto)
    {
        if (contexto.Result is not null)
        {
            return;
        }

        var usuarioLoggeado = contexto.HttpContext.Items[Items.UsuarioLoggeado];

        var usuarioNoIdentificado = usuarioLoggeado == null;

        if (usuarioNoIdentificado)
        {
            contexto.Result = new ObjectResult(new
            {
                CodigoInterno = "NoAutorizado",
                Message = $"No autenticado"
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var usuarioLoggeadoMapeado = (Usuario)usuarioLoggeado;

        var permiso = BuildPermission(contexto);

        var hasNotPermission = !usuarioLoggeadoMapeado.Rol.TienePermiso(permiso);

        if (hasNotPermission)
        {
            contexto.Result = new ObjectResult(new
            {
                CodigoInterno = "Prohibido",
                Message = $"Falta permiso {permiso}"
            })
            {
                StatusCode = (int)HttpStatusCode.Forbidden
            };
        }
    }

    private string BuildPermission(AuthorizationFilterContext context)
    {
        return permiso ?? $"{context.RouteData.Values["action"].ToString().ToLower()}-{context.RouteData.Values["controller"].ToString().ToLower()}";
    }
}
