using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class AgregoMiebrosHogar : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "DueñoId",
            table: "Hogares",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateTable(
            name: "MiembroHogar",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MiembroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PermisoAsociarDispositivos = table.Column<bool>(type: "bit", nullable: false),
                PermisoListarDispositivos = table.Column<bool>(type: "bit", nullable: false),
                PermisoNotificaciones = table.Column<bool>(type: "bit", nullable: false),
                HogarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MiembroHogar", x => x.Id);
                table.ForeignKey(
                    name: "FK_MiembroHogar_Hogares_HogarId",
                    column: x => x.HogarId,
                    principalTable: "Hogares",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_MiembroHogar_Usuarios_MiembroId",
                    column: x => x.MiembroId,
                    principalTable: "Usuarios",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Hogares_DueñoId",
            table: "Hogares",
            column: "DueñoId");

        migrationBuilder.CreateIndex(
            name: "IX_MiembroHogar_HogarId",
            table: "MiembroHogar",
            column: "HogarId");

        migrationBuilder.CreateIndex(
            name: "IX_MiembroHogar_MiembroId",
            table: "MiembroHogar",
            column: "MiembroId");

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

        migrationBuilder.DropTable(
            name: "MiembroHogar");

        migrationBuilder.DropIndex(
            name: "IX_Hogares_DueñoId",
            table: "Hogares");

        migrationBuilder.DropColumn(
            name: "DueñoId",
            table: "Hogares");
    }
}
