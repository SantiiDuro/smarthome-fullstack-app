using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Sesiones.Entidades;

public sealed record class Sesion
{
    public Guid Id { get; init; }

    public string Token { get; init; } = null!;

    public Usuario Usuario { get; init; } = null!;
}
