using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class PasoModeloDeIntAString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroModelo",
                table: "Dispositivos");

            migrationBuilder.AddColumn<string>(
                name: "Modelo",
                table: "Dispositivos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modelo",
                table: "Dispositivos");

            migrationBuilder.AddColumn<int>(
                name: "NumeroModelo",
                table: "Dispositivos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
