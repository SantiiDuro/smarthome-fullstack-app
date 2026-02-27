using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;

namespace SmartHome.LogicaNegocio.DispositivosHogar.Estrategia;

public class OperacionEncenderLampara
    : IDispositivoOperacion
{
    public bool EjecutarOperacion(DispositivoHogar dispositivoHogar)
    {
        if (dispositivoHogar.EstaEncendida.HasValue && dispositivoHogar.EstaEncendida.Value)
        {
            return false;
        }

        dispositivoHogar.EstaEncendida = true;
        return true;
    }
}
