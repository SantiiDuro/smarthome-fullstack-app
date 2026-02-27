using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;

namespace SmartHome.LogicaNegocio.Cuartos.Entidades;
public class Cuarto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = null!;
    public Hogar Hogar { get; set; } = null!;
    public Guid HogarId { get; set; }
    public List<DispositivoHogar> DispositivosHogar { get; set; } = null!;

    public Cuarto()
    {
        Id = Guid.NewGuid();
    }
}
