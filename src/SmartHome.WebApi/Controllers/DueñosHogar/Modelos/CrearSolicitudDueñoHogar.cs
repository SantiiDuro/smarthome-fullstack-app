using System.Text.RegularExpressions;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.WebApi.Controllers.DueñosHogar.Modelos;

public sealed record CrearSolicitudDueñoHogar
{
    public string Nombre { get; init; } = null!;
    public string Apellido { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Contraseña { get; init; } = null!;
    public string FotoPerfil { get; init; } = null!;

    public CrearDueñosHogarArgs Args()
    {
        if (string.IsNullOrEmpty(Nombre))
        {
            throw new ArgumentNullException(nameof(Nombre));
        }

        if (string.IsNullOrEmpty(Apellido))
        {
            throw new ArgumentNullException(nameof(Apellido));
        }

        if (string.IsNullOrEmpty(Email))
        {
            throw new ArgumentNullException(nameof(Email));
        }

        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!emailRegex.IsMatch(Email))
        {
            throw new ArgumentException("El correo electrónico no tiene un formato válido.");
        }

        if (string.IsNullOrEmpty(Contraseña))
        {
            throw new ArgumentNullException(nameof(Contraseña));
        }

        var minCaracteres = 6;
        var caracteresEspeciales = new char[] { '!', '@', '#', '$', '%', '&', '*', '.' };
        if (Contraseña.Length < minCaracteres || !Contraseña.Any(c => caracteresEspeciales.Contains(c)))
        {
            throw new ArgumentException($"La contraseña debe tener al menos {minCaracteres} caracteres y un caracter especial (!@#$%&*.).");
        }

        if (string.IsNullOrEmpty(FotoPerfil))
        {
            throw new ArgumentNullException(nameof(FotoPerfil));
        }

        return new CrearDueñosHogarArgs(
            Nombre,
            Apellido,
            Email,
            Contraseña,
            FotoPerfil);
    }
}
