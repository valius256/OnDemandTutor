using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class FaqEntityTypeConfiguration : IEntityTypeConfiguration<FAQ>
{
    public void Configure(EntityTypeBuilder<FAQ> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(f => f.Question)
            .IsRequired();

        builder.Property(f => f.Answer);

        builder.Property(f => f.CreateAt)
            .IsRequired()
            .HasColumnType("datetime");

        // Define relationships
        builder.HasOne<User>(f => f.CreateBy)
            .WithMany(u => u.FAQs)
            .HasForeignKey(f => f.CreateById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}