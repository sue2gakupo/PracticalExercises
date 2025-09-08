using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

[Index("MemberID", Name = "UQ_Supporter_Member", IsUnique = true)]
public partial class Supporter
{
    [Key]
    [StringLength(8)]
    public string SupporterID { get; set; } = null!;

    [StringLength(50)]
    public string? SupporterName { get; set; }

    [StringLength(500)]
    public string? AvatarUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [StringLength(36)]
    public string MemberID { get; set; } = null!;

    [InverseProperty("Supporter")]
    public virtual ICollection<Favorite> Favorite { get; set; } = new List<Favorite>();

    [ForeignKey("MemberID")]
    [InverseProperty("Supporter")]
    public virtual Member Member { get; set; } = null!;

    [InverseProperty("Supporter")]
    public virtual ICollection<Order> Order { get; set; } = new List<Order>();

    [InverseProperty("Supporter")]
    public virtual ICollection<ShippingAddress> ShippingAddress { get; set; } = new List<ShippingAddress>();
}
