using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using ReportMicroService.Bussiness.Abstract;
using ReportMicroService.Bussiness.Concrete;
using ReportMicroService.Entities.Model;
using ReportMicroService.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.Test.Services
{
    public class ReportServiceTests
    {
        private readonly MockReports _mockReports;
        public ReportServiceTests()
        {
            _mockReports = new MockReports();
        }
        [Fact]
        public async Task GetReport_ListOfReport_ReportExistsInRepo()
        {
            var reports = _mockReports.GetReports();
            var mockLogger = new Mock<ILogger<IReportService>>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(x => x.Report.GetAllAsync()).ReturnsAsync(reports);
            var reportServ = new ReportService(mockUoW.Object, mockLogger.Object);
            var result = await reportServ.GetAllReports();
            var count = result.Count();
            mockUoW.Verify(x => x.Report.GetAllAsync(), Times.Once);
            Assert.Equal(reports.Count, count);
        }

        [Fact]
        public async Task GetReport_ListOfReport_ReportExistsInRepoIsNotDeleted()
        {
            var reports = _mockReports.GetReports();
            var mockLogger = new Mock<ILogger<IReportService>>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(x => x.Report.GetDeleteFilteredAll()).ReturnsAsync(reports);
            var reportServ = new ReportService(mockUoW.Object, mockLogger.Object);
            var result = await reportServ.GetDeleteFilteredAllReports();
            var count = result.Count();
            mockUoW.Verify(x => x.Report.GetDeleteFilteredAll(), Times.Once);
            Assert.Equal(reports.Count, count);
        }

        [Fact]
        public void GetReport_GetById_ReportExistInRepo()
        {
            var mockLogger = new Mock<ILogger<IReportService>>();
            var mockRepo = new Mock<IReportRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            int id = 1;

            mockUoW.Setup(x => x.Report.GetByIdAsync(id)).Returns(mockRepo.Object.GetByIdAsync(id));
            var reportServ = new ReportService(mockUoW.Object, mockLogger.Object);
            var result = reportServ.GetReportById(id).Result;
            mockRepo.Verify(x => x.GetByIdAsync(id), Times.Once);
        }


        [Fact]
        public async Task CreateReport_CreatedStatus_PassingReportObjectToCreate()
        {
            var newReport = new Report();
            newReport.ReportId = 1;
            var mockLogger = new Mock<ILogger<IReportService>>();
            var mockRepo = new Mock<IReportRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(x => x.Report.AddAsync(newReport)).Returns(mockRepo.Object.AddAsync(newReport));
            var reportServ = new ReportService(mockUoW.Object, mockLogger.Object);
            var result = await reportServ.CreateReport(newReport);
            mockRepo.Verify(x => x.AddAsync(newReport), Times.Once);
        }

        [Fact]
        public void UpdateReport_UpdatedStatus_PassingReportObjectToUpdate()
        {

            var reports = _mockReports.GetReports();
            var mockLogger = new Mock<ILogger<IReportService>>();
            var mockRepo = new Mock<IReportRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            var report = reports[0];
            mockUoW.Setup(x => x.Report.Update(report));
            var reportServ = new ReportService(mockUoW.Object, mockLogger.Object);
            var result = reportServ.UpdateReport(report);
            mockUoW.Verify(x => x.Report.Update(report), Times.Once);
        }

        [Fact]
        public async Task SendKafka_ContactReportObject_CreateReportRequest()
        {
            string Location = "Hatay";
            var mockLogger = new Mock<ILogger<IReportService>>();
            var mockRepo = new Mock<IReportRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(x => x.Report.AddAsync(It.IsAny<Report>())).ReturnsAsync(new Report());
            var reportServ = new ReportService(mockUoW.Object, mockLogger.Object);
            var result = await reportServ.RequestReport(Location);
            mockUoW.Verify(x => x.Report.AddAsync(It.IsAny<Report>()), Times.Once);
            Assert.NotNull(result);
        }

        
    }
}
