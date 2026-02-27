using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartHome.WebApi.Filtros;

public sealed class ExcepcionFiltro
    : IExceptionFilter
{
    private static readonly Dictionary<Type, Func<Exception, ObjectResult>> _errores = new()
    {
        {
            typeof(ArgumentNullException),
        (Exception excepcion) =>
        {
            var excepcionConcreta = (ArgumentNullException)excepcion;
        return new ObjectResult(new
            {
                CodigoInterno = "ArgumentoInvalido",
                Mensaje = "El argumento no puede ser nulo ni vacio",
                Argumento = excepcionConcreta.ParamName
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
        };
            }
        },
        {
            typeof(ArgumentException),
        (Exception excepcion) =>
        {
            var excepcionConcreta = (ArgumentException)excepcion;
        return new ObjectResult(new
            {
                CodigoInterno = "ArgumentoInvalido",
                Mensaje = excepcionConcreta.Message,
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
        };
            }
        },
        {
            typeof(InvalidOperationException),
        (Exception excepcion) =>
        {
            var excepcionConcreta = (InvalidOperationException)excepcion;
        return new ObjectResult(new
            {
                CodigoInterno = "OperacionInvalida",
                Mensaje = excepcionConcreta.Message,
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
        };
            }
        },
        {
            typeof(FileNotFoundException),
            (Exception excepcion) =>
            {
                var excepcionConcreta = (FileNotFoundException)excepcion;
                return new ObjectResult(new
                {
                    CodigoInterno = "OperacionInvalida",
                    Mensaje = excepcionConcreta.Message,
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        },
        {
            typeof(FormatException),
            (Exception excepcion) =>
            {
                var excepcionConcreta = (FormatException)excepcion;
                return new ObjectResult(new
                {
                    CodigoInterno = "FormatoInvalido",
                    Mensaje = excepcionConcreta.Message,
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        },
        {
            typeof(KeyNotFoundException),
            (Exception excepcion) =>
            {
                var excepcionConcreta = (KeyNotFoundException)excepcion;
                return new ObjectResult(new
                {
                    CodigoInterno = "ElementoNoEncontrado",
                    Mensaje = excepcionConcreta.Message,
                })
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
        }
    };

    public void OnException(ExceptionContext contexto)
    {
        var response = _errores.GetValueOrDefault(contexto.Exception.GetType());

        if (response == null)
        {
            contexto.Result = new ObjectResult(new
            {
                CodigoInterno = "ErrorInterno",
                Mensaje = "Error al procesar la solicitud"
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            return;
        }

        contexto.Result = response(contexto.Exception);
    }
}
