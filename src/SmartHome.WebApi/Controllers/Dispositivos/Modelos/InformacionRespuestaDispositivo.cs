using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Empresas;

namespace SmartHome.WebApi.Controllers.Dispositivos.Modelos;

public record InformacionRespuestaDispositivo
{
    public string Id { get; init; } = null!;
    public string Nombre { get; init; } = null!;
    public string Modelo { get; init; } = null!;
    public string Descripcion { get; init; } = null!;
    public string FotoPrincipal { get; init; } = null!;
    public string NombreEmpresa { get; init; } = null!;
    public string Tipo { get; init; } = null!;

    public InformacionRespuestaDispositivo(IEmpresaLogica logicaEmpresa, Dispositivo dispositivo)
    {
        Id = dispositivo.Id.ToString();
        Nombre = dispositivo.Nombre;
        Modelo = dispositivo.Modelo;
        Descripcion = dispositivo.Descripcion;
        var fotografiaPrincipal = dispositivo.Fotografias.Where(f => f.EsPrincipal).FirstOrDefault();
        FotoPrincipal = fotografiaPrincipal.Url;
        NombreEmpresa = logicaEmpresa.ObtenerPorId(dispositivo.EmpresaId).Nombre;
        Tipo = dispositivo.Tipo.ToString();
    }
}
