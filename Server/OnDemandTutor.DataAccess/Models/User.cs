using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OnDemandTutor.DataAccess.Models;

[Table("User")]
public partial class User : IdentityUser
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [StringLength(10)]
    [AllowNull]
    public string Phone { get; set; }

    [StringLength(50)]
    public string Email { get; set; }

    [StringLength(100)]
    [AllowNull]
    public string Address { get; set; }

    public int? AvatarImageId { get; set; }

    public int? Status { get; set; }


    public DateTime? Dob { get; set; }

    public int? Role { get; set; }

    public bool? RecordStatus { get; set; }

    [Column(TypeName = "money")]
    public decimal? Balance { get; set; }

    public double? Rating { get; set; }

    public int? DegreeImageId { get; set; }

    public int? IdCardImageID { get; set; }

    public string ScheduleDesciption { get; set; }

    public virtual ICollection<Blog> BlogCreateByNavigations { get; set; } = new List<Blog>();


    public virtual ICollection<Blog> BlogUpdateByNavigations { get; set; } = new List<Blog>();


    public virtual ICollection<ClassRequest> ClassRequests { get; set; } = new List<ClassRequest>();


    public virtual ICollection<Class> ClassStudents { get; set; } = new List<Class>();


    public virtual ICollection<Class> ClassTutors { get; set; } = new List<Class>();


    public virtual Medium DegreeImage { get; set; }


    public virtual ICollection<FAQ> FAQs { get; set; } = new List<FAQ>();

    public virtual Medium IdCardImage { get; set; }


    public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();


    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();


    public virtual ICollection<Transaction> TransactionCreatedByNavigations { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionReferences { get; set; } = new List<Transaction>();


    public virtual ICollection<TutorDegree> TutorDegrees { get; set; } = new List<TutorDegree>();

    public virtual ICollection<TutorTeachTimeSchedule> TutorTeachTimeSchedules { get; set; } = new List<TutorTeachTimeSchedule>();


    public virtual ICollection<TutorVideo> TutorVideos { get; set; } = new List<TutorVideo>();
}
