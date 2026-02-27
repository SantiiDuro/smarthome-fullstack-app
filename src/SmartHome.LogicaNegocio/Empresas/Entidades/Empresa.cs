using SmartHome.LogicaNegocio.Dispositivos.Entidades;

namespace SmartHome.LogicaNegocio.Empresas.Entidades;

public class Empresa
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = null!;
    public string Logotipo { get; init; } = null!;
    public string Rut { get; init; } = null!;
    public string NombreCreador { get; init; } = null!;
    public List<Dispositivo> Dispositivos { get; set; } = null!;
    public string Validador { get; init; } = null!;

    public Empresa()
    {
        Id = Guid.NewGuid();
    }
}
