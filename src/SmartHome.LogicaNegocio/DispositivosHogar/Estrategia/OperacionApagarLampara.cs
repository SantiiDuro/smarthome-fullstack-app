using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;

namespace SmartHome.LogicaNegocio.DispositivosHogar.Estrategia;

public class OperacionApagarLampara
    : IDispositivoOperacion
{
    public bool EjecutarOperacion(DispositivoHogar dispositivoHogar)
    {
        if (dispositivoHogar.EstaEncendida.HasValue && !dispositivoHogar.EstaEncendida.Value)
        {
            return false;
        }

        dispositivoHogar.EstaEncendida = false;
        return true;
    }
}
