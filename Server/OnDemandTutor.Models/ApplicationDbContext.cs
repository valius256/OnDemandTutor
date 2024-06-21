using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Models.EntityTypeConfiguration;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ConsultationRequest> ConsultationRequests { get; set; }

    public DbSet<FAQ> Faqs { get; set; }

    public DbSet<Notification> Notifications { get; set; }

    public DbSet<Slot> Slots { get; set; }

    public DbSet<Subject> Subjects { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<TutorDegree> TutorDegrees { get; set; }

    public DbSet<TutorVideo> TutorVideos { get; set; }
    public DbSet<SlotStudent> SlotStudents { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<StudentClass> StudentClasses { get; set; }
    public DbSet<TutorSubject> TutorSubjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SlotEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SlotStudentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new TutorDegreeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TutorVideoEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FaqEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BlogEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ClassEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SubjectEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StudentClassEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TutorSubjectEntityTypeConfiguration());
    }
}