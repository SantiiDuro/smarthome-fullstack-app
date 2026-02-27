using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;

namespace SmartHome.LogicaNegocio.DispositivosHogar.Estrategia;

public class OperacionSensorVentanaCierra
    : IDispositivoOperacion
{
    public bool EjecutarOperacion(DispositivoHogar dispositivoHogar)
    {
        if (dispositivoHogar.EstaAbierto.HasValue && !dispositivoHogar.EstaAbierto.Value)
        {
            return false;
        }

        dispositivoHogar.EstaAbierto = false;
        return true;
    }
}
