using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "SecurityStamp",
                value: new Guid("c9734bea-2f45-4300-9ed2-dde6ea54ee08"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "SecurityStamp",
                value: new Guid("046cf5b4-0a9f-4083-ab5d-b73a9c0e9f0b"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "SecurityStamp",
                value: new Guid("2bbb8d5e-ff2b-439b-a63f-00e3a1aa2a13"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "SecurityStamp",
                value: new Guid("ae09b5db-1217-4c00-bf7d-a4531023d300"));
        }
    }
}
