namespace SmartHome.LogicaNegocio;

public class ParametroPaginacion
{
    public int NumeroDePagina { get; set; }
    public int TamañoDePagina { get; set; }

    public ParametroPaginacion(int numeroDePagina, int tamañoDePagina)
    {
        if (numeroDePagina <= 0 || tamañoDePagina <= 0)
        {
            numeroDePagina = 1;
            tamañoDePagina = 10;
        }

        NumeroDePagina = numeroDePagina;
        TamañoDePagina = tamañoDePagina;
    }

    public ParametroPaginacion()
    {
        NumeroDePagina = 1;
        TamañoDePagina = 10;
    }
}
