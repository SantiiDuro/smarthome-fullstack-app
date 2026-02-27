namespace SmartHome.LogicaNegocio.Empresas.Entidades;
public sealed record class ObtenerEmpresasArgs
{
    public readonly List<Empresa> Empresas = null!;
    public readonly int CantidadPaginas;

    public ObtenerEmpresasArgs(List<Empresa> empresas, int cantidadPaginas)
    {
        Empresas = empresas;
        CantidadPaginas = cantidadPaginas;
    }
}
