namespace SmartHome.WebApi.Controllers.Dispositivos.Modelos;

public record class RespuestaDispositivos
{
    public List<InformacionRespuestaDispositivo> Dispositivos { get; init; }
    public int CantidadPaginas { get; init; }

    public RespuestaDispositivos(List<InformacionRespuestaDispositivo> dispositivos, int cantidadPaginas)
    {
        Dispositivos = dispositivos;
        CantidadPaginas = cantidadPaginas;
    }
}
