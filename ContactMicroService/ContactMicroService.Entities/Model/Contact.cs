using ContactMicroService.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ContactMicroService.Entities.Model
{
    public class Contact : BaseEntity
    {
        public ICollection<ContactInfo>? ContactInfos { get; set; }
        public int ContactId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Company { get; set; }

    }
}
