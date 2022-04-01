using Microsoft.EntityFrameworkCore.Migrations;

namespace Void.DAL.Migrations
{
    public partial class ReduceTickerCostScale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CostToMoveUpUsd",
                table: "Tickers",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "CostToMoveDownUsd",
                table: "Tickers",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldPrecision: 18,
                oldScale: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CostToMoveUpUsd",
                table: "Tickers",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "CostToMoveDownUsd",
                table: "Tickers",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,8)",
                oldPrecision: 18,
                oldScale: 8);
        }
    }
}
