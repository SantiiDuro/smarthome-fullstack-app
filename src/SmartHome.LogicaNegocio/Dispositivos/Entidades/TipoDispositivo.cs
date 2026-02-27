namespace SmartHome.LogicaNegocio.Dispositivos.Entidades;

/// <summary>
/// Enumera los distintos tipos de dispositivos que se pueden tener en el sistema.
/// </summary>
public enum TipoDispositivo
{
    /// <summary>
    /// Dispositivo de tipo sensor que detecta si una ventana se abre o cierra.
    /// </summary>
    SensorVentana,

    /// <summary>
    /// Dispositivo de tipo cámara.
    /// </summary>
    Camara,

    /// <summary>
    /// Sensor que detecta movimiento.
    /// </summary>
    SensorMovimiento,

    /// <summary>
    /// Lampara inteligente.
    /// </summary>
    Lampara
}
