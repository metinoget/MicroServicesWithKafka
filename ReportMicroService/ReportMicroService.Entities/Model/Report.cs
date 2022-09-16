using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportMicroService.Entities.Enums;

namespace ReportMicroService.Entities.Model
{
    public class Report : BaseEntity
    {
        public int ReportId { get; set; }
        public ReportState ReportState { get; set; }
        public string? ReportURL { get; set; }

    }
}
