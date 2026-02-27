using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class CambioAtributoEmpresaEnDispositivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Empresas_EmpresaId",
                table: "Dispositivos");

            migrationBuilder.DropIndex(
                name: "IX_Dispositivos_EmpresaId",
                table: "Dispositivos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_EmpresaId",
                table: "Dispositivos",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivos_Empresas_EmpresaId",
                table: "Dispositivos",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
