namespace SmartHome.LogicaNegocio.Dispositivos.Entidades;

public class ParametroDispositivoFiltro
{
    public string NombreDispositivo { get; set; } = null!;
    public string Modelo { get; set; } = null!;
    public string NombreEmpresa { get; set; } = null!;
    public string TipoDispositivo { get; set; } = null!;

    public ParametroDispositivoFiltro(string nombreDispositivo, string modelo, string nombreEmpresa, string tipoDispositivo)
    {
        NombreDispositivo = nombreDispositivo;
        Modelo = modelo;
        NombreEmpresa = nombreEmpresa;
        TipoDispositivo = tipoDispositivo;
    }

    public ParametroDispositivoFiltro()
    {
    }
}
