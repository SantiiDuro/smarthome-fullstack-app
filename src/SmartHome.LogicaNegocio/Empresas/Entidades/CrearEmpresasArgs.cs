namespace SmartHome.LogicaNegocio.Empresas.Entidades;

public sealed record class CrearEmpresasArgs
{
    public readonly string Nombre = null!;
    public readonly string Logotipo = null!;
    public readonly string Rut = null!;
    public readonly string Validador = null!;
    public CrearEmpresasArgs(
        string nombre,
        string logotipo,
        string rut,
        string validador)
    {
        if (string.IsNullOrEmpty(nombre))
        {
            throw new ArgumentNullException(nameof(nombre));
        }

        if (string.IsNullOrEmpty(logotipo))
        {
            throw new ArgumentNullException(nameof(logotipo));
        }

        if (string.IsNullOrEmpty(rut))
        {
            throw new ArgumentNullException(nameof(rut));
        }

        if (string.IsNullOrEmpty(validador))
        {
            throw new ArgumentNullException(nameof(validador));
        }

        var validadoresRuta = BuscarDirectorioConCarpeta("Validadores");
        if (validadoresRuta is null)
        {
            throw new DirectoryNotFoundException("No se pudo encontrar la carpeta 'Validadores' en la jerarquía de directorios.");
        }

        var dllRuta = Path.Combine(validadoresRuta, $"{validador}.dll");

        if (!File.Exists(dllRuta))
        {
            throw new FileNotFoundException($"No se encontró el archivo DLL para el validador: {validador}", dllRuta);
        }

        Nombre = nombre;
        Logotipo = logotipo;
        Rut = rut;
        Validador = validador;
    }

    private string BuscarDirectorioConCarpeta(string carpetaObjetivo)
    {
        var directorioActual = AppDomain.CurrentDomain.BaseDirectory;

        while (!string.IsNullOrEmpty(directorioActual))
        {
            var rutaObjetivo = Path.Combine(directorioActual, carpetaObjetivo);
            if (Directory.Exists(rutaObjetivo))
            {
                return rutaObjetivo;
            }

            directorioActual = Path.GetFullPath(Path.Combine(directorioActual, ".."));
        }

        throw new DirectoryNotFoundException($"No se pudo encontrar la carpeta '{carpetaObjetivo}' en la jerarquía de directorios.");
    }
}
