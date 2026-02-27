using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.DispositivosHogar.Estrategia;

namespace SmartHome.LogicaNegocio.DispositivosHogar.Fabrica;

public class DispositivoOperacionFactory
{
    public static IDispositivoOperacion CrearOperacion(TipoDispositivo tipo, string operacion)
    {
        switch (tipo, operacion)
        {
            case (TipoDispositivo.SensorVentana, "Abre"):
                return new OperacionSensorVentanaAbre();
            case (TipoDispositivo.SensorVentana, "Cierra"):
                return new OperacionSensorVentanaCierra();
            case (TipoDispositivo.Lampara, "Encender"):
                return new OperacionEncenderLampara();
            case (TipoDispositivo.Lampara, "Apagar"):
                return new OperacionApagarLampara();
            default:
                throw new InvalidOperationException("Operación no soportada para el tipo de dispositivo");
        }
    }
}
