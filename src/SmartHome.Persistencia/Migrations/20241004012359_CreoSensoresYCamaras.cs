using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class CreoSensoresYCamaras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DetectaMovimiento",
                table: "Dispositivos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DetectaPersona",
                table: "Dispositivos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Dispositivos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "UsoExterior",
                table: "Dispositivos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UsoInterior",
                table: "Dispositivos",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetectaMovimiento",
                table: "Dispositivos");

            migrationBuilder.DropColumn(
                name: "DetectaPersona",
                table: "Dispositivos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Dispositivos");

            migrationBuilder.DropColumn(
                name: "UsoExterior",
                table: "Dispositivos");

            migrationBuilder.DropColumn(
                name: "UsoInterior",
                table: "Dispositivos");
        }
    }
}
