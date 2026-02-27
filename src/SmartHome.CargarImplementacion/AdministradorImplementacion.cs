using System.Reflection;

namespace SmartHome.CargarImplementacion;

public class AdministradorImplementacion<TInterface>(string ruta)
    where TInterface : class
{
    private readonly Dictionary<string, Type> _implementaciones = [];

    public void CargarImplementaciones()
    {
        var directorio = new DirectoryInfo(ruta);
        var archivos = directorio.GetFiles("*.dll").ToList();
        _implementaciones.Clear();

        archivos.ForEach(archivo =>
        {
            var validadorCargado = Assembly.LoadFile(archivo.FullName);
            var tiposCargados = validadorCargado
                .GetTypes()
                .Where(t => t.IsClass && typeof(TInterface).IsAssignableFrom(t))
                .ToList();

            foreach (var tipo in tiposCargados)
            {
                var identifier = Path.GetFileNameWithoutExtension(archivo.Name);
                _implementaciones[identifier] = tipo;
            }
        });
    }

    public virtual List<string> ObtenerIdentificadores()
    {
        CargarImplementaciones();
        return _implementaciones.Keys.ToList();
    }

    public TInterface ObtenerImplementacionPorIdentificador(string identificador, params object[] args)
    {
        CargarImplementaciones();
        if (_implementaciones.TryGetValue(identificador, out var tipo))
        {
            return Activator.CreateInstance(tipo, args) as TInterface;
        }

        throw new InvalidOperationException($"No se encontró una implementación con el identificador {identificador}.");
    }
}
