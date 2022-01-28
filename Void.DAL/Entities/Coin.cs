using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Void.DAL.Entities
{
    public class Coin
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }

    public class CoinConfiguration : IEntityTypeConfiguration<Coin>
    {
        public void Configure(EntityTypeBuilder<Coin> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedNever()
                   .HasMaxLength(55);

            builder.Property(x => x.Symbol)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.Name)
                   .HasMaxLength(60)
                   .IsRequired();
        }
    }
}
