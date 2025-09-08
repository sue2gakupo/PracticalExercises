using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

public partial class ProductSeries
{
    [Key]
    [StringLength(8)]
    public string SeriesID { get; set; } = null!;

    [StringLength(80)]
    public string SeriesName { get; set; } = null!;

    [Column(TypeName = "ntext")]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? CoverImageUrl { get; set; }

    [StringLength(200)]
    public string? Tags { get; set; }

    public int SortOrder { get; set; }

    public bool IsPublic { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [StringLength(8)]
    public string CreatorID { get; set; } = null!;

    [ForeignKey("CreatorID")]
    [InverseProperty("ProductSeries")]
    public virtual Creator Creator { get; set; } = null!;

    [InverseProperty("Series")]
    public virtual ICollection<Favorite> Favorite { get; set; } = new List<Favorite>();

    [InverseProperty("Series")]
    public virtual ICollection<Product> Product { get; set; } = new List<Product>();
}
