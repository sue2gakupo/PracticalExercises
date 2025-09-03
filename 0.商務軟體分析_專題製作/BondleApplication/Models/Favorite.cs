using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

[Index("MemberID", Name = "IX_Favorite_MemberID")]
[Index("ProductID", Name = "IX_Favorite_ProductID")]
[Index("SeriesID", Name = "IX_Favorite_SeriesID")]
[Index("FavoriteType", Name = "IX_Favorite_Type")]
public partial class Favorite
{
    [Key]
    [StringLength(8)]
    public string FavoriteID { get; set; } = null!;

    [StringLength(36)]
    public string MemberID { get; set; } = null!;

    [StringLength(8)]
    public string? ProductID { get; set; }

    [StringLength(8)]
    public string? SeriesID { get; set; }

    public byte FavoriteType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [ForeignKey("MemberID")]
    [InverseProperty("Favorite")]
    public virtual Member Member { get; set; } = null!;

    [ForeignKey("ProductID")]
    [InverseProperty("Favorite")]
    public virtual Product? Product { get; set; }

    [ForeignKey("SeriesID")]
    [InverseProperty("Favorite")]
    public virtual ProductSeries? Series { get; set; }
}
