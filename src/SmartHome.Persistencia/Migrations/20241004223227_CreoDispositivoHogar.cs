using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class CreoDispositivoHogar : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DispositivosHogar",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DispositivoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                HogarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EstaConectado = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DispositivosHogar", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DispositivosHogar");
    }
}
