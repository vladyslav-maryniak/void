using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Void.DAL.Migrations
{
    public partial class AddTickerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetCoinId = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Last = table.Column<decimal>(type: "decimal(18,10)", precision: 18, scale: 10, nullable: false),
                    CostToMoveUpUsd = table.Column<decimal>(type: "decimal(18,10)", precision: 18, scale: 10, nullable: false),
                    CostToMoveDownUsd = table.Column<decimal>(type: "decimal(18,10)", precision: 18, scale: 10, nullable: false),
                    TrustScore = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidAskSpreadPercentage = table.Column<double>(type: "float", nullable: false),
                    IsStale = table.Column<bool>(type: "bit", nullable: false),
                    TradeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoinId = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    ExchangeId = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickers_Coins_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickers_Exchanges_ExchangeId",
                        column: x => x.ExchangeId,
                        principalTable: "Exchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickers_CoinId",
                table: "Tickers",
                column: "CoinId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickers_ExchangeId",
                table: "Tickers",
                column: "ExchangeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickers");
        }
    }
}
