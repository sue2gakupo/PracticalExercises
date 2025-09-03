using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

[Index("ProductID", Name = "IX_ProductVariations_ProductID")]
[Index("SKU", Name = "IX_ProductVariations_SKU")]
public partial class ProductVariations
{
    [Key]
    [StringLength(8)]
    public string VariationID { get; set; } = null!;

    [StringLength(8)]
    public string ProductID { get; set; } = null!;

    [StringLength(50)]
    public string SKU { get; set; } = null!;

    [StringLength(50)]
    public string VariationName { get; set; } = null!;

    [StringLength(30)]
    public string? Color { get; set; }

    [StringLength(20)]
    public string? Size { get; set; }

    [StringLength(30)]
    public string? Material { get; set; }

    [StringLength(20)]
    public string? Edition { get; set; }

    [Column(TypeName = "money")]
    public decimal? PriceDifference { get; set; }

    public int Stock { get; set; }

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }

    public bool IsDefault { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [InverseProperty("Variation")]
    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

    [ForeignKey("ProductID")]
    [InverseProperty("ProductVariations")]
    public virtual Product Product { get; set; } = null!;

    [InverseProperty("Variation")]
    public virtual ICollection<ProductImages> ProductImages { get; set; } = new List<ProductImages>();
}
