using ModeloValidador.Abstracciones;
using SmartHome.CargarImplementacion;
using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Empresas;

public sealed class EmpresaLogica(IEmpresaRepositorio repositorioEmpresa, IUsuarioRepositorio repositorioUsuario)
    : IEmpresaLogica
{
    public Empresa Agregar(CrearEmpresasArgs args, Usuario usuario)
    {
        if (usuario.Empresa is not null)
        {
            throw new InvalidOperationException("Un usuario solo puede ser dueño de una única empresa.");
        }

        var nombreUnico = EsNombreUnico(args.Nombre);
        var rutUnico = EsRutUnico(args.Rut);

        if (!nombreUnico)
        {
            throw new ArgumentException("El nombre de la empresa ya está en uso. Debe ser único.");
        }

        if (!rutUnico)
        {
            throw new ArgumentException("El RUT de la empresa ya está registrado. Debe ser único.");
        }

        var empresa = new Empresa
        {
            Nombre = args.Nombre,
            Logotipo = args.Logotipo,
            Rut = args.Rut,
            NombreCreador = usuario.Nombre + " " + usuario.Apellido,
            Dispositivos = [],
            Validador = args.Validador
        };

        repositorioEmpresa.Agregar(empresa);

        usuario.Empresa = empresa;

        repositorioUsuario.Actualizar(usuario);

        return empresa;
    }

    public List<string> ObtenerIdentificadoresDeImplementaciones()
    {
        var directorioValidadores = BuscarDirectorioConCarpeta("Validadores");

        var administradorImplementacion = new AdministradorImplementacion<IModeloValidador>(directorioValidadores);
        var identificadores = administradorImplementacion.ObtenerIdentificadores();

        return identificadores;
    }

    private string BuscarDirectorioConCarpeta(string carpetaObjetivo)
    {
        var directorioActual = AppDomain.CurrentDomain.BaseDirectory;

        while (!string.IsNullOrEmpty(directorioActual))
        {
            var rutaObjetivo = Path.Combine(directorioActual, carpetaObjetivo);
            if (Directory.Exists(rutaObjetivo))
            {
                return rutaObjetivo;
            }

            directorioActual = Path.GetFullPath(Path.Combine(directorioActual, ".."));
        }

        throw new DirectoryNotFoundException($"No se pudo encontrar la carpeta '{carpetaObjetivo}' en la jerarquía de directorios.");
    }

    public void GuardarCambios()
    {
        repositorioEmpresa.GuardarCambios();
    }

    public ObtenerEmpresasArgs ObtenerTodos(ParametroPaginacion? parametroPaginacion,
        ParametroEmpresaFiltro? parametroFiltro)
    {
        return repositorioEmpresa.ObtenerTodos(parametroPaginacion, parametroFiltro);
    }

    private bool EsNombreUnico(string nombre)
    {
        return !repositorioEmpresa.Existe(e => e.Nombre == nombre);
    }

    private bool EsRutUnico(string rut)
    {
        return !repositorioEmpresa.Existe(e => e.Rut == rut);
    }

    public Empresa ObtenerPorId(Guid id)
    {
        return repositorioEmpresa.ObtenerPorId(id);
    }
}
