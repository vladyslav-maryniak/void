using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Void.DAL.Entities
{
    public class Ticker
    {
        public int Id { get; set; }

        public string TargetCoinId { get; set; }
        public decimal Last { get; set; }
        public decimal CostToMoveUpUsd { get; set; }
        public decimal CostToMoveDownUsd { get; set; }
        public string TrustScore { get; set; }
        public double BidAskSpreadPercentage { get; set; }
        public bool IsStale { get; set; }
        public string TradeUrl { get; set; }
        public DateTime Timestamp { get; set; }

        public string CoinId { get; set; }
        public Coin Coin { get; set; }

        public string ExchangeId { get; set; }
        public Exchange Exchange { get; set; }
    }

    public class TickerConfiguration : IEntityTypeConfiguration<Ticker>
    {
        public void Configure(EntityTypeBuilder<Ticker> builder)
        {
            builder.Property(x => x.TargetCoinId)
                   .HasMaxLength(55)
                   .IsRequired();

            builder.Property(x => x.Last)
                   .HasPrecision(18, 10);

            builder.Property(x => x.CostToMoveUpUsd)
                   .HasPrecision(18, 8);

            builder.Property(x => x.CostToMoveDownUsd)
                   .HasPrecision(18, 8);

            builder.HasOne(x => x.Coin)
                   .WithMany()
                   .HasForeignKey(x => x.CoinId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.CoinId)
                   .HasMaxLength(55)
                   .IsRequired();

            builder.HasOne(x => x.Exchange)
                   .WithMany()
                   .HasForeignKey(x => x.ExchangeId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.ExchangeId)
                   .HasMaxLength(35)
                   .IsRequired();
        }
    }
}
