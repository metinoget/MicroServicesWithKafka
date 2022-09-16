using Microsoft.EntityFrameworkCore;
using ReportMicroService.DataAccess.Configurations;
using ReportMicroService.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.DataAccess
{
    public class ReportServiceDbContext : DbContext
    {

        public ReportServiceDbContext(DbContextOptions<ReportServiceDbContext> options) : base(options)
        {

        }
        public DbSet<Report>? Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ReportConfiguration());

        }


    }
}
