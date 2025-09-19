using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BondleApplication.Models
{
    public partial class MemberData
    {
        [Required(ErrorMessage = "E-Mail必填")]
        [EmailAddress(ErrorMessage = "E-Mail格式錯誤")]
        [StringLength(255)]
        [Display(Name = "E-Mail")]
        public string Email { get; set; } = null!;
    
    
    
    }

    public partial class ShippingAddressData
    {
        [Required(ErrorMessage = "請選擇運送方式")]
        [Display(Name = "[運送方式")]
        public byte AddressType { get; set; }

        [Required(ErrorMessage = "請輸入收件人姓名")]
        [Display(Name = "收件人姓名")]
        [StringLength(50)]
        public string RecipientName { get; set; } = null!;

        [Required(ErrorMessage = "請輸入收件人電話")]
        [StringLength(20)]
        [Phone(ErrorMessage = "電話格式不正確")]
        [Display(Name = "收件人電話")]
        public string RecipientPhone { get; set; } = null!;

        [Display(Name = "郵遞區號")]
        public string? PostalCode { get; set; }

        [StringLength(20)]
        [Display(Name = "城市")]
        public string? City { get; set; }

        [StringLength(20)]
        [Display(Name = "地區")]
        public string? District { get; set; }

        [StringLength(200)]
        [Display(Name = "詳細地址")]
        public string? Address { get; set; }

    }


    public partial class CreatorData
    {
        [Required(ErrorMessage = "創作者名稱必填")]
        [StringLength(50, ErrorMessage = "創作者名稱不可超過50個字")]
        [Display(Name = "創作者名稱")]
        public string CreatorName { get; set; } = null!;

        [Display(Name = "創作者簡介")]
        public string? Biography { get; set; }

        [StringLength(50, ErrorMessage = "銀行帳號不可超過50個字")]
        [Display(Name = "銀行帳號")]
        public string? BankAccount { get; set; }

        [StringLength(10, ErrorMessage = "銀行代碼為三位數字")]
        [Display(Name = "銀行代碼")]
        public string? BankCode { get; set; }

        [StringLength(50, ErrorMessage = "名稱不可超過50個字")]
        [Display(Name = "帳戶名稱")]
        public string? AccountHolderName { get; set; }

        [StringLength(20, ErrorMessage = "綠界商店代碼不可超過20個字")]
        [Display(Name = "綠界商店代碼")]
        public string? ECPayMerchantID { get; set; }

    }

    public partial class SupporterData
    {

        [StringLength(50, ErrorMessage = "創作者名稱不可超過50個字")]
        [Display(Name = "支持者名稱")]
        public string? SupporterName { get; set; }

    }


    public partial class OrderData
    {

        [StringLength(50)]
        [Display(Name = "收件人姓名")]
        public string? RecipientName { get; set; }

        [StringLength(20)]
        [Display(Name = "收件人電話")]
        public string? RecipientPhone { get; set; }

        [StringLength(300)]
        [Display(Name = "收件地址")]
        public string? ShippingAddress { get; set; }

    }


    public partial class DigitalProductData
    {
        public int DownloadLimit { get; set; }

        public int? ValidityDays { get; set; }

        public byte? LicenseType { get; set; }

        [Column(TypeName = "ntext")]
        public string? LicenseDescription { get; set; }


    }

    public partial class PhysicalProductData
    {

        [Column(TypeName = "decimal(8, 3)")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Length { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Width { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Height { get; set; }

        public int DeliveryDays { get; set; }

        public byte ShippingFeeType { get; set; }

        public bool IsFragile { get; set; }

        [StringLength(200)]
        public string? PackagingNote { get; set; }
    }



    public partial class ProductData
    {
        [Required(ErrorMessage = "商品名稱必填")]
        [StringLength(100, ErrorMessage = "商品名稱不可超過100字")]
        [Display(Name = "商品名稱")]
        public string ProductName { get; set; } = null!;

        [Column(TypeName = "ntext")]
        [Display(Name = "商品說明")]
        public string? Description { get; set; }

        [Required]
        [Range(1, 2)]
        [Display(Name = "商品類型")]
        public byte ProductType { get; set; }

        [Required(ErrorMessage = "請輸入價格")]
        [Range(1, 999999, ErrorMessage = "價格必須大於 0")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

    }

    public partial class CategoryData
    {

        [Required(ErrorMessage = "請輸入類別名稱")]
        [StringLength(50, ErrorMessage = "類別名稱不可超過50個字")]
        [DisplayName("類別名稱")]
        public string CategoryName { get; set; } = null!;

        [StringLength(200, ErrorMessage = "描述不可超過200個字")]
        [DisplayName("類別描述")]
        public string? Description { get; set; }

    }

    public partial class ProductSeriesData
    {

        [StringLength(80)]
        public string SeriesName { get; set; } = null!;

        [Column(TypeName = "ntext")]
        public string? Description { get; set; }

        [StringLength(200)]
        public string? Tags { get; set; }
    }

    public partial class ProductVariationsData
    {

        [StringLength(50)]
        public string SKU { get; set; } = null!;

        [StringLength(100)]
        public string VariationName { get; set; } = null!;

        [StringLength(30)]
        public string? Color { get; set; }

        [StringLength(20)]
        public string? Size { get; set; }

        [StringLength(30)]
        public string? Material { get; set; }

        [StringLength(20)]
        public string? Edition { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceDifference { get; set; }

        public int Stock { get; set; }

        public int SafetyStock { get; set; }

    }




    [MetadataType(typeof(MemberData))]
    public partial class Member { }

    [MetadataType(typeof(ShippingAddressData))]
    public partial class ShippingAddress { }

    [MetadataType(typeof(CreatorData))]
    public partial class Creator { }

    [MetadataType(typeof(SupporterData))]
    public partial class Supporter { }

    [MetadataType(typeof(OrderData))]
    public partial class Order { }

    [MetadataType(typeof(ProductData))]
    public partial class Product { }

    [MetadataType(typeof(ProductVariationsData))]
    public partial class ProductVariations { }

    [MetadataType(typeof(ProductSeriesData))]
    public partial class ProductSeries { }

    [MetadataType(typeof(DigitalProductData))]
    public partial class DigitalProduct { }

    [ModelMetadataType(typeof(CategoryData))]
    public partial class Category { }

}
