using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.LogicaNegocio.Dispositivos;
public interface IDispositivoLogica
{
    void GuardarCambios();
    Dispositivo AgregarSensorVentana(CrearSensoresArgs sensores);
    Dispositivo AgregarSensorMovimiento(CrearSensoresArgs sensores);
    Dispositivo AgregarCamara(CrearCamarasArgs camaras);
    Dispositivo AgregarLampara(CrearLamparasArgs lampara);
    Dispositivo ObtenerPorId(string id);
    List<TipoDispositivo> ObtenerTiposDeDispositivos();
    ObtenerDispositivosArgs ObtenerTodos(ParametroPaginacion? parametroPaginacion,
        ParametroDispositivoFiltro? parametroDispositivoFiltro);
    void ImportarDispositivos(string ruta, string identificadorImportador, Empresa empresa);
    List<string> ObtenerIdentificadoresDeImportadores();
}
