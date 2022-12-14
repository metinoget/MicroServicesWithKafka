using DocumentFormat.OpenXml.Bibliography;
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
        private readonly ILogger<ReportController> _logger;
        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<Response<IEnumerable<Report>>> GetAllData()
        {
            try
            {
                var reports = await _reportService.GetAllReports();
                return new Response<IEnumerable<Report>>().Ok(reports.Count(), reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<IEnumerable<Report>>().NotFound("Report cannot found.");
            }
        }

        [HttpGet("[action]")]
        public async Task<Response<IEnumerable<Report>>> GetDeleteFilteredAllData()
        {
            try
            {
                var reports = await _reportService.GetDeleteFilteredAllReports();
                return new Response<IEnumerable<Report>>().Ok(reports.Count(), reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<IEnumerable<Report>>().NotFound("Report cannot found.");
            }
        }


        [HttpPost("[action]")]
        public async Task<Response<Report>> CreateOrUpdate(Report report)
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
        public async Task<Response<Report>> Delete(int id)
        {
            try
            {
                if (id != 0)
                {
                    var deleteData = await _reportService.GetReportById(id);
                    deleteData.IsDeleted = true;
                    await _reportService.UpdateReport(deleteData);
                    return new Response<Report>().Ok(1, null);
                }

                return new Response<Report>().NotFound("Report can not found");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<Report>().NotFound("Delete report failed");
            }
        }

        [HttpPost("[action]/{id}")]
        public async Task<Response<Report>> GetById(int id)
        {
            try
            {

                if (id != 0)
                {
                    var report = await _reportService.GetReportById(id);
                    if (report != null)
                        return new Response<Report>().Ok(1, report);
                    else return new Response<Report>().NotFound("Report cannot found.");
                }
                else
                    return new Response<Report>().NotFound("Report cannot found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<Report>().NotFound("Unhandled Exception.");
            }

        }
        [HttpPost("[action]")]
        public async Task<Response<Report>> RequestReport(string Location)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Location))
                {
                    var report =await _reportService.RequestReport(Location);
                    return new Response<Report>().Ok(1, report);
                }
                return new Response<Report>().NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new Response<Report>().NotFound();
            }
        }

        [HttpGet("content/{link}")]
        public IActionResult GetContent(string link)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(Path.Combine("contents", link), contentType, Path.GetFileName(link));
        }
    }
}
