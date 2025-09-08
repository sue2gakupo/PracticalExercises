using System;
using System.Collections.Generic;
using BondleApplication.Models;
using Microsoft.EntityFrameworkCore;



namespace BondleApplication.Access.Data;

public partial class BondleDBContext2 : BondleDBContext
{
    public BondleDBContext2(DbContextOptions<BondleDBContext> options)
        : base(options)
    {
    }


   
}
