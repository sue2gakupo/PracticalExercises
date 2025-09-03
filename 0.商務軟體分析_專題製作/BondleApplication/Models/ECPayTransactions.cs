using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

[Index("ECPayTradeNo", Name = "IX_ECPayTransactions_ECPayTradeNo")]
[Index("OrderID", Name = "IX_ECPayTransactions_OrderID")]
[Index("PaymentStatus", Name = "IX_ECPayTransactions_PaymentStatus")]
public partial class ECPayTransactions
{
    [Key]
    [StringLength(15)]
    public string TransactionID { get; set; } = null!;

    [StringLength(13)]
    public string OrderID { get; set; } = null!;

    [StringLength(8)]
    public string AddressID { get; set; } = null!;

    [StringLength(20)]
    public string? ECPayMerchantID { get; set; }

    [StringLength(20)]
    public string? ECPayTradeNo { get; set; }

    [StringLength(20)]
    public string? ECPayPaymentType { get; set; }

    [Column(TypeName = "money")]
    public decimal TradeAmount { get; set; }

    public byte PaymentStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TradeDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PaymentDate { get; set; }

    [StringLength(20)]
    public string? CVSPaymentNo { get; set; }

    [StringLength(10)]
    public string? CVSValidationNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpireDate { get; set; }

    [Column(TypeName = "ntext")]
    public string? ECPayReturnData { get; set; }

    [StringLength(100)]
    public string? CheckMacValue { get; set; }

    [ForeignKey("AddressID")]
    [InverseProperty("ECPayTransactions")]
    public virtual ShippingAddresses Address { get; set; } = null!;

    [ForeignKey("OrderID")]
    [InverseProperty("ECPayTransactions")]
    public virtual Order Order { get; set; } = null!;
}
