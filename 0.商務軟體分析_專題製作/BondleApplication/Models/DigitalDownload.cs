using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

[Index("MemberID", Name = "IX_DigitalDownload_MemberID")]
[Index("OrderID", Name = "IX_DigitalDownload_OrderID")]
public partial class DigitalDownload
{
    [Key]
    [StringLength(8)]
    public string DownloadID { get; set; } = null!;

    [StringLength(13)]
    public string OrderID { get; set; } = null!;

    [StringLength(36)]
    public string MemberID { get; set; } = null!;

    public int DownloadCount { get; set; }

    public int DownloadLimit { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastDLDate { get; set; }

    [ForeignKey("MemberID")]
    [InverseProperty("DigitalDownload")]
    public virtual Member Member { get; set; } = null!;

    [ForeignKey("OrderID")]
    [InverseProperty("DigitalDownload")]
    public virtual Order Order { get; set; } = null!;
}
