using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

public partial class Categories
{
    [Key]
    [StringLength(8)]
    public string CategoryID { get; set; } = null!;

    [StringLength(50)]
    public string CategoryName { get; set; } = null!;

    [StringLength(200)]
    public string? Description { get; set; }

    [StringLength(8)]
    public string? ParentCategoryID { get; set; }

    [StringLength(500)]
    public string? IconUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("ParentCategory")]
    public virtual ICollection<Categories> InverseParentCategory { get; set; } = new List<Categories>();

    [ForeignKey("ParentCategoryID")]
    [InverseProperty("InverseParentCategory")]
    public virtual Categories? ParentCategory { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Product> Product { get; set; } = new List<Product>();
}
