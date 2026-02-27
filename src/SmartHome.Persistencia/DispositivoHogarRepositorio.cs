using Microsoft.EntityFrameworkCore;
using SmartHome.LogicaNegocio.DispositivosHogar;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;

namespace SmartHome.Persistencia;
public class DispositivoHogarRepositorio(ContextoSql contexto)
    : IDispositivoHogarRepositorio
{
    private readonly ContextoSql _contexto = contexto;

    public void GuardarCambios()
    {
        _contexto.SaveChanges();
    }

    public DispositivoHogar Agregar(DispositivoHogar dispositivoHogar)
    {
        _contexto.DispositivosHogar.Add(dispositivoHogar);

        return dispositivoHogar;
    }

    public List<DispositivoHogar> ObtenerTodos(ParametroDispositivoHogarFiltro? filtro)
    {
        var query = _contexto.DispositivosHogar
            .Include(dh => dh.Dispositivo)
            .Include(dh => dh.Dispositivo.Fotografias)
            .Include(dh => dh.Hogar)
            .Include(dh => dh.Cuarto).ToList();

        if (filtro != null)
        {
            if (!string.IsNullOrEmpty(filtro.NombreCuarto))
            {
                query = query.Where(x => x.Cuarto != null && x.Cuarto.Nombre
                    .Equals(filtro.NombreCuarto, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
        }

        return query.ToList();
    }

    public void Actualizar(DispositivoHogar dh)
    {
        _contexto.DispositivosHogar.Update(dh);

        GuardarCambios();
    }
}
