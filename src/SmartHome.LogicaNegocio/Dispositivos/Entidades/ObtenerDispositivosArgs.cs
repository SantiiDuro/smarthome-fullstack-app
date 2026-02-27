namespace SmartHome.LogicaNegocio.Dispositivos.Entidades;

public sealed record class ObtenerDispositivosArgs
{
    public readonly List<Dispositivo> Dispositivos = null!;
    public readonly int CantidadPaginas;

    public ObtenerDispositivosArgs(List<Dispositivo> dispositivos, int cantidadPaginas)
    {
        Dispositivos = dispositivos;
        CantidadPaginas = cantidadPaginas;
    }
}
