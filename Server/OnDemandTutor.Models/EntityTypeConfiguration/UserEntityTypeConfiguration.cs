using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.LastName).IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Phone).IsRequired(false).HasMaxLength(10);
            builder.Property(x => x.Role).IsRequired();
            builder.HasIndex(x => x.FireBaseid).IsUnique();

            // Configure relationships
            builder.HasMany(e => e.BlogCreateBy)
                .WithOne(b => b.CreateByUser)
                .HasForeignKey(b => b.CreateBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.BlogUpdateBy)
                .WithOne(b => b.UpdateByUser)
                .HasForeignKey(b => b.UpdateBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.SlotStudents)
                .WithOne(ss => ss.User)
                .HasForeignKey(ss => ss.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.FAQs)
                .WithOne(f => f.CreateByNavigation)
                .HasForeignKey(f => f.CreateBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.SlotStudents)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            builder.HasMany(u => u.Notifications)
                .WithOne(n => n.Receiver)
                .HasForeignKey(n => n.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.TransactionCreatedByNavigations)
                .WithOne(t => t.CreatedByNavigation)
                .HasForeignKey(t => t.CreatedBy);

            builder.HasMany(e => e.TransactionReferences)
                .WithOne(t => t.ReferenceNavigation)
                .HasForeignKey(t => t.ReferenceId);

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
                .WithOne(s => s.CreatedByNavigation)
                .OnDelete(DeleteBehavior.Restrict); // Adjust DeleteBehavior if needed

        }
    }
}