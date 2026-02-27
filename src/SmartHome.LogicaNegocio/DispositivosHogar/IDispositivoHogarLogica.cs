using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.DispositivosHogar;
public interface IDispositivoHogarLogica
{
    void GuardarCambios();
    DispositivoHogar Agregar(CrearDispositivosHogarArgs args, Usuario usuario);
    List<DispositivoHogar> ObtenerDispositivosDeHogar(string idHogar, Usuario usuario, ParametroDispositivoHogarFiltro? filtro);
    DispositivoHogar ObtenerDispositivoHogarPorId(string hardwardId);
    void SensorDetectaMovimiento(string hardwardId);
    void CamaraDetectaMovimiento(string hardwardId);
    void CamaraDetectaPersona(string hardwardId);
    void Conectar(string dispositivoHogarId, Usuario usuario);
    void Desconectar(string dispositivoHogarId, Usuario usuario);
    void AgregarACuarto(string dispositivoHogarId, Cuarto cuarto, Usuario usuario);
    void ActualizarNombreDispositivoHogar(string hardwardId, string nombre, Usuario usuario);
    bool EjecutarOperacionDispositivo(string hardwardId, string operacion);
}
