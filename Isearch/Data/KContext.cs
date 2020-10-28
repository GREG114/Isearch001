using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Isearch.Models;

namespace Isearch.Models
{
    public class Kcontext : DbContext
    {
        public Kcontext(DbContextOptions<Kcontext> options)
            : base(options)
        {
        }

        public DbSet<voucher> t_Voucher { get; set; }

        // public DbSet<saleforecast> saleforecasts { get; set; }
    }
}
