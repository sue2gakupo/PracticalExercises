using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

public partial class ShippingAddresses
{
    [Key]
    [StringLength(8)]
    public string AddressID { get; set; } = null!;

    [StringLength(36)]
    public string MemberID { get; set; } = null!;

    public byte AddressType { get; set; }

    [StringLength(50)]
    public string RecipientName { get; set; } = null!;

    [StringLength(20)]
    public string RecipientPhone { get; set; } = null!;

    [StringLength(20)]
    public string? ConvenienceStoreType { get; set; }

    [StringLength(20)]
    public string? StoreCode { get; set; }

    [StringLength(50)]
    public string? StoreName { get; set; }

    [StringLength(10)]
    public string? PostalCode { get; set; }

    [StringLength(20)]
    public string? City { get; set; }

    [StringLength(20)]
    public string? District { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    public bool IsDefault { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [InverseProperty("Address")]
    public virtual ICollection<ECPayTransactions> ECPayTransactions { get; set; } = new List<ECPayTransactions>();

    [ForeignKey("MemberID")]
    [InverseProperty("ShippingAddresses")]
    public virtual Member Member { get; set; } = null!;

    [InverseProperty("Address")]
    public virtual ICollection<Order> Order { get; set; } = new List<Order>();
}
