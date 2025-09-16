using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

public partial class ProductImages
{
    [Key]
    [StringLength(8)]
    public string ImageID { get; set; } = null!;

    [StringLength(500)]
    public string ImageUrl { get; set; } = null!;

    public int SortOrder { get; set; }

    [StringLength(100)]
    public string? ImageCaption { get; set; }

    public long FileSize { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [StringLength(8)]
    public string VariationID { get; set; } = null!;

    [ForeignKey("VariationID")]
    [InverseProperty("ProductImages")]
    public virtual ProductVariations Variation { get; set; } = null!;

    public static implicit operator string?(ProductImages? v)
    {
        throw new NotImplementedException();
    }
}
