using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class SacoClaseIntermedia : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MiembrosHogarNotificaciones");

        migrationBuilder.AddColumn<Guid>(
            name: "MiembroId",
            table: "Notificaciones",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateIndex(
            name: "IX_Notificaciones_MiembroId",
            table: "Notificaciones",
            column: "MiembroId");

        migrationBuilder.AddForeignKey(
            name: "FK_Notificaciones_MiembrosHogar_MiembroId",
            table: "Notificaciones",
            column: "MiembroId",
            principalTable: "MiembrosHogar",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Notificaciones_MiembrosHogar_MiembroId",
            table: "Notificaciones");

        migrationBuilder.DropIndex(
            name: "IX_Notificaciones_MiembroId",
            table: "Notificaciones");

        migrationBuilder.DropColumn(
            name: "MiembroId",
            table: "Notificaciones");

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
}
