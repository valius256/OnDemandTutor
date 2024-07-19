using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class SlotStudentEntityTypeConfiguration : IEntityTypeConfiguration<SlotStudent>
{
    public void Configure(EntityTypeBuilder<SlotStudent> builder)
    {
        builder.HasKey(ss => ss.Id);
        builder.Property(ss => ss.Id).ValueGeneratedOnAdd();
        builder.Property(ss => ss.PaymentStatus).HasDefaultValue(PaymentStatus.Notpaid);
        builder.Property(ss => ss.Rating).IsRequired(false);
        builder.Property(ss => ss.Feedback).IsRequired(false);
        builder.HasOne(ss => ss.Slot)
            .WithMany(s => s.SlotStudents)
            .HasForeignKey(ss => ss.SlotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ss => ss.User)
            .WithMany(u => u.SlotStudents)
            .HasForeignKey(ss => ss.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}