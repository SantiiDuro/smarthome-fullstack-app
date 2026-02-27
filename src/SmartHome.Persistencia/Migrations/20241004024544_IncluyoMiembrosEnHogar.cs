using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class IncluyoMiembrosEnHogar : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MiembroHogar_Hogares_HogarId",
            table: "MiembroHogar");

        migrationBuilder.DropForeignKey(
            name: "FK_MiembroHogar_Usuarios_MiembroId",
            table: "MiembroHogar");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MiembroHogar",
            table: "MiembroHogar");

        migrationBuilder.RenameTable(
            name: "MiembroHogar",
            newName: "MiembrosHogar");

        migrationBuilder.RenameIndex(
            name: "IX_MiembroHogar_MiembroId",
            table: "MiembrosHogar",
            newName: "IX_MiembrosHogar_MiembroId");

        migrationBuilder.RenameIndex(
            name: "IX_MiembroHogar_HogarId",
            table: "MiembrosHogar",
            newName: "IX_MiembrosHogar_HogarId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_MiembrosHogar",
            table: "MiembrosHogar",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_MiembrosHogar_Hogares_HogarId",
            table: "MiembrosHogar",
            column: "HogarId",
            principalTable: "Hogares",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_MiembrosHogar_Usuarios_MiembroId",
            table: "MiembrosHogar",
            column: "MiembroId",
            principalTable: "Usuarios",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MiembrosHogar_Hogares_HogarId",
            table: "MiembrosHogar");

        migrationBuilder.DropForeignKey(
            name: "FK_MiembrosHogar_Usuarios_MiembroId",
            table: "MiembrosHogar");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MiembrosHogar",
            table: "MiembrosHogar");

        migrationBuilder.RenameTable(
            name: "MiembrosHogar",
            newName: "MiembroHogar");

        migrationBuilder.RenameIndex(
            name: "IX_MiembrosHogar_MiembroId",
            table: "MiembroHogar",
            newName: "IX_MiembroHogar_MiembroId");

        migrationBuilder.RenameIndex(
            name: "IX_MiembrosHogar_HogarId",
            table: "MiembroHogar",
            newName: "IX_MiembroHogar_HogarId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_MiembroHogar",
            table: "MiembroHogar",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_MiembroHogar_Hogares_HogarId",
            table: "MiembroHogar",
            column: "HogarId",
            principalTable: "Hogares",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_MiembroHogar_Usuarios_MiembroId",
            table: "MiembroHogar",
            column: "MiembroId",
            principalTable: "Usuarios",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
