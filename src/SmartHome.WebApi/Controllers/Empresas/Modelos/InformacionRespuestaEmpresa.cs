using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.WebApi.Controllers.Empresas.Modelos;

public record InformacionRespuestaEmpresa
{
    public string Nombre { get; init; }
    public string Logotipo { get; init; }
    public string Rut { get; init; }
    public string NombreDueño { get; init; }
    public string Id { get; init; }

    public InformacionRespuestaEmpresa(Empresa empresa)
    {
        Nombre = empresa.Nombre;
        Logotipo = empresa.Logotipo;
        Rut = empresa.Rut;
        NombreDueño = empresa.NombreCreador;
        Id = empresa.Id.ToString();
    }
}
