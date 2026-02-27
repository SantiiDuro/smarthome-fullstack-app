using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class EspecificoClavesForaneas : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Roles",
            keyColumn: "Id",
            keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacc"),
            column: "Permisos",
            value: "[5,7]");

        migrationBuilder.CreateIndex(
            name: "IX_Hogares_DueñoId",
            table: "Hogares",
            column: "DueñoId");

        migrationBuilder.CreateIndex(
            name: "IX_DispositivosHogar_DispositivoId",
            table: "DispositivosHogar",
            column: "DispositivoId");

        migrationBuilder.CreateIndex(
            name: "IX_DispositivosHogar_HogarId",
            table: "DispositivosHogar",
            column: "HogarId");

        migrationBuilder.AddForeignKey(
            name: "FK_DispositivosHogar_Dispositivos_DispositivoId",
            table: "DispositivosHogar",
            column: "DispositivoId",
            principalTable: "Dispositivos",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_DispositivosHogar_Hogares_HogarId",
            table: "DispositivosHogar",
            column: "HogarId",
            principalTable: "Hogares",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Hogares_Usuarios_DueñoId",
            table: "Hogares",
            column: "DueñoId",
            principalTable: "Usuarios",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_DispositivosHogar_Dispositivos_DispositivoId",
            table: "DispositivosHogar");

        migrationBuilder.DropForeignKey(
            name: "FK_DispositivosHogar_Hogares_HogarId",
            table: "DispositivosHogar");

        migrationBuilder.DropForeignKey(
            name: "FK_Hogares_Usuarios_DueñoId",
            table: "Hogares");

        migrationBuilder.DropIndex(
            name: "IX_Hogares_DueñoId",
            table: "Hogares");

        migrationBuilder.DropIndex(
            name: "IX_DispositivosHogar_DispositivoId",
            table: "DispositivosHogar");

        migrationBuilder.DropIndex(
            name: "IX_DispositivosHogar_HogarId",
            table: "DispositivosHogar");

        migrationBuilder.UpdateData(
            table: "Roles",
            keyColumn: "Id",
            keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacc"),
            column: "Permisos",
            value: "[5]");
    }
}
