namespace SmartHome.LogicaNegocio.Dispositivos.Entidades;
public sealed record class FotografiaDispositivo
{
    public Guid Id { get; init; }
    public string Url { get; init; } = null!;
    public bool EsPrincipal { get; init; }

    public FotografiaDispositivo()
    {
        Id = Guid.NewGuid();
    }
}
