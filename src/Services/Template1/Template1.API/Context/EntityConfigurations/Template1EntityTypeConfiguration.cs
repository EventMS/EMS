using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Template1_Services.API.Context.EntityConfigurations
{
    class Template1EntityTypeConfiguration
        : IEntityTypeConfiguration<Model.Template1>
    {
        public void Configure(EntityTypeBuilder<Model.Template1> builder)
        {
            builder.ToTable("Template1");

            builder.HasKey(ci => ci.Template1Id);

            builder.Property(ci => ci.Template1Id)
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired();
        }
    }
}
