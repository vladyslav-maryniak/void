using Microsoft.EntityFrameworkCore.Migrations;

namespace Void.DAL.Migrations
{
    public partial class AddExchangeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exchanges",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchanges", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exchanges");
        }
    }
}
