using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnDemandTutor.DataAccess.Models;

public partial class OnDemandTutorContext : IdentityDbContext<User>
{
    public OnDemandTutorContext()
    {
    }

    public OnDemandTutorContext(DbContextOptions<OnDemandTutorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassRequest> ClassRequests { get; set; }

    public virtual DbSet<ConsultationRequest> ConsultationRequests { get; set; }

    public virtual DbSet<FAQ> FAQs { get; set; }

    public virtual DbSet<Invitation> Invitations { get; set; }

    public virtual DbSet<Medium> Media { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TutorDegree> TutorDegrees { get; set; }

    public virtual DbSet<TutorTeachTimeSchedule> TutorTeachTimeSchedules { get; set; }

    public virtual DbSet<TutorVideo> TutorVideos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-O5NQD2D\\QUANGPHAT;Initial Catalog=OnDemandTutor;User ID=sa;Password=12345;Trust Server Certificate=True;Command Timeout=300");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

         modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(login => new { login.LoginProvider, login.ProviderKey });
         modelBuilder.Entity<IdentityUserRole<string>>().HasKey(userRole => new { userRole.UserId, userRole.RoleId });
         modelBuilder.Entity<IdentityUserToken<string>>().HasKey(userToken => new { userToken.UserId, userToken.LoginProvider, userToken.Name });
         modelBuilder.Entity<User>(entity =>
         {
            entity.HasKey(e => e.Id).HasName("PK_User");
            entity.Property(e => e.Phone).IsFixedLength();

            entity.HasOne(d => d.DegreeImage).WithMany(p => p.UserDegreeImages).HasConstraintName("FK_User_Media1");

            entity.HasOne(d => d.IdCardImage).WithMany(p => p.UserIdCardImages).HasConstraintName("FK_User_Media");

            
         });

        modelBuilder.HasDefaultSchema("identity");


        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.BlogCreateByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Blog_User");

            entity.HasOne(d => d.UpdateByNavigation).WithMany(p => p.BlogUpdateByNavigations).HasConstraintName("FK_Blog_User1");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasOne(d => d.Student).WithMany(p => p.ClassStudents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Class_User1");

            entity.HasOne(d => d.Subject).WithMany(p => p.Classes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Class_Subject");

            entity.HasOne(d => d.Tutor).WithMany(p => p.ClassTutors).HasConstraintName("FK_Class_User");
        });

        modelBuilder.Entity<ClassRequest>(entity =>
        {
            entity.HasOne(d => d.Class).WithMany(p => p.ClassRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassRequest_Class");

            entity.HasOne(d => d.Tutor).WithMany(p => p.ClassRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassRequest_User");
        });

        modelBuilder.Entity<FAQ>(entity =>
        {
            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.FAQs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FAQ_User");
        });

        modelBuilder.Entity<Invitation>(entity =>
        {
            entity.HasOne(d => d.Class).WithMany(p => p.Invitations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invitation_Class");

            entity.HasOne(d => d.Tutor).WithMany(p => p.Invitations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invitation_User");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasOne(d => d.Receiver).WithMany(p => p.Notifications).HasConstraintName("FK_Notification_User");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Lesson");

            entity.HasOne(d => d.Class).WithMany(p => p.Slots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lesson_Class");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TransactionCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_User1");

            entity.HasOne(d => d.Reference).WithMany(p => p.TransactionReferences)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_User");

            entity.HasOne(d => d.Slot).WithMany(p => p.Transactions).HasConstraintName("FK_Transaction_Slot");
        });

        modelBuilder.Entity<TutorDegree>(entity =>
        {
            entity.HasOne(d => d.DegreeImg).WithMany(p => p.TutorDegrees).HasConstraintName("FK_TutorDegree_Media");

            entity.HasOne(d => d.Tutor).WithMany(p => p.TutorDegrees).HasConstraintName("FK_TutorDegree_User");
        });

        modelBuilder.Entity<TutorTeachTimeSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Schedule");

            entity.HasOne(d => d.Tutor).WithMany(p => p.TutorTeachTimeSchedules).HasConstraintName("FK_TutorFreeTimeSchedule_User");
        });

        modelBuilder.Entity<TutorVideo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TutorVIdeo");

            entity.HasOne(d => d.Tutor).WithMany(p => p.TutorVideos).HasConstraintName("FK_TutorVIdeo_User");
        });

     
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
