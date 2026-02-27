using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class ClaseIntermedia : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MiembrosHogar_Notificaciones_NotificacionId",
            table: "MiembrosHogar");

        migrationBuilder.DropIndex(
            name: "IX_MiembrosHogar_NotificacionId",
            table: "MiembrosHogar");

        migrationBuilder.DropColumn(
            name: "NotificacionId",
            table: "MiembrosHogar");

        migrationBuilder.CreateTable(
            name: "MiembrosHogarNotificaciones",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MiembroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NotificacionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MiembrosHogarNotificaciones", x => x.Id);
                table.ForeignKey(
                    name: "FK_MiembrosHogarNotificaciones_MiembrosHogar_MiembroId",
                    column: x => x.MiembroId,
                    principalTable: "MiembrosHogar",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_MiembrosHogarNotificaciones_Notificaciones_NotificacionId",
                    column: x => x.NotificacionId,
                    principalTable: "Notificaciones",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_MiembrosHogarNotificaciones_MiembroId",
            table: "MiembrosHogarNotificaciones",
            column: "MiembroId");

        migrationBuilder.CreateIndex(
            name: "IX_MiembrosHogarNotificaciones_NotificacionId",
            table: "MiembrosHogarNotificaciones",
            column: "NotificacionId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MiembrosHogarNotificaciones");

        migrationBuilder.AddColumn<Guid>(
            name: "NotificacionId",
            table: "MiembrosHogar",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_MiembrosHogar_NotificacionId",
            table: "MiembrosHogar",
            column: "NotificacionId");

        migrationBuilder.AddForeignKey(
            name: "FK_MiembrosHogar_Notificaciones_NotificacionId",
            table: "MiembrosHogar",
            column: "NotificacionId",
            principalTable: "Notificaciones",
            principalColumn: "Id");
    }
}
