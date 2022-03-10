using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Void.DAL.Migrations
{
    public partial class AddBlacklistedCoinEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlacklistedCoins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BlacklistedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistedCoins", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistedCoins");
        }
    }
}
