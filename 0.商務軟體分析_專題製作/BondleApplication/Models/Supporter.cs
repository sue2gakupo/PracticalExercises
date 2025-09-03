using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

public partial class Supporter
{
    [Key]
    [StringLength(36)]
    public string MemberID { get; set; } = null!;

    [StringLength(50)]
    public string? SupporterName { get; set; }

    [StringLength(500)]
    public string? AvatarUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [ForeignKey("MemberID")]
    [InverseProperty("Supporter")]
    public virtual Member Member { get; set; } = null!;
}
