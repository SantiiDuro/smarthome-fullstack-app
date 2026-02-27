using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.LogicaNegocio.Dispositivos.Entidades;

public class CrearCamarasArgs
{
    public string Nombre { get; set; }
    public string Modelo { get; set; }
    public string Descripcion { get; set; }
    public List<FotografiaDispositivo> Fotografias { get; set; }
    public Empresa Empresa { get; set; }
    public bool DetectaMovimiento { get; init; }
    public bool DetectaPersona { get; init; }
    public bool UsoExterior { get; init; }
    public bool UsoInterior { get; init; }

    public CrearCamarasArgs(
        string nombre,
        string modelo,
        string descripcion,
        List<FotografiaDispositivo> fotografias,
        Empresa empresa,
        bool detectaMovimiento,
        bool detectaPersona,
        bool usoExterior,
        bool usoInterior)
    {
        if (string.IsNullOrEmpty(nombre))
        {
            throw new ArgumentNullException(nameof(nombre));
        }

        if (string.IsNullOrEmpty(modelo))
        {
            throw new ArgumentNullException(nameof(modelo));
        }

        if (string.IsNullOrEmpty(descripcion))
        {
            throw new ArgumentNullException(nameof(descripcion));
        }

        ArgumentNullException.ThrowIfNull(fotografias);

        if (!fotografias.Any(f => f.EsPrincipal == true))
        {
            throw new InvalidOperationException("El dispositivo no contiene fotografía principal");
        }

        var fotografiasPrincipales = fotografias.Where(f => f.EsPrincipal == true);
        if (fotografiasPrincipales.Count() > 1)
        {
            throw new InvalidOperationException("El dispositivo contiene más de una fotografía principal");
        }

        Nombre = nombre;
        Modelo = modelo;
        Descripcion = descripcion;
        Fotografias = fotografias;
        Empresa = empresa;
        DetectaMovimiento = detectaMovimiento;
        DetectaPersona = detectaPersona;
        UsoExterior = usoExterior;
        UsoInterior = usoInterior;
    }
}
