using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Hogares.Entidades;
public sealed class CrearHogaresArgs
{
    public readonly string Calle = null!;
    public readonly int NumPuerta;
    public readonly int Latitud;
    public readonly int Longitud;
    public readonly int CantMiembrosSoportados;
    public readonly string? Alias;
    public readonly Guid DueñoId;

    public CrearHogaresArgs(
        string calle,
        int numPuerta,
        int latitud,
        int longitud,
        int cantMiembrosSoportados,
        string? alias,
        Usuario dueño)
    {
        if (string.IsNullOrEmpty(calle))
        {
            throw new ArgumentNullException(nameof(calle));
        }

        if (latitud < -90 || latitud > 90)
        {
            throw new ArgumentException("La latitud debe estar entre -90 y 90");
        }

        if (longitud < -180 || longitud > 180)
        {
            throw new ArgumentException("La longitud debe estar entre -180 y 180");
        }

        if (numPuerta < 0)
        {
            throw new ArgumentException("La menor número de puerta permitido es 0");
        }

        if (cantMiembrosSoportados < 1)
        {
            throw new ArgumentException("La minima cantidad de miembros soportados es 1");
        }

        ArgumentNullException.ThrowIfNull(dueño);

        Calle = calle;
        NumPuerta = numPuerta;
        Latitud = latitud;
        Longitud = longitud;
        CantMiembrosSoportados = cantMiembrosSoportados;
        Alias = alias;
        DueñoId = dueño.Id;
    }
}
