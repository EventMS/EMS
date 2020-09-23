using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Club.API.Context.EntityConfigurations
{
    class ClubEntityTypeConfiguration
        : IEntityTypeConfiguration<Model.Club>
    {
        public void Configure(EntityTypeBuilder<Model.Club> builder)
        {
            builder.ToTable("Club");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired();
        }
    }
}
