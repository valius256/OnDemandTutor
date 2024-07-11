using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.FirstName).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Password).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Phone).IsRequired(false).HasMaxLength(10);
        builder.Property(x => x.Role).IsRequired();
        builder.HasIndex(x => x.FireBaseid).IsUnique();
        builder.Property(x => x.Balance).HasColumnType("money").IsRequired(false);
        builder.Property(x => x.TutorFeePerHour).HasColumnType("money").IsRequired(false);
        builder.Property(x => x.AvatarImageUrl).HasMaxLength(1000);
        builder.Property(x => x.IdCardImageUrl).HasMaxLength(1000);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.DeaActiveReason).IsRequired(false);
        builder.Property(x => x.CreatedDate).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.Balance).HasDefaultValue(0);
        // default will query the user with active status
        // builder.HasQueryFilter(x => x.IsActive);


        // Configure relationships
        builder.HasMany(e => e.BlogCreateBy)
            .WithOne(b => b.CreateBy)
            .HasForeignKey(b => b.CreateById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.BlogUpdateBy)
            .WithOne(b => b.UpdateBy)
            .HasForeignKey(b => b.UpdateById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.SlotStudents)
            .WithOne(ss => ss.User)
            .HasForeignKey(ss => ss.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.FAQs)
            .WithOne(f => f.CreateBy)
            .HasForeignKey(f => f.CreateById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.SlotStudents)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);

        builder.HasMany(u => u.Notifications)
            .WithOne(n => n.Receiver)
            .HasForeignKey(n => n.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.TransactionCreatedBy)
            .WithOne(t => t.CreatedBy)
            .HasForeignKey(t => t.CreatedById);

        builder.HasMany(u => u.TutorDegrees)
            .WithOne(td => td.Tutor)
            .HasForeignKey(td => td.TutorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.TutorDegrees)
            .WithOne(td => td.Tutor)
            .HasForeignKey(td => td.TutorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.SlotStudents)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

        // Configure relationship with Slots
        builder.HasMany(u => u.Slots)
            .WithOne(s => s.CreatedBy)
            .HasForeignKey(s => s.CreateById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.SubjectCreateBy)
            .WithOne(s => s.CreateBy)
            .HasForeignKey(s => s.CreateById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Consultations)
            .WithOne(u => u.HandleBy)
            .HasForeignKey(u => u.HandleById)
            .OnDelete(DeleteBehavior.Restrict);

    }
}