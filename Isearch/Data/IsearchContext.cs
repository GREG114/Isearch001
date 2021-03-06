﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Isearch.Models;

namespace Isearch.Models
{
    public class IsearchContext : DbContext
    {
        public IsearchContext (DbContextOptions<IsearchContext> options)
            : base(options)
        {
        }

        public DbSet<NTQ> NTQ { get; set; }

        public DbSet<Training> Trainings { get; set; }

        public DbSet<TrainingFeedBack> TrainingFeedBacks { get; set; }

        public DbSet<Custin> Custins { get; set; }

        // public DbSet<saleforecast> saleforecasts { get; set; }
    }
}
