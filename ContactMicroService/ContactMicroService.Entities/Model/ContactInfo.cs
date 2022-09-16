using ContactMicroService.Entities.Enums;
using ContactMicroService.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ContactMicroService.Entities.Model
{
    public class ContactInfo :BaseEntity
    { 
        public int ContactInfoId { get; set; }  
        public int ContactId { get; set; }
        public ContactType ContactType { get; set; }
        public string? Information { get; set; }

    }
}
