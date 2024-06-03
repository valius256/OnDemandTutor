using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnDemandTutor.DataAccess.Models;

[Table("Blog")]
public partial class Blog
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public int CreateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateAt { get; set; }

    public int? UpdateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [ForeignKey("CreateBy")]
    [InverseProperty("BlogCreateByNavigations")]
    public virtual User CreateByNavigation { get; set; }

    [ForeignKey("UpdateBy")]
    [InverseProperty("BlogUpdateByNavigations")]
    public virtual User UpdateByNavigation { get; set; }
}
