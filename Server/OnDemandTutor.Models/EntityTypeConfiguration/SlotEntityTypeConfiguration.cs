using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration
{
    public class SlotEntityTypeConfiguration : IEntityTypeConfiguration<Slot>
    {
        public void Configure(EntityTypeBuilder<Slot> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.StartTime)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(s => s.EndTime)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(s => s.TeachAddress)
                .HasMaxLength(100);

            builder.Property(s => s.IsOnline)
                .IsRequired();

            builder.Property(s => s.NumberOfStudents)
                .IsRequired();

            builder.Property(s => s.PaymentStatus)
                .IsRequired();
            
            builder.Property(s => s.SubjectId)
                .IsRequired(false)
                .HasColumnType("integer");
            

            builder.Property(s => s.ActualEndTime)
                .HasColumnType("datetime");

            // Define relationships

            builder.HasMany(s => s.SlotStudents).WithOne(s => s.Slot).OnDelete(DeleteBehavior.NoAction);
            
            builder.HasMany(s => s.SlotStudents) 
                .WithOne(ss => ss.Slot)
                .HasForeignKey(ss => ss.SlotId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(s => s.SlotTransactionNavigation)
                .WithOne(t => t.Slot)
                .HasForeignKey(t => t.ReferenceId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
