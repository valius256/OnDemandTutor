using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class TutorVideoEntityTypeConfiguration : IEntityTypeConfiguration<TutorVideo>
{
    public void Configure(EntityTypeBuilder<TutorVideo> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}