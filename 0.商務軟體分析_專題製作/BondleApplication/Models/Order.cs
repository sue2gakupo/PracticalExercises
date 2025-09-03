using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

[Index("CreateDate", Name = "IX_Order_CreateDate")]
[Index("MemberID", Name = "IX_Order_MemberID")]
[Index("OrderNumber", Name = "IX_Order_OrderNumber")]
[Index("OrderStatus", Name = "IX_Order_OrderStatus")]
[Index("PaymentStatus", Name = "IX_Order_PaymentStatus")]
[Index("OrderNumber", Name = "UQ_Order_OrderNumber", IsUnique = true)]
public partial class Order
{
    [Key]
    [StringLength(13)]
    public string OrderID { get; set; } = null!;

    [StringLength(36)]
    public string MemberID { get; set; } = null!;

    [StringLength(8)]
    public string AddressID { get; set; } = null!;

    [StringLength(12)]
    public string OrderNumber { get; set; } = null!;

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

    [ForeignKey("AddressID")]
    [InverseProperty("Order")]
    public virtual ShippingAddresses Address { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<DigitalDownload> DigitalDownload { get; set; } = new List<DigitalDownload>();

    [InverseProperty("Order")]
    public virtual ICollection<ECPayTransactions> ECPayTransactions { get; set; } = new List<ECPayTransactions>();

    [ForeignKey("MemberID")]
    [InverseProperty("Order")]
    public virtual Member Member { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
}
