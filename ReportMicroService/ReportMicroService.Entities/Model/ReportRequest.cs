using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.Entities.Model
{
    public class ReportRequest
    {
        public int Id { get; set; }
        public string? Location { get; set; }
    }
}
