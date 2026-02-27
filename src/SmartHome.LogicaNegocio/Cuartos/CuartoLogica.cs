using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Cuartos;
public sealed class CuartoLogica(ICuartoRepositorio repositorioCuarto, IHogarLogica logicaHogar)
    : ICuartoLogica
{
    private const string PermisoAdministrarCuartos = "AdministrarCuarto";

    public void GuardarCambios()
    {
        repositorioCuarto.GuardarCambios();
    }

    public Cuarto Agregar(CrearCuartosArgs args, Usuario usuario)
    {
        if (logicaHogar.VerificarPermiso(PermisoAdministrarCuartos, usuario, args.Hogar.Id.ToString()))
        {
            var existe = repositorioCuarto.Existe(c => c.HogarId == args.Hogar.Id && c.Nombre == args.Nombre);
            if (existe)
            {
                throw new InvalidOperationException("Ya existe un cuarto con el mismo nombre en el hogar");
            }

            var cuarto = new Cuarto
            {
                Nombre = args.Nombre,
                Hogar = args.Hogar,
                HogarId = args.Hogar.Id,
                DispositivosHogar = []
            };

            repositorioCuarto.Agregar(cuarto);
            GuardarCambios();

            return cuarto;
        }

        throw new InvalidOperationException("No tienes permiso para administrar los cuartos de este hogar");
    }

    public Cuarto ObtenerPorId(string id)
    {
        if (Guid.TryParse(id, out _))
        {
            var idGuid = Guid.Parse(id);
            if (repositorioCuarto.Existe(c => c.Id == idGuid))
            {
                return repositorioCuarto.ObtenerPorId(idGuid);
            }

            throw new KeyNotFoundException("No existe un cuarto con ese identificador");
        }

        throw new FormatException("Identificador de cuarto inválido");
    }
}
