using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration
{
    public class ConsultationRequestEnttityTypeConfiguration : IEntityTypeConfiguration<ConsultationRequest>
    {
        public void Configure(EntityTypeBuilder<ConsultationRequest> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.ReasonFailed).IsRequired(false);
        }
    }
}
