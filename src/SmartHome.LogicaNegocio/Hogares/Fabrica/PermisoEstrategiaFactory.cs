using SmartHome.LogicaNegocio.Hogares.Estrategia;

namespace SmartHome.LogicaNegocio.Hogares.Fabrica;

public class PermisoEstrategiaFactory
{
    public static IPermisoEstrategia CrearEstrategia(string accion)
    {
        switch (accion)
        {
            case "AsociarDispositivo":
                return new PermisoAsociarDispositivoEstrategia();
            case "AgregarMiembro":
                return new PermisoAgregarMiembroEstrategia();
            case "ListarDispositivo":
                return new PermisoListarDispositivoEstrategia();
            case "ListarMiembro":
                return new PermisoListarMiembroEstrategia();
            case "ModificarAlias":
                return new PermisoModificarAliasEstrategia();
            case "AdministrarCuarto":
                return new PermisoAdministrarCuartoEstrategia();
            case "ModificarNombreDispositivo":
                return new PermisoModificarNombreDispositivoEstrategia();
            default:
                throw new InvalidOperationException("Permiso no existente");
        }
    }
}
