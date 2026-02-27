using SmartHome.LogicaNegocio.Empresas.Entidades;
namespace SmartHome.LogicaNegocio.Dispositivos.Entidades;

public sealed record class CrearSensoresArgs
{
    public readonly string Nombre = null!;
    public readonly string Modelo;
    public readonly string Descripcion = null!;
    public readonly List<FotografiaDispositivo> Fotografias = null!;
    public readonly Empresa Empresa = null!;

    public CrearSensoresArgs(
        string nombre,
        string modelo,
        string descripcion,
        List<FotografiaDispositivo> fotografias,
        Empresa empresa)
    {
        if (string.IsNullOrEmpty(nombre))
        {
            throw new ArgumentNullException(nameof(nombre));
        }

        if (string.IsNullOrEmpty(modelo))
        {
            throw new ArgumentNullException(nameof(modelo));
        }

        if (string.IsNullOrEmpty(descripcion))
        {
            throw new ArgumentNullException(nameof(descripcion));
        }

        ArgumentNullException.ThrowIfNull(fotografias);

        if (!fotografias.Any(f => f.EsPrincipal == true))
        {
            throw new InvalidOperationException("El dispositivo no contiene fotografía principal");
        }

        var fotografiasPrincipales = fotografias.Where(f => f.EsPrincipal == true);
        if (fotografiasPrincipales.Count() > 1)
        {
            throw new InvalidOperationException("El dispositivo contiene más de una fotografía principal");
        }

        Nombre = nombre;
        Modelo = modelo;
        Descripcion = descripcion;
        Fotografias = fotografias;
        Empresa = empresa;
    }
}
