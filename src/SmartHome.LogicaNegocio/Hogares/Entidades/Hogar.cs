using SmartHome.LogicaNegocio.Cuartos.Entidades;

namespace SmartHome.LogicaNegocio.Hogares.Entidades;
public sealed record class Hogar
{
    public Guid Id { get; init; }
    public string? Alias { get; set; } = null!;
    public string Calle { get; init; } = null!;
    public int NumPuerta { get; init; }
    public int Latitud { get; init; }
    public int Longitud { get; init; }
    public int CantMiembrosSoportados { get; init; }
    public Guid DueñoId { get; init; }
    public List<MiembroHogar> Miembros { get; set; } = null!;
    public List<Cuarto> Cuartos { get; set; } = null!;

    public Hogar()
    {
        Id = Guid.NewGuid();
    }
}
