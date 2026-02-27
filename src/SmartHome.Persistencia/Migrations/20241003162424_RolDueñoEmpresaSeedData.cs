using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class RolDueñoEmpresaSeedData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "EmpresaId",
            table: "Usuarios",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "NombreDueño",
            table: "Empresa",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.InsertData(
            table: "Roles",
            columns: ["Id", "Permisos", "Tipo"],
            values: [new Guid("030c21ec-8635-48e3-af7e-68fda450dacc"), "[5,6]", "dueño empresa"]);

        migrationBuilder.UpdateData(
            table: "Usuarios",
            keyColumn: "Id",
            keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacf"),
            column: "EmpresaId",
            value: null);

        migrationBuilder.CreateIndex(
            name: "IX_Usuarios_EmpresaId",
            table: "Usuarios",
            column: "EmpresaId");

        migrationBuilder.AddForeignKey(
            name: "FK_Usuarios_Empresa_EmpresaId",
            table: "Usuarios",
            column: "EmpresaId",
            principalTable: "Empresa",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Usuarios_Empresa_EmpresaId",
            table: "Usuarios");

        migrationBuilder.DropIndex(
            name: "IX_Usuarios_EmpresaId",
            table: "Usuarios");

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Id",
            keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacc"));

        migrationBuilder.DropColumn(
            name: "EmpresaId",
            table: "Usuarios");

        migrationBuilder.DropColumn(
            name: "NombreDueño",
            table: "Empresa");
    }
}
