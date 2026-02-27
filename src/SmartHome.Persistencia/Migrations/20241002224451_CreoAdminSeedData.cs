using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class CreoAdminSeedData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Rol",
            columns: ["Id", "Permisos", "Tipo"],
            values: new object[,]
            {
                { new Guid("030c21ec-8635-48e3-af7e-68fda450daca"), "[0]", "dueño hogar" },
                { new Guid("030c21ec-8635-48e3-af7e-68fda450dacb"), "[1]", "administrador" }
            });

        migrationBuilder.InsertData(
            table: "Usuarios",
            columns: ["Id", "Apellido", "Contraseña", "Email", "FotoPerfil", "Nombre", "RolId"],
            values: [new Guid("030c21ec-8635-48e3-af7e-68fda450dacf"), "admin", "admin1234.", "admin@gmail.com", null, "admin", new Guid("030c21ec-8635-48e3-af7e-68fda450dacb")]);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Rol",
            keyColumn: "Id",
            keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450daca"));

        migrationBuilder.DeleteData(
            table: "Usuarios",
            keyColumn: "Id",
            keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacf"));

        migrationBuilder.DeleteData(
            table: "Rol",
            keyColumn: "Id",
            keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacb"));
    }
}
