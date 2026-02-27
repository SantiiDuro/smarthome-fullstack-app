using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class NombreDispositivoHogar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PermisoModificarNombreDispositivos",
                table: "MiembrosHogar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "DispositivosHogar",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispositivosHogar_Cuartos_CuartoId",
                table: "DispositivosHogar");

            migrationBuilder.DropTable(
                name: "Cuartos");

            migrationBuilder.DropIndex(
                name: "IX_DispositivosHogar_CuartoId",
                table: "DispositivosHogar");

            migrationBuilder.DropColumn(
                name: "PermisoAdministrarCuartos",
                table: "MiembrosHogar");

            migrationBuilder.DropColumn(
                name: "PermisoModificarNombreDispositivos",
                table: "MiembrosHogar");

            migrationBuilder.DropColumn(
                name: "CuartoId",
                table: "DispositivosHogar");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "DispositivosHogar");
        }
    }
}
