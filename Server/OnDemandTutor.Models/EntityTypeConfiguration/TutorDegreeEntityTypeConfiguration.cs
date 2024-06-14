using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class TutorDegreeEntityTypeConfiguration : IEntityTypeConfiguration<TutorDegree>
    {
        public void Configure(EntityTypeBuilder<TutorDegree> builder)
        {
            builder.HasKey(td => td.Id);

            builder.Property(td => td.Description)
                .IsRequired();

            // Configure relationship with User (Tutor)
            builder.HasOne(td => td.Tutor)
                   .WithMany(u => u.TutorDegrees)
                   .HasForeignKey(td => td.TutorId)
                   .OnDelete(DeleteBehavior.Restrict); // Adjust DeleteBehavior if needed
        }
    }