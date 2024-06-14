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
          
            
            builder.HasOne(e => e.CreateByUser)
                .WithMany(u => u.BlogCreateBy)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.UpdateByUser)
                .WithMany(u => u.BlogUpdateBy)
                .HasForeignKey(e => e.UpdateBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}