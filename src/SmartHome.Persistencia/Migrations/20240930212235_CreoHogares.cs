using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class CreoHogares : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Hogares",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Calle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                NumPuerta = table.Column<int>(type: "int", nullable: false),
                Latitud = table.Column<int>(type: "int", nullable: false),
                Longitud = table.Column<int>(type: "int", nullable: false),
                CantMiembrosSoportados = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Hogares", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Hogares");
    }
}
