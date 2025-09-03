using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

public partial class DigitalProduct
{
    [Key]
    [StringLength(8)]
    public string ProductID { get; set; } = null!;

    [StringLength(50)]
    public string FileFormat { get; set; } = null!;

    public long? FileSize { get; set; }

    [StringLength(500)]
    public string FilePath { get; set; } = null!;

    [StringLength(500)]
    public string? PreviewImagePath { get; set; }

    public int DownloadLimit { get; set; }

    public int? ValidityDays { get; set; }

    public byte? LicenseType { get; set; }

    [Column(TypeName = "ntext")]
    public string? LicenseDescription { get; set; }

    [ForeignKey("ProductID")]
    [InverseProperty("DigitalProduct")]
    public virtual Product Product { get; set; } = null!;
}
