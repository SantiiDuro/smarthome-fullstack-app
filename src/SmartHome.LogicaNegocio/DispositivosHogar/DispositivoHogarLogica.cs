using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.DispositivosHogar.Fabrica;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.DispositivosHogar;
public sealed class DispositivoHogarLogica
    (IDispositivoHogarRepositorio repositorioDispositivoHogar, IDispositivoRepositorio repositorioDispositivo, IHogarLogica logicaHogar)
    : IDispositivoHogarLogica
{
    private const string PermisoAdministrarCuartos = "AdministrarCuarto";
    private const string PermisoConexionDispositivo = "AsociarDispositivo";
    private const string PermisoModificarNombreDispositivo = "ModificarNombreDispositivo";
    private const string PermisoAsociarDispositivo = "AsociarDispositivo";
    private const string PermisoListarDispositivos = "ListarDispositivo";

    public void GuardarCambios()
    {
        repositorioDispositivoHogar.GuardarCambios();
    }

    public DispositivoHogar Agregar(CrearDispositivosHogarArgs args, Usuario usuario)
    {
        if (logicaHogar.VerificarPermiso(PermisoAsociarDispositivo, usuario, args.Hogar.Id.ToString()))
        {
            ValidarDispositivoYHogarExistentes(args);

            var dispositivoHogar = new DispositivoHogar
            {
                Nombre = args.Dispositivo.Nombre,
                Dispositivo = args.Dispositivo,
                DispositivoId = args.Dispositivo.Id,
                Hogar = args.Hogar,
                HogarId = args.Hogar.Id,
                EstaConectado = args.EstaConectado
            };

            if (args.Dispositivo.Tipo == TipoDispositivo.SensorVentana)
            {
                dispositivoHogar.EstaAbierto = false;
            }

            if (args.Dispositivo.Tipo == TipoDispositivo.Lampara)
            {
                dispositivoHogar.EstaEncendida = false;
            }

            repositorioDispositivoHogar.Agregar(dispositivoHogar);
            GuardarCambios();

            return dispositivoHogar;
        }

        throw new InvalidOperationException("No tienes permiso para agregar dispositivos a este hogar");
    }

    private void ValidarDispositivoYHogarExistentes(CrearDispositivosHogarArgs args)
    {
        if (!repositorioDispositivo.Existe(d => d.Id == args.Dispositivo.Id))
        {
            throw new InvalidOperationException("No existen dispositivos con ese identificador");
        }

        if (!logicaHogar.Existe(h => h.Id == args.Hogar.Id))
        {
            throw new InvalidOperationException("No existen hogares con ese identificador");
        }
    }

    public List<DispositivoHogar> ObtenerDispositivosDeHogar(string idHogar, Usuario usuario,
        ParametroDispositivoHogarFiltro? filtro)
    {
        if (Guid.TryParse(idHogar, out var idHogarGuid))
        {
            if (logicaHogar.VerificarPermiso(PermisoListarDispositivos, usuario, idHogar))
            {
                return repositorioDispositivoHogar
                    .ObtenerTodos(filtro)
                    .Where(dh => dh.HogarId == idHogarGuid)
                    .ToList();
            }

            throw new InvalidOperationException("No tienes permiso para listar los dispositivos de este hogar");
        }

        throw new FormatException("El id del hogar es incorrecto");
    }

    public DispositivoHogar ObtenerDispositivoHogarPorId(string hardwardId)
    {
        if (Guid.TryParse(hardwardId, out _))
        {
            var hardwardIdGuid = Guid.Parse(hardwardId);
            var parametroFiltro = new ParametroDispositivoHogarFiltro();
            var dispositivoHogar = repositorioDispositivoHogar
                .ObtenerTodos(parametroFiltro).FirstOrDefault(dh => dh.Id == hardwardIdGuid);
            if (dispositivoHogar is null)
            {
                throw new KeyNotFoundException("El hardwardId no existe en este hogar");
            }

            return dispositivoHogar;
        }

        throw new FormatException("El hardwardId es invalido");
    }

    public bool EjecutarOperacionDispositivo(string hardwardId, string operacion)
    {
        var dispositivoHogar = ObtenerDispositivoHogarPorId(hardwardId);
        var estrategia = DispositivoOperacionFactory.CrearOperacion(dispositivoHogar.Dispositivo.Tipo, operacion);

        var resultado = estrategia.EjecutarOperacion(dispositivoHogar);
        if (resultado)
        {
            repositorioDispositivoHogar.Actualizar(dispositivoHogar);
        }

        return resultado;
    }

    public void SensorDetectaMovimiento(string hardwardId)
    {
        var dispositivoHogar = ObtenerDispositivoHogarPorId(hardwardId);
        if (dispositivoHogar.Dispositivo.Tipo != TipoDispositivo.SensorMovimiento)
        {
            throw new InvalidOperationException("Comando válido solo para sensores de movimiento");
        }
    }

    public void CamaraDetectaMovimiento(string hardwardId)
    {
        var dispositivoHogar = ObtenerDispositivoHogarPorId(hardwardId);
        if (dispositivoHogar.Dispositivo.Tipo != TipoDispositivo.Camara ||
            dispositivoHogar.Dispositivo.DetectaMovimiento == false)
        {
            throw new InvalidOperationException("Comando válido solo para camaras que pueden detectar movimiento");
        }
    }

    public void CamaraDetectaPersona(string hardwardId)
    {
        var dispositivoHogar = ObtenerDispositivoHogarPorId(hardwardId);
        if (dispositivoHogar.Dispositivo.Tipo != TipoDispositivo.Camara ||
            dispositivoHogar.Dispositivo.DetectaPersona == false)
        {
            throw new InvalidOperationException("Comando válido solo para camaras que pueden detectar personas");
        }
    }

    public void Conectar(string dispositivoHogarId, Usuario usuario)
    {
        var dispositivoHogar = ObtenerDispositivoHogarPorId(dispositivoHogarId);

        if (logicaHogar.VerificarPermiso(PermisoConexionDispositivo, usuario, dispositivoHogar.HogarId.ToString()))
        {
            dispositivoHogar.EstaConectado = true;

            repositorioDispositivoHogar.Actualizar(dispositivoHogar);
        }
        else
        {
            throw new InvalidOperationException("No tienes permiso para conectar dispositivos en este hogar");
        }
    }

    public void Desconectar(string dispositivoHogarId, Usuario usuario)
    {
        var dispositivoHogar = ObtenerDispositivoHogarPorId(dispositivoHogarId);

        if (logicaHogar.VerificarPermiso(PermisoConexionDispositivo, usuario, dispositivoHogar.HogarId.ToString()))
        {
            dispositivoHogar.EstaConectado = false;

            repositorioDispositivoHogar.Actualizar(dispositivoHogar);
        }
        else
        {
            throw new InvalidOperationException("No tienes permiso para desconectar dispositivos en este hogar");
        }
    }

    public void AgregarACuarto(string dispositivoHogarId, Cuarto cuarto, Usuario usuario)
    {
        var dispositivoHogar = ObtenerDispositivoHogarPorId(dispositivoHogarId);

        if (dispositivoHogar.HogarId != cuarto.HogarId)
        {
            throw new InvalidOperationException("El dispositivo no pertenece al hogar asociado con el cuarto especificado.");
        }

        if (dispositivoHogar.CuartoId is not null)
        {
            throw new InvalidOperationException("El dispositivo ya está asignado a un cuarto y no puede reasignarse.");
        }

        if (logicaHogar.VerificarPermiso(PermisoAdministrarCuartos, usuario, cuarto.HogarId.ToString()))
        {
            dispositivoHogar.Cuarto = cuarto;
            dispositivoHogar.CuartoId = cuarto.Id;

            repositorioDispositivoHogar.Actualizar(dispositivoHogar);
        }
        else
        {
            throw new InvalidOperationException("No tienes permiso para administrar los cuartos de este hogar");
        }
    }

    public void ActualizarNombreDispositivoHogar(string hardwardId, string nombre, Usuario usuario)
    {
        var dispositivoHogar = ObtenerDispositivoHogarPorId(hardwardId);

        if (logicaHogar.VerificarPermiso(PermisoModificarNombreDispositivo, usuario, dispositivoHogar.HogarId.ToString()))
        {
            dispositivoHogar.Nombre = nombre;

            repositorioDispositivoHogar.Actualizar(dispositivoHogar);
        }
        else
        {
            throw new InvalidOperationException("No tienes permiso para modificar el nombre del dispositivos en este hogar");
        }
    }
}
