using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

public partial class PhysicalProduct
{
    [Key]
    [StringLength(8)]
    public string ProductID { get; set; } = null!;

    [Column(TypeName = "decimal(8, 3)")]
    public decimal? Weight { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Length { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Width { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Height { get; set; }

    public int DeliveryDays { get; set; }

    public byte ShippingFeeType { get; set; }

    [Column(TypeName = "money")]
    public decimal? FixedShippingFee { get; set; }

    public bool IsFragile { get; set; }

    [StringLength(200)]
    public string? PackagingNote { get; set; }

    [ForeignKey("ProductID")]
    [InverseProperty("PhysicalProduct")]
    public virtual Product Product { get; set; } = null!;
}
