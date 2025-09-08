using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

public partial class DigitalDownload
{
    [Key]
    [StringLength(8)]
    public string DownloadID { get; set; } = null!;

    public int DownloadCount { get; set; }

    public int DownloadLimit { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastDLDate { get; set; }

    [StringLength(13)]
    public string OrderID { get; set; } = null!;

    [ForeignKey("OrderID")]
    [InverseProperty("DigitalDownload")]
    public virtual Order Order { get; set; } = null!;
}
