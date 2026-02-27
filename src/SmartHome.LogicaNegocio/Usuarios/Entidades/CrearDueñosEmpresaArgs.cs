using System.Text.RegularExpressions;
using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.LogicaNegocio.Usuarios.Entidades;
public sealed class CrearDueñosEmpresaArgs
{
    public readonly string Nombre = null!;
    public readonly string Apellido = null!;
    public readonly string Email = null!;
    public readonly string Contraseña = null!;
    public readonly Empresa? Empresa;

    public CrearDueñosEmpresaArgs(
        string nombre,
        string apellido,
        string email,
        string contraseña)
    {
        if (string.IsNullOrEmpty(nombre))
        {
            throw new ArgumentNullException(nameof(nombre));
        }

        if (string.IsNullOrEmpty(apellido))
        {
            throw new ArgumentNullException(nameof(apellido));
        }

        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email));
        }

        if (string.IsNullOrEmpty(contraseña))
        {
            throw new ArgumentNullException(nameof(contraseña));
        }

        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!emailRegex.IsMatch(email))
        {
            throw new ArgumentException("El correo electrónico no tiene un formato válido.");
        }

        var minCaracteres = 6;
        var caracteresEspeciales = new char[] { '!', '@', '#', '$', '%', '&', '*', '.' };
        if (contraseña.Length < minCaracteres || !contraseña.Any(c => caracteresEspeciales.Contains(c)))
        {
            throw new ArgumentException($"La contraseña debe tener al menos {minCaracteres} caracteres y un caracter especial (!@#$%&*.).");
        }

        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        Contraseña = contraseña;
        Empresa = null;
    }
}
