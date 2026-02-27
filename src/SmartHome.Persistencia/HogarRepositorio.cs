using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Hogares.Entidades;

namespace SmartHome.Persistencia;

public class HogarRepositorio(ContextoSql contexto)
    : IHogarRepositorio
{
    private readonly ContextoSql _contexto = contexto;

    public void GuardarCambios()
    {
        _contexto.SaveChanges();
    }

    public void Agregar(Hogar hogar)
    {
        _contexto.Hogares.Add(hogar);
    }

    public List<Hogar> ObtenerTodos()
    {
        return _contexto.Hogares.Include(h => h.Miembros).Include(h => h.Cuartos).ToList();
    }

    public void AgregarMiembro(MiembroHogar miembro)
    {
        var hogar = _contexto.Hogares.First(h => h.Id == miembro.HogarId);
        _contexto.Entry(hogar).Collection(h => h.Miembros).Load();
        _contexto.Add(miembro);

        GuardarCambios();
    }

    public void ActualizarMiembro(MiembroHogar miembro)
    {
        var hogar = _contexto.Hogares.First(h => h.Id == miembro.HogarId);

        var miembroExistente = _contexto.MiembrosHogar.FirstOrDefault(m => m.Id == miembro.Id);

        miembroExistente.Notificaciones = miembro.Notificaciones;

        contexto.MiembrosHogar.Update(miembroExistente);
    }

    public bool Existe(Expression<Func<Hogar, bool>> predicado)
    {
        return _contexto.Hogares.Any(predicado);
    }

    public void Actualizar(Hogar hogar)
    {
        _contexto.Hogares.Update(hogar);

        GuardarCambios();
    }
}
