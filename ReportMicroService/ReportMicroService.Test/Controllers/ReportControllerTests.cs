using Microsoft.Extensions.Logging;
using Moq;
using ReportMicroService.Bussiness.Abstract;
using ReportMicroService.Entities.Model;
using ReportMicroService.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.Test.Controllers
{
    public class ReportControllerTests
    {
        private readonly Mock<IReportService> _reportService;
        private readonly Mock<ILogger<ReportController>> logger;
        private readonly MockReports _mockReport;
        public ReportControllerTests()
        {
            _reportService = new Mock<IReportService>();
            logger = new Mock<ILogger<ReportController>>();
            _mockReport = new MockReports();
        }



        [Fact]
        public void GetReport_ListOfReport_ReportExistsInRepo()
        {
            var reports = _mockReport.GetReports();
            _reportService.Setup(x => x.GetAllReports())
                .ReturnsAsync(reports);
            var controller = new ReportController(_reportService.Object, logger.Object);

            var actionResult = controller.GetAllData();
            var result = actionResult.Result;
            var statusCode = result.Code;
            var actual = result.Data as IEnumerable<Report>;

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(_mockReport.GetReports().Count(), actual.Count());
        }

        [Fact]
        public void GetReport_ListOfReport_ReportIfNotExistsInRepo()
        {
            _reportService.Setup(x => x.GetAllReports())
                .ReturnsAsync((List<Report>)null);
            var controller = new ReportController(_reportService.Object, logger.Object);

            var actionResult = controller.GetAllData();
            var result = actionResult.Result;
            var statusCode = result.Code;
            var actual = result.Data as IEnumerable<Report>;

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }


        [Fact]
        public void GetFilteredReport_ListOfReport_ReportExistsInRepoIsNotDeleted()
        {
            var reports = _mockReport.GetReports();
            _reportService.Setup(x => x.GetDeleteFilteredAllReports())
                .ReturnsAsync(reports);
            var controller = new ReportController(_reportService.Object, logger.Object);

            var actionResult = controller.GetDeleteFilteredAllData();
            var result = actionResult.Result;
            var statusCode = result.Code;
            var actual = result.Data as IEnumerable<Report>;

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(reports.Count(), actual.Count());
        }
        [Fact]
        public void GetReport_ListOfReport_ReportIfNotExistsInRepoIsNotDeleted()
        {
            _reportService.Setup(x => x.GetDeleteFilteredAllReports())
                .ReturnsAsync((List<Report>)null);
            var controller = new ReportController(_reportService.Object, logger.Object);

            var actionResult = controller.GetDeleteFilteredAllData();
            var result = actionResult.Result;
            var statusCode = result.Code;
            var actual = result.Data as IEnumerable<Report>;

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }




        [Fact]
        public void GetReportById_ReportObject_ReportwithSpecifiedIdExists()
        {
            var reports = _mockReport.GetReports();
            var expected = reports[0];
            _reportService.Setup(x => x.GetReportById(1).Result)
                .Returns(expected);
            var controller = new ReportController(_reportService.Object, logger.Object);

            var actionResult = controller.GetById(1);
            var result = actionResult.Result;
            var statusCode = result.Code;
            var actual = result.Data as Report;

            Assert.IsType<HttpStatusCode>(statusCode);
            Assert.Equal(HttpStatusCode.OK, statusCode);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetReportById_shouldReturnBadRequest_ReportwithSpecificeIdNotExists()
        {
            _reportService.Setup(x => x.GetReportById(1))
                .ReturnsAsync((Report)null);
            var controller = new ReportController(_reportService.Object, logger.Object);

            var actionResult = controller.GetById(1);
            var result = actionResult.Result;
            var statusCode = result.Code;

            Assert.IsType<HttpStatusCode>(statusCode);
            Assert.Equal(HttpStatusCode.NotFound, statusCode);

        }
        [Fact]
        public void GetReportById_shouldReturnBadRequest_SendZeroAsReportId()
        {
            var report = (_mockReport.GetReports())[0];
            _reportService.Setup(x => x.GetReportById(It.IsAny<int>()))
                .ReturnsAsync(report);
            var controller = new ReportController(_reportService.Object, logger.Object);

            var actionResult = controller.GetById(0);
            var result = actionResult.Result;
            var statusCode = result.Code;

            Assert.IsType<HttpStatusCode>(statusCode);
            Assert.Equal(HttpStatusCode.NotFound, statusCode);

        }


        [Fact]
        public void CreateReport_CreatedStatus_PassingReportObjectToCreate()
        {
            var reports = _mockReport.GetReports();
            var newReport = reports[0];
            newReport.ReportId = 0;
            _reportService.Setup(x => x.CreateReport(It.IsAny<Report>()));
            var controller = new ReportController(_reportService.Object, logger.Object);
            var actionResult = controller.CreateOrUpdate(newReport);
            var result = actionResult.Result;
            Assert.IsType<HttpStatusCode>(result.Code);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            _reportService.Verify(x => x.CreateReport(newReport), Times.Once);
        }


        [Fact]
        public void UpdateReport_UpdatedStatus_PassingReportObjectToUpdate()
        {
            var reports = _mockReport.GetReports();
            var newReport = reports[0];
            _reportService.Setup(x => x.UpdateReport(It.IsAny<Report>()));
            var controller = new ReportController(_reportService.Object, logger.Object);
            var actionResult = controller.CreateOrUpdate(newReport);
            var result = actionResult.Result;
            Assert.IsType<HttpStatusCode>(result.Code);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            _reportService.Verify(x => x.UpdateReport(newReport), Times.Once);

        }
        [Fact]
        public void DeleteReport_DeletedStatus_PassingReportIdToDelete()
        {
            var reports = _mockReport.GetReports();
            var deleteReport = reports[0];
            _reportService.Setup(x => x.GetReportById(deleteReport.ReportId)).ReturnsAsync(deleteReport);
            _reportService.Setup(x => x.UpdateReport(It.IsAny<Report>()));
            var controller = new ReportController(_reportService.Object, logger.Object);
            var actionResult = controller.Delete(deleteReport.ReportId);
            var result = actionResult.Result;
            _reportService.Verify(x => x.UpdateReport(It.IsAny<Report>()), Times.Once);
            Assert.Equal(HttpStatusCode.OK, result.Code);

        }
        [Fact]
        public void DeleteReport_DeletedStatusFailed_PassingReportIdToDelete()
        {
            var reports = _mockReport.GetReports();
            var deleteReport = reports[0];
            _reportService.Setup(x => x.GetReportById(deleteReport.ReportId)).ReturnsAsync((Report)null);
            var controller = new ReportController(_reportService.Object, logger.Object);
            var actionResult = controller.Delete(deleteReport.ReportId);
            var result = actionResult.Result;

            Assert.Equal(HttpStatusCode.NotFound, result.Code);

        }
        [Fact]

        public void DeleteReport_DeletedStatusFailed_PassingZeroToDelete()
        {
            _reportService.Setup(x => x.GetReportById(It.IsAny<int>())).ReturnsAsync((Report)null);
            var controller = new ReportController(_reportService.Object, logger.Object);
            var actionResult = controller.Delete(0);
            var result = actionResult.Result;

            Assert.Equal(HttpStatusCode.NotFound, result.Code);

        }



        [Fact]
        public void RequestReport_ReportObject_PassingRequestObjectToReport()
        {
            var location = "Hatay";
            _reportService.Setup(x => x.RequestReport(It.IsAny<string>())).ReturnsAsync(new Report());
            var controller = new ReportController(_reportService.Object, logger.Object);
            var actionResult = controller.RequestReport(location);
            var result = actionResult.Result;
            Assert.IsType<HttpStatusCode>(result.Code);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            _reportService.Verify(x => x.RequestReport(location), Times.Once);
        }

        [Fact]
        public void RequestReport_ShouldReturnBadRequest_PassingnullObjectToReport()
        {
            var location = "Hatay";
            _reportService.Setup(x => x.RequestReport(It.IsAny<string>())).ReturnsAsync(new Report());
            var controller = new ReportController(_reportService.Object, logger.Object);
            var actionResult = controller.RequestReport((string)null);
            var result = actionResult.Result;
            Assert.IsType<HttpStatusCode>(result.Code);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }
    }
}
