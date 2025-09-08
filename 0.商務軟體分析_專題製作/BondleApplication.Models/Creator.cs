using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

[Index("MemberID", Name = "UQ_Creator_Member", IsUnique = true)]
public partial class Creator
{
    [Key]
    [StringLength(8)]
    public string CreatorID { get; set; } = null!;

    [StringLength(50)]
    public string CreatorName { get; set; } = null!;

    [Column(TypeName = "ntext")]
    public string? Biography { get; set; }

    [StringLength(500)]
    public string? AvatarUrl { get; set; }

    [StringLength(500)]
    public string? CoverUrl { get; set; }

    public byte? VerificationStatus { get; set; }

    [StringLength(50)]
    public string? BankAccount { get; set; }

    [StringLength(10)]
    public string? BankCode { get; set; }

    [StringLength(50)]
    public string? AccountHolderName { get; set; }

    [StringLength(20)]
    public string? ECPayMerchantID { get; set; }

    public byte? ECPayStatus { get; set; }

    [Column(TypeName = "decimal(5, 3)")]
    public decimal PlatformFeeRate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ApplyDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? VerificationDate { get; set; }

    public byte Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [StringLength(36)]
    public string MemberID { get; set; } = null!;

    [ForeignKey("MemberID")]
    [InverseProperty("Creator")]
    public virtual Member Member { get; set; } = null!;

    [InverseProperty("Creator")]
    public virtual ICollection<Order> Order { get; set; } = new List<Order>();

    [InverseProperty("Creator")]
    public virtual ICollection<Product> Product { get; set; } = new List<Product>();

    [InverseProperty("Creator")]
    public virtual ICollection<ProductSeries> ProductSeries { get; set; } = new List<ProductSeries>();
}
