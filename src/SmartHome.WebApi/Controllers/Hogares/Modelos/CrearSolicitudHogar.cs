using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.WebApi.Controllers.Hogares.Modelos;

public sealed record CrearSolicitudHogar
{
    public string Calle { get; init; } = null!;
    public int NumPuerta { get; init; }
    public int Latitud { get; init; }
    public int Longitud { get; init; }
    public int CantMiembrosSoportados { get; init; }
    public string? Alias { get; init; }

    public CrearHogaresArgs Args(Usuario usuario)
    {
        if (string.IsNullOrEmpty(Calle))
        {
            throw new ArgumentNullException(nameof(Calle));
        }

        if (Latitud < -90 || Latitud > 90)
        {
            throw new ArgumentException("La latitud debe estar entre -90 y 90");
        }

        if (Longitud < -180 || Longitud > 180)
        {
            throw new ArgumentException("La longitud debe estar entre -180 y 180");
        }

        if (NumPuerta < 0)
        {
            throw new ArgumentException("La menor número de puerta permitido es 0");
        }

        if (CantMiembrosSoportados < 1)
        {
            throw new ArgumentException("La minima cantidad de miembros soportados es 1");
        }

        return new CrearHogaresArgs(
            Calle,
            NumPuerta,
            Latitud,
            Longitud,
            CantMiembrosSoportados,
            Alias,
            usuario);
    }
}
