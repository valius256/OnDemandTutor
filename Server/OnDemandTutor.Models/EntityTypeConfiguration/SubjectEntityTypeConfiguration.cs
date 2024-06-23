using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class SubjectEntityTypeConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        // Define properties
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.SubjectType).IsRequired().HasMaxLength(50);
        builder.Property(s => s.Description).HasMaxLength(500);

        // Define relationships
        builder.HasOne(s => s.CreateBy)
            .WithMany(u => u.SubjectCreateBy)
            .HasForeignKey(s => s.CreateById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Class)
            .WithOne(c => c.Subject)
            .HasForeignKey(c => c.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Slots)
            .WithOne(sl => sl.Subject)
            .HasForeignKey(sl => sl.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.TutorDegree)
            .WithOne(s => s.Subject)
            .HasForeignKey(s => s.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}