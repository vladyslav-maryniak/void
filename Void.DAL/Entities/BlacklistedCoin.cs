using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Void.DAL.Entities
{
    public class BlacklistedCoin
    {
        public string Id { get; set; }
        public string Reason { get; set; }
        public DateTime BlacklistedAt { get; set; }
    }

    public class BlacklistedCoinConfiguration : IEntityTypeConfiguration<BlacklistedCoin>
    {
        public void Configure(EntityTypeBuilder<BlacklistedCoin> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedNever()
                   .HasMaxLength(55);

            builder.Property(x => x.Reason)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(x => x.BlacklistedAt)
                   .IsRequired();
        }
    }
}
