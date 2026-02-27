using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.LogicaNegocio.Usuarios.Entidades;

public sealed record class Usuario
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = null!;
    public string Apellido { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Contraseña { get; init; } = null!;
    public Rol Rol { get; set; } = null!;
    public Guid RolId { get; set; }
    public string? FotoPerfil { get; set; }
    public Empresa? Empresa { get; set; }

    public DateTime FechaCreacion { get; init; }

    public Usuario()
    {
        Id = Guid.NewGuid();
    }
}
