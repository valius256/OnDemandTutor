using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class RequestWithDrawEntityTypeConfiguration : IEntityTypeConfiguration<RequestWithDraw>
{
    public void Configure(EntityTypeBuilder<RequestWithDraw> builder)
    {
        builder.HasKey(ld => ld.Id);
        builder.Property(ld => ld.Id).ValueGeneratedOnAdd();

        builder.Property(r => r.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(r => r.BankAccountNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.BankName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(r => r.Reply)
            .HasMaxLength(500)
            .IsRequired(false);


        builder.HasOne(r => r.User)
            .WithMany(u => u.RequestWithDraw)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Operator)
            .WithMany(u => u.OpearateBy)
            .HasForeignKey(r => r.OperatorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}