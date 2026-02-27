using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class NuevosPermisos : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "FechaCreacion",
            table: "Usuarios",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Roles",
            keyColumn: "Id",
            keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacb"),
            column: "Permisos",
            value: "[1,2,3,4]");

        migrationBuilder.UpdateData(
            table: "Usuarios",
            keyColumn: "Id",
            keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacf"),
            column: "FechaCreacion",
            value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "FechaCreacion",
            table: "Usuarios");

        migrationBuilder.UpdateData(
            table: "Roles",
            keyColumn: "Id",
            keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacb"),
            column: "Permisos",
            value: "[1,2]");
    }
}
