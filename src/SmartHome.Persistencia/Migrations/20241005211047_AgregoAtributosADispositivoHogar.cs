using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class AgregoAtributosADispositivoHogar : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "NotificacionId",
            table: "MiembrosHogar",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Notificaciones",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DispositivoHogarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Evento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FueLeida = table.Column<bool>(type: "bit", nullable: false),
                FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notificaciones", x => x.Id);
                table.ForeignKey(
                    name: "FK_Notificaciones_DispositivosHogar_DispositivoHogarId",
                    column: x => x.DispositivoHogarId,
                    principalTable: "DispositivosHogar",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MiembrosHogar_NotificacionId",
            table: "MiembrosHogar",
            column: "NotificacionId");

        migrationBuilder.CreateIndex(
            name: "IX_Notificaciones_DispositivoHogarId",
            table: "Notificaciones",
            column: "DispositivoHogarId");

        migrationBuilder.AddForeignKey(
            name: "FK_MiembrosHogar_Notificaciones_NotificacionId",
            table: "MiembrosHogar",
            column: "NotificacionId",
            principalTable: "Notificaciones",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MiembrosHogar_Notificaciones_NotificacionId",
            table: "MiembrosHogar");

        migrationBuilder.DropTable(
            name: "Notificaciones");

        migrationBuilder.DropIndex(
            name: "IX_MiembrosHogar_NotificacionId",
            table: "MiembrosHogar");

        migrationBuilder.DropColumn(
            name: "NotificacionId",
            table: "MiembrosHogar");
    }
}
