using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class CreoDispositivos : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Empresa",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Logotipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Rut = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Empresa", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Dispositivos",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                NumeroModelo = table.Column<int>(type: "int", nullable: false),
                Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Dispositivos", x => x.Id);
                table.ForeignKey(
                    name: "FK_Dispositivos_Empresa_EmpresaId",
                    column: x => x.EmpresaId,
                    principalTable: "Empresa",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "FotografiaDispositivo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                EsPrincipal = table.Column<bool>(type: "bit", nullable: false),
                DispositivoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FotografiaDispositivo", x => x.Id);
                table.ForeignKey(
                    name: "FK_FotografiaDispositivo_Dispositivos_DispositivoId",
                    column: x => x.DispositivoId,
                    principalTable: "Dispositivos",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Dispositivos_EmpresaId",
            table: "Dispositivos",
            column: "EmpresaId");

        migrationBuilder.CreateIndex(
            name: "IX_FotografiaDispositivo_DispositivoId",
            table: "FotografiaDispositivo",
            column: "DispositivoId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FotografiaDispositivo");

        migrationBuilder.DropTable(
            name: "Dispositivos");

        migrationBuilder.DropTable(
            name: "Empresa");
    }
}
