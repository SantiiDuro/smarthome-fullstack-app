using Importador;
using ModeloValidador.Abstracciones;
using SmartHome.CargarImplementacion;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.LogicaNegocio.Dispositivos;
public sealed class DispositivoLogica(IDispositivoRepositorio repositorioDispositivo, IEmpresaRepositorio repositorioEmpresa)
    : IDispositivoLogica
{
    public void GuardarCambios()
    {
        repositorioDispositivo.GuardarCambios();
    }

    public Dispositivo AgregarSensorVentana(CrearSensoresArgs args)
    {
        if (args.Empresa == null)
        {
            throw new ArgumentNullException(nameof(args.Empresa), "El dueño del dispositivo no tiene una empresa asociada.");
        }

        var empresa = repositorioEmpresa.ObtenerPorId(args.Empresa.Id);

        var dispositivo = new Dispositivo
        {
            Tipo = TipoDispositivo.SensorVentana,
            Nombre = args.Nombre,
            Modelo = args.Modelo,
            Descripcion = args.Descripcion,
            Fotografias = args.Fotografias,
            EmpresaId = empresa.Id,
            Empresa = empresa
        };

        ValidarDispositivo(dispositivo);

        repositorioDispositivo.Agregar(dispositivo);

        return dispositivo;
    }

    public Dispositivo AgregarSensorMovimiento(CrearSensoresArgs args)
    {
        if (args.Empresa == null)
        {
            throw new ArgumentNullException(nameof(args.Empresa), "El dueño del dispositivo no tiene una empresa asociada.");
        }

        var empresa = repositorioEmpresa.ObtenerPorId(args.Empresa.Id);

        var dispositivo = new Dispositivo
        {
            Tipo = TipoDispositivo.SensorMovimiento,
            Nombre = args.Nombre,
            Modelo = args.Modelo,
            Descripcion = args.Descripcion,
            Fotografias = args.Fotografias,
            EmpresaId = empresa.Id,
            Empresa = empresa
        };

        ValidarDispositivo(dispositivo);

        repositorioDispositivo.Agregar(dispositivo);

        return dispositivo;
    }

    public Dispositivo AgregarCamara(CrearCamarasArgs args)
    {
        if (args.Empresa == null)
        {
            throw new ArgumentNullException(nameof(args.Empresa), "El dueño del dispositivo no tiene una empresa asociada.");
        }

        var empresa = repositorioEmpresa.ObtenerPorId(args.Empresa.Id);

        var dispositivo = new Dispositivo
        {
            Tipo = TipoDispositivo.Camara,
            Nombre = args.Nombre,
            Modelo = args.Modelo,
            Descripcion = args.Descripcion,
            Fotografias = args.Fotografias,
            EmpresaId = empresa.Id,
            Empresa = empresa,
            DetectaMovimiento = args.DetectaMovimiento,
            DetectaPersona = args.DetectaPersona,
            UsoExterior = args.UsoExterior,
            UsoInterior = args.UsoInterior
        };

        ValidarDispositivo(dispositivo);

        repositorioDispositivo.Agregar(dispositivo);

        return dispositivo;
    }

    public Dispositivo AgregarLampara(CrearLamparasArgs args)
    {
        if (args.Empresa == null)
        {
            throw new ArgumentNullException(nameof(args.Empresa), "El dueño del dispositivo no tiene una empresa asociada.");
        }

        var empresa = repositorioEmpresa.ObtenerPorId(args.Empresa.Id);

        var dispositivo = new Dispositivo
        {
            Tipo = TipoDispositivo.Lampara,
            Nombre = args.Nombre,
            Modelo = args.Modelo,
            Descripcion = args.Descripcion,
            Fotografias = args.Fotografias,
            EmpresaId = empresa.Id,
            Empresa = empresa
        };

        ValidarDispositivo(dispositivo);

        repositorioDispositivo.Agregar(dispositivo);

        return dispositivo;
    }

    private void ValidarDispositivo(Dispositivo dispositivo)
    {
        var yaExiste = EmpresaYaTieneDispositivo(dispositivo.Empresa.Id, dispositivo.Nombre, dispositivo.Modelo);

        if (yaExiste)
        {
            throw new ArgumentException("La empresa ya registró un dispositivo con el mismo nombre y número de modelo.");
        }

        if (!EsModeloValido(dispositivo.Modelo, dispositivo.Empresa.Validador))
        {
            throw new ArgumentException("El modelo del dispositivo fue rechazado por el validador de la empresa.");
        }
    }

    private bool EsModeloValido(string modelo, string identificadorValidador)
    {
        var directorioValidadores = BuscarDirectorioConCarpeta("Validadores");

        var administradorImplementacion = new AdministradorImplementacion<IModeloValidador>(directorioValidadores);
        var validador = administradorImplementacion.ObtenerImplementacionPorIdentificador(identificadorValidador);

        return validador.EsValido(new Modelo(modelo));
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

        throw new DirectoryNotFoundException($"No se pudo encontrar la carpeta '{carpetaObjetivo}' en la jerarquía de directorios a partir de '{AppDomain.CurrentDomain.BaseDirectory}'.");
    }

    public ObtenerDispositivosArgs ObtenerTodos(ParametroPaginacion? parametroPaginacion,
        ParametroDispositivoFiltro? parametroFiltro)
    {
        return repositorioDispositivo.ObtenerTodos(parametroPaginacion, parametroFiltro);
    }

    private bool EmpresaYaTieneDispositivo(Guid empresaId, string nombre, string modelo)
    {
        return repositorioDispositivo.Existe(x =>
            x.EmpresaId == empresaId && x.Nombre == nombre && x.Modelo == modelo);
    }

    public List<TipoDispositivo> ObtenerTiposDeDispositivos()
    {
        return repositorioDispositivo.ObtenerTiposDeDispositivos();
    }

    public Dispositivo ObtenerPorId(string id)
    {
        if (Guid.TryParse(id, out var idGuid))
        {
            var dispositivoExiste = repositorioDispositivo.Existe(d => d.Id == idGuid);

            if (!dispositivoExiste)
            {
                throw new ArgumentException("El dispositivo no existe.");
            }

            var dispositivo = repositorioDispositivo.ObtenerPorId(idGuid);

            return dispositivo;
        }

        throw new FormatException("El id del dispositivo no tiene el formato correto");
    }

    public void ImportarDispositivos(string ruta, string identificadorImportador, Empresa empresa)
    {
        if (empresa is null)
        {
            throw new InvalidOperationException("Solo usuarios con empresa pueden importar dispositivos");
        }

        var directorioImportadores = BuscarDirectorioConCarpeta("Importadores");

        var administradorImplementacion = new AdministradorImplementacion<IImportador>(directorioImportadores);

        var importador = administradorImplementacion.ObtenerImplementacionPorIdentificador(identificadorImportador);

        var dispositivosDto = importador.Importar(ruta);

        foreach (var dispositivoDto in dispositivosDto)
        {
            var tipo = Enum.Parse<TipoDispositivo>(dispositivoDto.Tipo);

            var fotografias = dispositivoDto.Fotografias?.Select(f => new FotografiaDispositivo
            {
                Url = f.Url ?? string.Empty,
                EsPrincipal = f.EsPrincipal
            }).ToList() ?? [];

            var dispositivo = new Dispositivo
            {
                Id = Guid.Parse(dispositivoDto.Id),
                Tipo = tipo,
                Nombre = dispositivoDto.Nombre,
                Modelo = dispositivoDto.Modelo,
                Descripcion = dispositivoDto.Descripcion ?? string.Empty,
                Fotografias = fotografias,
                Empresa = empresa,
                EmpresaId = empresa.Id,
                DetectaMovimiento = dispositivoDto.DetectaMovimiento,
                DetectaPersona = dispositivoDto.DetectaPersona,
                UsoExterior = dispositivoDto.UsoExterior,
                UsoInterior = dispositivoDto.UsoInterior
            };

            repositorioDispositivo.Agregar(dispositivo);
        }
    }

    public List<string> ObtenerIdentificadoresDeImportadores()
    {
        var directorioImportadores = BuscarDirectorioConCarpeta("Importadores");

        var administradorImplementacion = new AdministradorImplementacion<IImportador>(directorioImportadores);
        var identificadores = administradorImplementacion.ObtenerIdentificadores();

        return identificadores;
    }
}
