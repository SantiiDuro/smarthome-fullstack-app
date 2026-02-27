using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class AgregoCuartos : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "PermisoAdministrarCuartos",
            table: "MiembrosHogar",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<Guid>(
            name: "CuartoId",
            table: "DispositivosHogar",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Cuartos",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                HogarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cuartos", x => x.Id);
                table.ForeignKey(
                    name: "FK_Cuartos_Hogares_HogarId",
                    column: x => x.HogarId,
                    principalTable: "Hogares",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_DispositivosHogar_CuartoId",
            table: "DispositivosHogar",
            column: "CuartoId");

        migrationBuilder.CreateIndex(
            name: "IX_Cuartos_HogarId",
            table: "Cuartos",
            column: "HogarId");

        migrationBuilder.AddForeignKey(
            name: "FK_DispositivosHogar_Cuartos_CuartoId",
            table: "DispositivosHogar",
            column: "CuartoId",
            principalTable: "Cuartos",
            principalColumn: "Id");
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
            name: "CuartoId",
            table: "DispositivosHogar");
    }
}
