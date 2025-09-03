using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Models;

public partial class MemberRoles
{
    [Key]
    [StringLength(36)]
    public string MemberID { get; set; } = null!;

    public bool IsCreator { get; set; }

    public bool IsSupporter { get; set; }

    [ForeignKey("MemberID")]
    [InverseProperty("MemberRoles")]
    public virtual Member Member { get; set; } = null!;
}
