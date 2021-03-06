// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Void.DAL;

namespace Void.DAL.Migrations
{
    [DbContext(typeof(VoidContext))]
    [Migration("20220109205051_AddTickerEntity")]
    partial class AddTickerEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Void.DAL.Entities.Coin", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(55)
                        .HasColumnType("nvarchar(55)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Coins");
                });

            modelBuilder.Entity("Void.DAL.Entities.Exchange", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Exchanges");
                });

            modelBuilder.Entity("Void.DAL.Entities.Ticker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("BidAskSpreadPercentage")
                        .HasColumnType("float");

                    b.Property<string>("CoinId")
                        .IsRequired()
                        .HasMaxLength(55)
                        .HasColumnType("nvarchar(55)");

                    b.Property<decimal>("CostToMoveDownUsd")
                        .HasPrecision(18, 10)
                        .HasColumnType("decimal(18,10)");

                    b.Property<decimal>("CostToMoveUpUsd")
                        .HasPrecision(18, 10)
                        .HasColumnType("decimal(18,10)");

                    b.Property<string>("ExchangeId")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<bool>("IsStale")
                        .HasColumnType("bit");

                    b.Property<decimal>("Last")
                        .HasPrecision(18, 10)
                        .HasColumnType("decimal(18,10)");

                    b.Property<string>("TargetCoinId")
                        .IsRequired()
                        .HasMaxLength(55)
                        .HasColumnType("nvarchar(55)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("TradeUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrustScore")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CoinId");

                    b.HasIndex("ExchangeId");

                    b.ToTable("Tickers");
                });

            modelBuilder.Entity("Void.DAL.Entities.Ticker", b =>
                {
                    b.HasOne("Void.DAL.Entities.Coin", "Coin")
                        .WithMany()
                        .HasForeignKey("CoinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Void.DAL.Entities.Exchange", "Exchange")
                        .WithMany()
                        .HasForeignKey("ExchangeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coin");

                    b.Navigation("Exchange");
                });
#pragma warning restore 612, 618
        }
    }
}
