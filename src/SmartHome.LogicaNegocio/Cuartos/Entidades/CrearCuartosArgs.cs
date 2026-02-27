using SmartHome.LogicaNegocio.Hogares.Entidades;

namespace SmartHome.LogicaNegocio.Cuartos.Entidades;
public sealed record class CrearCuartosArgs
{
    public readonly string Nombre = null!;
    public readonly Hogar Hogar = null!;

    public CrearCuartosArgs(
        string nombre,
        Hogar hogar)
    {
        if (string.IsNullOrEmpty(nombre))
        {
            throw new ArgumentNullException(nameof(nombre));
        }

        ArgumentNullException.ThrowIfNull(hogar);

        Nombre = nombre;
        Hogar = hogar;
    }
}
