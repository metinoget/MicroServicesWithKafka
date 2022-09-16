using ContactMicroService.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.DataAccess.Configrations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(m => m.ContactId);
            builder.Property(m => m.ContactId).ValueGeneratedOnAdd();
            builder.Property(m => m.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(m => m.LastName).IsRequired().HasMaxLength(100);
            builder.Property(m => m.Company).IsRequired().HasMaxLength(100);
            builder.HasMany(m => m.ContactInfos).WithOne();
           
        }
    }
}
