using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class CambioRelacionDueñoHogar3 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Hogares_Usuarios_Id",
            table: "Hogares");

        migrationBuilder.AddColumn<Guid>(
            name: "DueñoId",
            table: "Hogares",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Hogares_Usuarios_DueñoId",
            table: "Hogares");

        migrationBuilder.DropIndex(
            name: "IX_Hogares_DueñoId",
            table: "Hogares");

        migrationBuilder.DropColumn(
            name: "DueñoId",
            table: "Hogares");

        migrationBuilder.AddForeignKey(
            name: "FK_Hogares_Usuarios_Id",
            table: "Hogares",
            column: "Id",
            principalTable: "Usuarios",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
