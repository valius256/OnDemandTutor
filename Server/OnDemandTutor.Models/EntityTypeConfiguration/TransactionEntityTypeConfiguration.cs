using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(t => t.TransactionCode)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(t => t.SlotId)
            .IsRequired(false);
        
        builder.Property(t => t.PaymentMethod)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(t => t.Amount)
            .HasColumnType("money")
            .IsRequired();

        builder.Property(t => t.CreatedDate)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(t => t.Notes)
            .IsRequired(false);

        builder.HasOne(t => t.CreatedBy)
            .WithMany(u => u.TransactionCreatedBy)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
        
       
        builder.HasOne(t => t.Slot)
            .WithMany(s => s.SlotTransaction)
            .HasForeignKey(t => t.SlotId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}