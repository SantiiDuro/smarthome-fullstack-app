namespace SmartHome.LogicaNegocio.DispositivosHogar.Entidades;

public class ParametroDispositivoHogarFiltro
{
    public string NombreCuarto { get; set; } = null!;

    public ParametroDispositivoHogarFiltro(string nombreCuarto)
    {
        NombreCuarto = nombreCuarto;
    }

    public ParametroDispositivoHogarFiltro()
    {
    }
}
