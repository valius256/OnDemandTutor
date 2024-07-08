using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class TutorSubjectEntityTypeConfiguration : IEntityTypeConfiguration<TutorSubject>
{
    public void Configure(EntityTypeBuilder<TutorSubject> builder)
    {
        builder.HasKey(ts => ts.Id);
        builder.Property(ts => ts.Id).ValueGeneratedOnAdd();
            
        builder.HasOne(t => t.Subject).WithMany(t => t.TutorSubjects).HasForeignKey(ts => ts.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(t => t.User).WithMany(t => t.TutorSubjects).HasForeignKey(ts => ts.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}