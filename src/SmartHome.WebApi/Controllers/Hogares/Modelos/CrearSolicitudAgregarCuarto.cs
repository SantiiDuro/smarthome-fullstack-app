using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;

namespace SmartHome.WebApi.Controllers.Hogares.Modelos;

public sealed record CrearSolicitudAgregarCuarto
{
    public string Nombre { get; init; } = null!;

    public CrearCuartosArgs Args(Hogar hogar)
    {
        if (string.IsNullOrEmpty(Nombre))
        {
            throw new ArgumentNullException(nameof(Nombre));
        }

        return new CrearCuartosArgs(
            Nombre,
            hogar);
    }
}
