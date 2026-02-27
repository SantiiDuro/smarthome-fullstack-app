using FluentAssertions;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public sealed class ParametroPaginacionTest
{
    [TestMethod]
    public void ConstructorConNumeroDePaginaYTamañoDePaginaValidosDevuelveValoresCorrectos()
    {
        var numeroDePagina = 2;
        var tamanoDePagina = 5;

        var parametroPaginacion = new ParametroPaginacion(numeroDePagina, tamanoDePagina);

        parametroPaginacion.NumeroDePagina.Should().Be(2);
        parametroPaginacion.TamañoDePagina.Should().Be(5);
    }

    [TestMethod]
    public void ConstructorConTamanoDePaginaMenorOIgualACeroEstableceValorPorDefecto()
    {
        var numeroDePagina = 1;
        var tamanoDePagina = 0;

        var parametroPaginacion = new ParametroPaginacion(numeroDePagina, tamanoDePagina);

        parametroPaginacion.NumeroDePagina.Should().Be(1);
        parametroPaginacion.TamañoDePagina.Should().Be(10);
    }

    [TestMethod]
    public void ConstructorConNumeroDePaginaYTamañoDePaginaInvalidosEstableceValoresPorDefecto()
    {
        var numeroDePagina = -1;
        var tamanoDePagina = -5;

        var parametroPaginacion = new ParametroPaginacion(numeroDePagina, tamanoDePagina);

        parametroPaginacion.NumeroDePagina.Should().Be(1);
        parametroPaginacion.TamañoDePagina.Should().Be(10);
    }
}
