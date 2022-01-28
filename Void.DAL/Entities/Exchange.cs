using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Void.DAL.Entities
{
    public class Exchange
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ExchangeConfiguration : IEntityTypeConfiguration<Exchange>
    {
        public void Configure(EntityTypeBuilder<Exchange> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedNever()
                   .HasMaxLength(35);

            builder.Property(x => x.Name)
                   .HasMaxLength(30)
                   .IsRequired();
        }
    }
}
