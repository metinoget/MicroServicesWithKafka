using ContactMicroService.Entities.Model;
using ContactMicroService.DataAccess.Configrations;
using ContactMicroService.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.DataAccess
{
    public class ContactServiceDBContext : DbContext
    {


        public ContactServiceDBContext(DbContextOptions<ContactServiceDBContext> options) : base(options)
        {

        }

        public DbSet<Contact>? Contacts { get; set; }
        public DbSet<ContactInfo>? ContactInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ContactConfiguration());
            builder.ApplyConfiguration(new ContactInfoConfiguration());

        }


    }
}
