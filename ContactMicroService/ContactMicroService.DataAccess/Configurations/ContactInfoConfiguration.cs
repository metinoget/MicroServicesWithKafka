using ContactMicroService.Entities.Enums;
using ContactMicroService.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.DataAccess.Configurations
{
    public class ContactInfoConfiguration : IEntityTypeConfiguration<ContactInfo>
    {
        public void Configure(EntityTypeBuilder<ContactInfo> builder)
        {
            builder.HasKey(m => m.ContactInfoId);
            builder.Property(m => m.ContactInfoId).ValueGeneratedOnAdd();
            builder.Property(m => m.Information).IsRequired().HasMaxLength(100);
            builder.Property(m => m.ContactType).IsRequired();
            
        }
    }
}
