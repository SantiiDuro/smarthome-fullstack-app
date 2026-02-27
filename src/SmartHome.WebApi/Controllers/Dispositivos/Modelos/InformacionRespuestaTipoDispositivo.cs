using SmartHome.LogicaNegocio.Dispositivos.Entidades;

namespace SmartHome.WebApi.Controllers.Dispositivos.Modelos;

public class InformacionRespuestaTipoDispositivo
{
    public string Tipo { get; init; }

    public InformacionRespuestaTipoDispositivo(TipoDispositivo tipo)
    {
        Tipo = tipo.ToString();
    }
}
