using System.Text.Json.Serialization;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Empresas;

namespace SmartHome.WebApi.Controllers.Hogares.Modelos;

public class InformacionRespuestaListarDispositivos
{
    public string Id { get; init; } = null!;
    public string Nombre { get; init; } = null!;
    public string Modelo { get; init; } = null!;
    public string FotoPrincipal { get; init; } = null!;
    public string NombreEmpresa { get; init; } = null!;
    public bool EstaConectado { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EstaAbierto { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EstaEncendida { get; init; }

    public InformacionRespuestaListarDispositivos(IEmpresaLogica logicaEmpresa, DispositivoHogar dispositivoHogar)
    {
        Id = dispositivoHogar.Id.ToString();
        Nombre = dispositivoHogar.Nombre;
        Modelo = dispositivoHogar.Dispositivo.Modelo;
        var fotografiaPrincipal = dispositivoHogar.Dispositivo.Fotografias.Where(f => f.EsPrincipal).FirstOrDefault();
        FotoPrincipal = fotografiaPrincipal.Url;
        NombreEmpresa = logicaEmpresa.ObtenerPorId(dispositivoHogar.Dispositivo.EmpresaId).Nombre;
        EstaConectado = dispositivoHogar.EstaConectado;
        EstaAbierto = dispositivoHogar.EstaAbierto;
        EstaEncendida = dispositivoHogar.EstaEncendida;
    }
}
