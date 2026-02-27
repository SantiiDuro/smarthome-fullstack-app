using SmartHome.WebApi.Controllers.Empresas.Modelos;

public record class RespuestaEmpresas
{
    public List<InformacionRespuestaEmpresa> Empresas { get; init; }
    public int CantidadPaginas { get; init; }

    public RespuestaEmpresas(List<InformacionRespuestaEmpresa> empresas, int cantidadPaginas)
    {
        Empresas = empresas;
        CantidadPaginas = cantidadPaginas;
    }
}
