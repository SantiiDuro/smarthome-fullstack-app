using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;

namespace SmartHome.LogicaNegocio.DispositivosHogar;
public interface IDispositivoHogarRepositorio
{
    void GuardarCambios();
    DispositivoHogar Agregar(DispositivoHogar dispositivoHogar);
    void Actualizar(DispositivoHogar dispositivoHogar);
    List<DispositivoHogar> ObtenerTodos(ParametroDispositivoHogarFiltro? filtro);
}
