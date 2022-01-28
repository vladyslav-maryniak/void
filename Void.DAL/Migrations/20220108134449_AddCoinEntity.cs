using Microsoft.EntityFrameworkCore.Migrations;

namespace Void.DAL.Migrations
{
    public partial class AddCoinEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coins", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coins");
        }
    }
}
