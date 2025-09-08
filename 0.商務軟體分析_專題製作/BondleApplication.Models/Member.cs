using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

[Index("Email", Name = "UQ_Member_Email", IsUnique = true)]
public partial class Member
{
    [Key]
    [StringLength(36)]
    public string MemberID { get; set; } = null!;

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(512)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(100)]
    public string? GoogleUserID { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }

    public bool IsEmailVerified { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLoginDate { get; set; }

    public byte Status { get; set; }

    [InverseProperty("Member")]
    public virtual Creator? Creator { get; set; }

    [InverseProperty("Member")]
    public virtual Supporter? Supporter { get; set; }
}
