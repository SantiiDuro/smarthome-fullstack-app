using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class ModificacionPermisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacb"),
                column: "Permisos",
                value: "[1,2,3,4,6]");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacc"),
                column: "Permisos",
                value: "[5]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacb"),
                column: "Permisos",
                value: "[1,2,3,4]");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacc"),
                column: "Permisos",
                value: "[5,6]");
        }
    }
}
