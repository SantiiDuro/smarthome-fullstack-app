namespace SmartHome.LogicaNegocio.Notificaciones.Entidades;

public class ParametroNotificacionFiltro
{
    public string TipoDispositivo { get; set; } = null!;
    public DateTime FechaDeCreacion { get; set; }
    public string Leida { get; set; } = null!;

    public ParametroNotificacionFiltro(string tipoDispositivo, DateTime fechaDeCreacion, string leida)
    {
        TipoDispositivo = tipoDispositivo;
        FechaDeCreacion = fechaDeCreacion;
        Leida = leida;
    }

    public ParametroNotificacionFiltro()
    {
    }
}
