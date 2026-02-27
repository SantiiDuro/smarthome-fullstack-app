using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations;

/// <inheritdoc />
public partial class Relacion1NHogarMiembros : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MiembrosHogar_Hogares_HogarId",
            table: "MiembrosHogar");

        migrationBuilder.AlterColumn<Guid>(
            name: "HogarId",
            table: "MiembrosHogar",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_MiembrosHogar_Hogares_HogarId",
            table: "MiembrosHogar",
            column: "HogarId",
            principalTable: "Hogares",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MiembrosHogar_Hogares_HogarId",
            table: "MiembrosHogar");

        migrationBuilder.AlterColumn<Guid>(
            name: "HogarId",
            table: "MiembrosHogar",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddForeignKey(
            name: "FK_MiembrosHogar_Hogares_HogarId",
            table: "MiembrosHogar",
            column: "HogarId",
            principalTable: "Hogares",
            principalColumn: "Id");
    }
}
