using Microsoft.EntityFrameworkCore;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Sesiones.Entidades;

namespace SmartHome.Persistencia;

public class SesionRepositorio(ContextoSql contexto)
    : ISesionRepositorio
{
    private readonly ContextoSql _contexto = contexto;

    public void Agregar(Sesion sesion)
    {
        _contexto.Sesiones.Add(sesion);
    }

    public void GuardarCambios()
    {
        _contexto.SaveChanges();
    }

    public List<Sesion> ObtenerTodos()
    {
        return _contexto.Sesiones.Include(u => u.Usuario).Include(s => s.Usuario.Rol).Include(u => u.Usuario.Empresa).ToList();
    }

    public void Eliminar(string token)
    {
        var sesion = _contexto.Sesiones.FirstOrDefault(s => s.Token == token);

        if (sesion is not null)
        {
            _contexto.Sesiones.Remove(sesion);
        }
    }
}
