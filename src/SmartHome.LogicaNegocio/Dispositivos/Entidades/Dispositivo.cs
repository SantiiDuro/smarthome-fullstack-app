using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.LogicaNegocio.Dispositivos.Entidades;
public sealed record class Dispositivo
{
    public Guid Id { get; init; }
    public TipoDispositivo Tipo { get; init; }
    public string Nombre { get; init; } = null!;
    public string Modelo { get; init; } = null!;
    public string Descripcion { get; init; } = null!;
    public List<FotografiaDispositivo> Fotografias { get; init; } = null!;
    public Empresa Empresa { get; init; } = null!;
    public Guid EmpresaId { get; init; }
    public bool? DetectaMovimiento { get; init; }
    public bool? DetectaPersona { get; init; }
    public bool? UsoExterior { get; init; }
    public bool? UsoInterior { get; init; }

    public Dispositivo()
    {
        Id = Guid.NewGuid();
    }
}
