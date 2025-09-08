using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

public partial class Order
{
    [Key]
    [StringLength(13)]
    public string OrderID { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal TotalAmount { get; set; }

    public byte PaymentStatus { get; set; }

    public byte OrderStatus { get; set; }

    [StringLength(20)]
    public string? ECPayMerchantID { get; set; }

    [StringLength(50)]
    public string? RecipientName { get; set; }

    [StringLength(20)]
    public string? RecipientPhone { get; set; }

    [StringLength(300)]
    public string? ShippingAddress { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PaidDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ShippedDate { get; set; }

    [StringLength(8)]
    public string SupporterID { get; set; } = null!;

    [StringLength(8)]
    public string CreatorID { get; set; } = null!;

    [StringLength(8)]
    public string AddressID { get; set; } = null!;

    [ForeignKey("AddressID")]
    [InverseProperty("Order")]
    public virtual ShippingAddress Address { get; set; } = null!;

    [ForeignKey("CreatorID")]
    [InverseProperty("Order")]
    public virtual Creator Creator { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<DigitalDownload> DigitalDownload { get; set; } = new List<DigitalDownload>();

    [InverseProperty("Order")]
    public virtual ICollection<ECPayTransactions> ECPayTransactions { get; set; } = new List<ECPayTransactions>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

    [ForeignKey("SupporterID")]
    [InverseProperty("Order")]
    public virtual Supporter Supporter { get; set; } = null!;
}
