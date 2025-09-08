using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

[PrimaryKey("OrderID", "ProductID", "VariationID")]
public partial class OrderDetail
{
    [Key]
    [StringLength(13)]
    public string OrderID { get; set; } = null!;

    [StringLength(100)]
    public string ProductName { get; set; } = null!;

    [StringLength(100)]
    public string VariationName { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "money")]
    public decimal SubTotal { get; set; }

    public byte ProductType { get; set; }

    [Column(TypeName = "money")]
    public decimal? ShippingFee { get; set; }

    [Column(TypeName = "money")]
    public decimal? PlatformFee { get; set; }

    [Column(TypeName = "money")]
    public decimal? CreatorAmount { get; set; }

    public byte PaymentMethod { get; set; }

    [Key]
    [StringLength(8)]
    public string ProductID { get; set; } = null!;

    [Key]
    [StringLength(8)]
    public string VariationID { get; set; } = null!;

    [ForeignKey("OrderID")]
    [InverseProperty("OrderDetail")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductID")]
    [InverseProperty("OrderDetail")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("VariationID")]
    [InverseProperty("OrderDetail")]
    public virtual ProductVariations Variation { get; set; } = null!;
}
