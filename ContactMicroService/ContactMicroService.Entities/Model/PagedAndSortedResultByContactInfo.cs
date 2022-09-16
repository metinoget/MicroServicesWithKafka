using ContactMicroService.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.Entities.Model
{
    public class PagedAndSortedResultByContactInfo
    {
        public ContactType ContactType { get; set; }
        public string? Information { get; set; }
    }
}
