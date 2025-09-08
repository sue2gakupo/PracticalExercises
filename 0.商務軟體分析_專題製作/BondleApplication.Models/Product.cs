using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

public partial class Product
{
    [Key]
    [StringLength(8)]
    public string ProductID { get; set; } = null!;

    [StringLength(100)]
    public string ProductName { get; set; } = null!;

    [Column(TypeName = "ntext")]
    public string? Description { get; set; }

    public byte ProductType { get; set; }

    [StringLength(8)]
    public string? CategoryID { get; set; }

    [StringLength(8)]
    public string? SeriesID { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    public byte Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LaunchDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OfflineDate { get; set; }

    public int SortOrder { get; set; }

    public int PurchaseCount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [StringLength(8)]
    public string CreatorID { get; set; } = null!;

    [ForeignKey("CategoryID")]
    [InverseProperty("Product")]
    public virtual Category? Category { get; set; }

    [ForeignKey("CreatorID")]
    [InverseProperty("Product")]
    public virtual Creator Creator { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual DigitalProduct? DigitalProduct { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Favorite> Favorite { get; set; } = new List<Favorite>();

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

    [InverseProperty("Product")]
    public virtual PhysicalProduct? PhysicalProduct { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<ProductVariations> ProductVariations { get; set; } = new List<ProductVariations>();

    [ForeignKey("SeriesID")]
    [InverseProperty("Product")]
    public virtual ProductSeries? Series { get; set; }
}
