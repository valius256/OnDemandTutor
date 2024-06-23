using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class StudentClassEntityTypeConfiguration : IEntityTypeConfiguration<StudentClass>
{
    public void Configure(EntityTypeBuilder<StudentClass> builder)
    {
        builder.HasKey(cl => new { cl.StudentId, cl.ClassId });

        builder.HasOne(sc => sc.Class)
            .WithMany(c => c.StudentClasses)
            .HasForeignKey(sc => sc.ClassId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sc => sc.Student)
            .WithMany(s => s.StudentClasses)
            .HasForeignKey(sc => sc.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}