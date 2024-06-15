using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration
{
    public class ClassEntityTypeConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasKey(c => c.Id);

            // Define properties
            builder.Property(c => c.Name).HasMaxLength(100); // Adjust the maximum length as needed
            builder.Property(c => c.StudentName).HasMaxLength(100); // Adjust the maximum length as needed

            // Define relationships
            builder.HasOne(c => c.Subject)
                .WithMany(s => s.Class)
                .HasForeignKey(c => c.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.User)
                .WithMany() // Assuming User can have many classes
                .HasForeignKey(c => c.TutorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Students)
                .WithMany(u => u.Classes)
                .UsingEntity<StudentClass>(
                    j => j
                        .HasOne(sc => sc.Student)
                        .WithMany()
                        .HasForeignKey(sc => sc.StudentId),
                    j => j
                        .HasOne(sc => sc.Class)
                        .WithMany()
                        .HasForeignKey(sc => sc.ClassId)
                );

            builder.HasMany(c => c.Slots)
                .WithOne(s => s.Classes)
                .HasForeignKey(s => s.ClassId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
