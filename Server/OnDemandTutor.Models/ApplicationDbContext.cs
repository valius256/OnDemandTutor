using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<ConsultationRequest> ConsultationRequests { get; set; }

        public DbSet<FAQ> FAQs { get; set; }

        public DbSet<Invitation> Invitations { get; set; }

        public DbSet<Medium> Media { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Slot> Slots { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TutorDegree> TutorDegrees { get; set; }

        public DbSet<TutorTeachTimeSchedule> TutorTeachTimeSchedules { get; set; }

        public DbSet<TutorVideo> TutorVideos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>()
            .HasOne(c => c.Student)
            .WithMany(u => u.ClassStudents)
            .HasForeignKey(c => c.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Tutor)
                .WithMany(u => u.ClassTutors)
                .HasForeignKey(c => c.TutorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.CreatedByNavigation)
                .WithMany(u => u.TransactionCreatedByNavigations)
                .HasForeignKey(t => t.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ReferenceNavigation)
                .WithMany(u => u.TransactionReferences)
                .IsRequired()
                .HasForeignKey(t => t.ReferenceId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Slot>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Slots)
                .HasForeignKey(s => s.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseModel(ApplicationDbContextModel.Instance);
        }


    }
}
