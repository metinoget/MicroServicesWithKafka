using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.Entities.Model
{
    public class BaseEntity
    {
        public DateTime CreationDateTime { get;  set; }
        public DateTime LastModifiedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; }

    }
}
