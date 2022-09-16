using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ReportMicroService.Api.Helpers;
using ReportMicroService.Bussiness.Abstract;
using ReportMicroService.Entities.Model;

namespace ReportMicroService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet("[action]")]
        public async Task<Response<IEnumerable<Report>>> GetAllData()
        {
            var reports = await _reportService.GetAllReports();
            return new Response<IEnumerable<Report>>().Ok(reports.Count(), reports);
        }


        [HttpPost("[action]")]
        public async Task<Response<Report>> Create(Report report)
        {
            try
            {
                if (report.ReportId == 0)
                    await _reportService.CreateReport(report);
                else await _reportService.UpdateReport(report);
                return new Response<Report>().Ok(1, report);
            }
            catch (Exception ex)
            {
                return new Response<Report>().Error(1, report, ex.ToString());
            }
        }

        [HttpPost("[action]/{id}")]
        public async Task Delete(int id)
        {
            if (id != 0)
            {
                var deleteData = await _reportService.GetReportById(id);

                await _reportService.DeleteReport(deleteData);
            }


        }
        [HttpPost("[action]")]
        public async Task RequestReport(string Location)
        {
            await _reportService.RequestReport(Location);
        }

        [HttpGet("content/{link}")]
        public IActionResult GetContent(string link)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(Path.Combine("contents", link), contentType, Path.GetFileName(link));
        }
    }
}
