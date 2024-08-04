using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class ConsultationRequestEnttityTypeConfiguration : IEntityTypeConfiguration<ConsultationRequest>
{
    public void Configure(EntityTypeBuilder<ConsultationRequest> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.RequestDate).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.HandleById).IsRequired(false);

        builder.HasOne(cs => cs.HandleBy)
            .WithMany(u => u.Consultations)
            .HasForeignKey(cs => cs.HandleById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}