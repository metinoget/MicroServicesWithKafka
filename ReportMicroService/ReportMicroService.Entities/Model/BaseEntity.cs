using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.Entities.Model
{
    public class BaseEntity
    {
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime CreationDateTime { get;  set; } = DateTime.UtcNow;
        public DateTime LastModifiedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; }

    }
}
