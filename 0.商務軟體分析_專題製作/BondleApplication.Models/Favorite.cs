using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

public partial class Favorite
{
    [Key]
    [StringLength(8)]
    public string FavoriteID { get; set; } = null!;

    [StringLength(8)]
    public string? ProductID { get; set; }

    [StringLength(8)]
    public string? SeriesID { get; set; }

    public byte FavoriteType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [StringLength(8)]
    public string SupporterID { get; set; } = null!;

    [ForeignKey("ProductID")]
    [InverseProperty("Favorite")]
    public virtual Product? Product { get; set; }

    [ForeignKey("SeriesID")]
    [InverseProperty("Favorite")]
    public virtual ProductSeries? Series { get; set; }

    [ForeignKey("SupporterID")]
    [InverseProperty("Favorite")]
    public virtual Supporter Supporter { get; set; } = null!;
}
