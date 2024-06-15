using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models.EntityTypeConfiguration;

public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Content)
            .IsRequired();

        builder.Property(n => n.RefUrl);

        builder.Property(n => n.RefImageUrl);

        builder.Property(n => n.ViewStatus)
            .IsRequired();

        // Configure relationship with User
        builder.HasOne<User>(n => n.Receiver)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.ReceiverId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}