using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class CambioRelacionDueñoHogar4 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Hogares_Usuarios_DueñoId",
            table: "Hogares");

        migrationBuilder.DropIndex(
            name: "IX_Hogares_DueñoId",
            table: "Hogares");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_Hogares_DueñoId",
            table: "Hogares",
            column: "DueñoId");

        migrationBuilder.AddForeignKey(
            name: "FK_Hogares_Usuarios_DueñoId",
            table: "Hogares",
            column: "DueñoId",
            principalTable: "Usuarios",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
