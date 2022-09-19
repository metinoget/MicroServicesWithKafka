using ReportMicroService.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.Bussiness.Abstract
{
    public interface IReportService
    {
        Task<IEnumerable<Report>> GetAllReports();
        Task<IEnumerable<Report>> GetDeleteFilteredAllReports();
        Task<Report> GetReportById(int id);
        Task<Report> CreateReport(Report report);
        Task UpdateReport(Report report);
        Task<Report> RequestReport(string Location);
    }
}
