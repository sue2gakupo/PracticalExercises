using System;
using System.Collections.Generic;
using BondleApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Data;

public partial class BondleDBContext : DbContext
{
    public BondleDBContext(DbContextOptions<BondleDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<Creator> Creator { get; set; }

    public virtual DbSet<DigitalDownload> DigitalDownload { get; set; }

    public virtual DbSet<DigitalProduct> DigitalProduct { get; set; }

    public virtual DbSet<ECPayTransactions> ECPayTransactions { get; set; }

    public virtual DbSet<Favorite> Favorite { get; set; }

    public virtual DbSet<Member> Member { get; set; }

    public virtual DbSet<MemberRoles> MemberRoles { get; set; }

    public virtual DbSet<Order> Order { get; set; }

    public virtual DbSet<OrderDetail> OrderDetail { get; set; }

    public virtual DbSet<PhysicalProduct> PhysicalProduct { get; set; }

    public virtual DbSet<Product> Product { get; set; }

    public virtual DbSet<ProductImages> ProductImages { get; set; }

    public virtual DbSet<ProductSeries> ProductSeries { get; set; }

    public virtual DbSet<ProductVariations> ProductVariations { get; set; }

    public virtual DbSet<ShippingAddresses> ShippingAddresses { get; set; }

    public virtual DbSet<Supporter> Supporter { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categories>(entity =>
        {
            entity.Property(e => e.CategoryID).IsFixedLength();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ParentCategoryID).IsFixedLength();

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory).HasConstraintName("FK_Categories_Parent");
        });

        modelBuilder.Entity<Creator>(entity =>
        {
            entity.Property(e => e.MemberID).IsFixedLength();
            entity.Property(e => e.ApplyDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PlatformFeeRate).HasDefaultValue(5.000m);
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Member).WithOne(p => p.Creator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Creator_Member");
        });

        modelBuilder.Entity<DigitalDownload>(entity =>
        {
            entity.Property(e => e.DownloadID).IsFixedLength();
            entity.Property(e => e.DownloadLimit).HasDefaultValue(3);
            entity.Property(e => e.MemberID).IsFixedLength();
            entity.Property(e => e.OrderID).IsFixedLength();

            entity.HasOne(d => d.Member).WithMany(p => p.DigitalDownload)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DigitalDownload_Member");

            entity.HasOne(d => d.Order).WithMany(p => p.DigitalDownload)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DigitalDownload_Order");
        });

        modelBuilder.Entity<DigitalProduct>(entity =>
        {
            entity.Property(e => e.ProductID).IsFixedLength();
            entity.Property(e => e.DownloadLimit).HasDefaultValue(3);

            entity.HasOne(d => d.Product).WithOne(p => p.DigitalProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DigitalProduct_Product");
        });

        modelBuilder.Entity<ECPayTransactions>(entity =>
        {
            entity.Property(e => e.TransactionID).IsFixedLength();
            entity.Property(e => e.AddressID).IsFixedLength();
            entity.Property(e => e.OrderID).IsFixedLength();
            entity.Property(e => e.PaymentStatus).HasDefaultValue((byte)1);
            entity.Property(e => e.TradeDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Address).WithMany(p => p.ECPayTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ECPayTransactions_Address");

            entity.HasOne(d => d.Order).WithMany(p => p.ECPayTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ECPayTransactions_Order");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.Property(e => e.FavoriteID).IsFixedLength();
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MemberID).IsFixedLength();
            entity.Property(e => e.ProductID).IsFixedLength();
            entity.Property(e => e.SeriesID).IsFixedLength();

            entity.HasOne(d => d.Member).WithMany(p => p.Favorite)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Favorite_Member");

            entity.HasOne(d => d.Product).WithMany(p => p.Favorite).HasConstraintName("FK_Favorite_Product");

            entity.HasOne(d => d.Series).WithMany(p => p.Favorite).HasConstraintName("FK_Favorite_Series");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.Property(e => e.MemberID).IsFixedLength();
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LastLoginDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
        });

        modelBuilder.Entity<MemberRoles>(entity =>
        {
            entity.Property(e => e.MemberID).IsFixedLength();

            entity.HasOne(d => d.Member).WithOne(p => p.MemberRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberRoles_Member");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderID).IsFixedLength();
            entity.Property(e => e.AddressID).IsFixedLength();
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MemberID).IsFixedLength();
            entity.Property(e => e.OrderNumber).IsFixedLength();
            entity.Property(e => e.OrderStatus).HasDefaultValue((byte)1);
            entity.Property(e => e.PaymentStatus).HasDefaultValue((byte)1);

            entity.HasOne(d => d.Address).WithMany(p => p.Order)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Address");

            entity.HasOne(d => d.Member).WithMany(p => p.Order)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Member");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.OrderID).IsFixedLength();
            entity.Property(e => e.ProductID).IsFixedLength();
            entity.Property(e => e.VariationID).IsFixedLength();

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Product");

            entity.HasOne(d => d.Variation).WithMany(p => p.OrderDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Variation");
        });

        modelBuilder.Entity<PhysicalProduct>(entity =>
        {
            entity.Property(e => e.ProductID).IsFixedLength();
            entity.Property(e => e.DeliveryDays).HasDefaultValue(3);
            entity.Property(e => e.ShippingFeeType).HasDefaultValue((byte)1);

            entity.HasOne(d => d.Product).WithOne(p => p.PhysicalProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhysicalProduct_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductID).IsFixedLength();
            entity.Property(e => e.CategoryID).IsFixedLength();
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MemberID).IsFixedLength();
            entity.Property(e => e.SeriesID).IsFixedLength();
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Category).WithMany(p => p.Product).HasConstraintName("FK_Product_Category");

            entity.HasOne(d => d.Member).WithMany(p => p.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Member");

            entity.HasOne(d => d.Series).WithMany(p => p.Product).HasConstraintName("FK_Product_Series");
        });

        modelBuilder.Entity<ProductImages>(entity =>
        {
            entity.Property(e => e.ImageID).IsFixedLength();
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.VariationID).IsFixedLength();

            entity.HasOne(d => d.Variation).WithMany(p => p.ProductImages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductImages_Variation");
        });

        modelBuilder.Entity<ProductSeries>(entity =>
        {
            entity.Property(e => e.SeriesID).IsFixedLength();
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsPublic).HasDefaultValue(true);
            entity.Property(e => e.MemberID).IsFixedLength();

            entity.HasOne(d => d.Member).WithMany(p => p.ProductSeries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductSeries_Member");
        });

        modelBuilder.Entity<ProductVariations>(entity =>
        {
            entity.Property(e => e.VariationID).IsFixedLength();
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ProductID).IsFixedLength();
            entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductVariations_Product");
        });

        modelBuilder.Entity<ShippingAddresses>(entity =>
        {
            entity.Property(e => e.AddressID).IsFixedLength();
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MemberID).IsFixedLength();
            entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Member).WithMany(p => p.ShippingAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShippingAddresses_Member");
        });

        modelBuilder.Entity<Supporter>(entity =>
        {
            entity.Property(e => e.MemberID).IsFixedLength();
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Member).WithOne(p => p.Supporter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supporter_Member");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
