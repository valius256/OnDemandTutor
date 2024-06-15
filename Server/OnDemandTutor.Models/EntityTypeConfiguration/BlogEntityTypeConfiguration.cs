using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration
{
    public class BlogEntityTypeConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Content).IsRequired();




            builder.HasOne(b => b.CreateBy)
                 .WithMany(u => u.BlogCreateBy)
                 .HasForeignKey(b => b.CreateById)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.UpdateBy)
                .WithMany(u => u.BlogUpdateBy)
                .HasForeignKey(b => b.UpdateById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}