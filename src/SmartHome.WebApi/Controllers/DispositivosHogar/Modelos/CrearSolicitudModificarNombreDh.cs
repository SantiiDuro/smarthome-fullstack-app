namespace SmartHome.WebApi.Controllers.DispositivosHogar.Modelos;

public class CrearSolicitudModificarNombreDh
{
    public string Nombre { get; init; } = null!;

    public string ObtenerNombre()
    {
        if (string.IsNullOrEmpty(Nombre))
        {
            throw new ArgumentNullException(nameof(Nombre));
        }

        return Nombre;
    }
}
