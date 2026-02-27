using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.Persistencia;

public class UsuarioRepositorio(ContextoSql contexto)
    : IUsuarioRepositorio
{
    private readonly ContextoSql _contexto = contexto;

    public void GuardarCambios()
    {
        _contexto.SaveChanges();
    }

    public void Agregar(Usuario usuario)
    {
        _contexto.Usuarios.Add(usuario);
    }

    public bool Existe(Expression<Func<Usuario, bool>> predicado)
    {
        return _contexto.Usuarios.Any(predicado);
    }

    public ObtenerUsuariosArgs ObtenerTodos(ParametroPaginacion? parametroPaginacion,
        ParametroUsuarioFiltro? parametroFiltro)
    {
        var query = _contexto.Usuarios.Include(u => u.Rol)
            .Include(u => u.Empresa).AsQueryable();

        parametroPaginacion ??= new ParametroPaginacion(1, 10);
        if (parametroFiltro != null)
        {
            if (!string.IsNullOrEmpty(parametroFiltro.Rol))
            {
                query = query.Where(u => u.Rol.Tipo.ToLower() == parametroFiltro.Rol.ToLower());
            }

            if (!string.IsNullOrEmpty(parametroFiltro.NombreCompleto))
            {
                query = query.Where(u => (u.Nombre + " " + u.Apellido).ToLower()
                                         == parametroFiltro.NombreCompleto.ToLower());
            }
        }

        var totalUsuarios = query.Count();
        var cantidadPaginas = (totalUsuarios + parametroPaginacion.TamañoDePagina - 1) /
                             parametroPaginacion.TamañoDePagina;

        var usuarios = query
            .Skip((parametroPaginacion.NumeroDePagina - 1) * parametroPaginacion.TamañoDePagina)
            .Take(parametroPaginacion.TamañoDePagina)
            .ToList();

        return new ObtenerUsuariosArgs(usuarios, cantidadPaginas);
    }

    public void Eliminar(string email)
    {
        var usuario = _contexto.Usuarios.FirstOrDefault(u => u.Email == email);

        if (usuario is not null)
        {
            _contexto.Usuarios.Remove(usuario);
        }
    }

    public Rol ObtenerRolPorId(Guid rolId)
    {
        return _contexto.Roles.Find(rolId);
    }

    public void Actualizar(Usuario usuario)
    {
        _contexto.Usuarios.Update(usuario);
        _contexto.SaveChanges();
    }

    public Usuario ObtenerPorId(Guid id)
    {
        return _contexto.Usuarios
            .Include(u => u.Rol)
            .Include(u => u.Empresa)
            .FirstOrDefault(u => u.Id == id);
    }

    public Usuario ObtenerPorEmail(string email)
    {
        return _contexto.Usuarios
            .Include(u => u.Rol)
            .Include(u => u.Empresa)
            .FirstOrDefault(u => u.Email == email);
    }
}
