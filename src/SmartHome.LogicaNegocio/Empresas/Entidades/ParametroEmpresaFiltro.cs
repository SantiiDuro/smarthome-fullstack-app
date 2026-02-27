namespace SmartHome.LogicaNegocio.Empresas.Entidades;

public class ParametroEmpresaFiltro
{
    public string Nombre { get; set; } = null!;
    public string NombreCompletoCreador { get; set; } = null!;

    public ParametroEmpresaFiltro(string nombre, string nombreCompletoCreador)
    {
        Nombre = nombre;
        NombreCompletoCreador = nombreCompletoCreador;
    }

    public ParametroEmpresaFiltro()
    {
    }
}
