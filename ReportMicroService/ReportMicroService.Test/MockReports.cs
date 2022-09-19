using ReportMicroService.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.Test
{
    public class MockReports
    {

        public readonly List<Report> reportList;
        public MockReports()
        {
            reportList = new List<Report>
            {
            new Report
            {
                ReportId=1,
                IsDeleted=false,
            },
            new Report
            {
                ReportId = 2,
                IsDeleted=false,
            },
            new Report
            {
                ReportId=3,
                IsDeleted=false,

            }
            };
        }

        public List<Report> GetReports()
        {
            return reportList;
        }

        public Report GetById(int id)
        {
            return reportList.First(c => c.ReportId == id);
        }

    }
}
