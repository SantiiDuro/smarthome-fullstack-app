using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartHome.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class NuevosRolesAdminHoYBoHo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Permisos", "Tipo" },
                values: new object[,]
                {
                    { new Guid("030c21ec-8635-48e3-af7e-68fda450dacd"), "[1,2,3,4,6,0]", "administrador dueño hogar" },
                    { new Guid("030c21ec-8635-48e3-af7e-68fda450dace"), "[5,7,0]", "dueño empresa y hogar" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dacd"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("030c21ec-8635-48e3-af7e-68fda450dace"));
        }
    }
}
