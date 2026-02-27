using Microsoft.EntityFrameworkCore;
using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Notificaciones.Entidades;
using SmartHome.LogicaNegocio.Sesiones.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.Persistencia;
public class ContextoSql
    : DbContext
{
    public DbSet<Dispositivo> Dispositivos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Hogar> Hogares { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<MiembroHogar> MiembrosHogar { get; set; }
    public DbSet<DispositivoHogar> DispositivosHogar { get; set; }
    public DbSet<Notificacion> Notificaciones { get; set; }
    public DbSet<Cuarto> Cuartos { get; set; }
    public DbSet<Sesion> Sesiones { get; set; }

    public ContextoSql(DbContextOptions opciones)
        : base(opciones)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigurarEsquema(modelBuilder);
        ConfigurarSeedData(modelBuilder);
    }

    private void ConfigurarEsquema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MiembroHogar>(entity =>
        {
            entity
            .HasMany(mh => mh.Notificaciones)
            .WithOne(n => n.Miembro)
            .HasForeignKey(n => n.MiembroId)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Hogar>(entity =>
        {
            entity
            .HasMany(h => h.Miembros)
            .WithOne(m => m.Hogar)
            .HasForeignKey(m => m.HogarId);
        });

        modelBuilder.Entity<Hogar>(entity =>
        {
            entity
            .HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(h => h.DueñoId)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Hogar>(entity =>
        {
            entity
            .HasMany(h => h.Cuartos)
            .WithOne(c => c.Hogar)
            .HasForeignKey(m => m.HogarId);
        });

        modelBuilder.Entity<Cuarto>(entity =>
        {
            entity
            .HasMany(c => c.DispositivosHogar)
            .WithOne(dh => dh.Cuarto)
            .HasForeignKey(m => m.CuartoId);
        });
    }

    private void ConfigurarSeedData(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Rol>()
            .HasData(
                RolesPredefinidos.DueñoHogar,
                RolesPredefinidos.Admin,
                RolesPredefinidos.DueñoEmpresa,
                RolesPredefinidos.AdminDueñoHogar,
                RolesPredefinidos.DueñoEmpresaYHogar);

        modelBuilder
            .Entity<Usuario>()
            .HasData(
                new Usuario
                {
                    Id = Guid.Parse("030c21ec-8635-48e3-af7e-68fda450dacf"),
                    Nombre = "admin",
                    Apellido = "admin",
                    Email = "admin@gmail.com",
                    Contraseña = "admin1234.",
                    RolId = RolesPredefinidos.ID_ADMIN
                });
    }
}
