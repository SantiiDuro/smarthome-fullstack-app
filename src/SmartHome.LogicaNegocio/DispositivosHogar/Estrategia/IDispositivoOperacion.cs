using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;

namespace SmartHome.LogicaNegocio.DispositivosHogar.Estrategia;

public interface IDispositivoOperacion
{
    bool EjecutarOperacion(DispositivoHogar dispositivoHogar);
}
