using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class ClassEntityTypeConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        // Define properties
        builder.Property(c => c.Name).HasMaxLength(100);

        // Define relationships
        builder.HasOne(c => c.Subject)
            .WithMany(s => s.Class)
            .HasForeignKey(c => c.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.TutorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.StudentClasses)
            .WithOne(sc => sc.Class)
            .HasForeignKey(sc => sc.ClassId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasMany(c => c.Slots)
            .WithOne(s => s.Class)
            .HasForeignKey(s => s.ClassId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}