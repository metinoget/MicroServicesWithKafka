using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.Entities.Model
{
    public class BaseEntity
    {
        public DateTime CreationDateTime { get;  set; }
        public DateTime LastModifiedDateTime { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsUpdated { get; set; } = false;

    }
}
