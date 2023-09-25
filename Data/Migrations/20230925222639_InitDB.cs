using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "Email", "FullName", "IsActive", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[,]
                {
                    { 1, 30, "sample1@example.com", "Sample User One", true, "8jr0nptvznD5VS2WniCx5y6jYyQOSw1ZpfsulA8c/3A=", new Guid("0855b139-340a-47a5-9a24-a24b62886619"), "ali" },
                    { 2, 25, "sample2@example.com", "Sample User Two", true, "8jr0nptvznD5VS2WniCx5y6jYyQOSw1ZpfsulA8c/3A=", new Guid("68ff43f9-ff39-4b44-91fd-abd255efc354"), "ata" }
                });

            migrationBuilder.InsertData(
                table: "RefreshTokens",
                columns: new[] { "Id", "ExpiresAt", "IsRevoked", "IsUsed", "IssuedAt", "Token", "UserId" },
                values: new object[,]
                {
                    { 2, new DateTime(2023, 10, 11, 1, 31, 34, 443, DateTimeKind.Unspecified).AddTicks(3333), false, false, new DateTime(2023, 9, 26, 1, 31, 34, 443, DateTimeKind.Unspecified).AddTicks(3333), "D66709F8-27E9-4164-AF91-CA0877F10FEF", 1 },
                    { 3, new DateTime(2023, 10, 11, 1, 31, 34, 443, DateTimeKind.Unspecified).AddTicks(3333), false, false, new DateTime(2023, 9, 26, 1, 31, 34, 443, DateTimeKind.Unspecified).AddTicks(3333), "67C8F733-BF6A-4E49-8109-522DACCC60F7", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
