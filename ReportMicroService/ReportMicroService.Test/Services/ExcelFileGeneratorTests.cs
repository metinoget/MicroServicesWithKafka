using Microsoft.Extensions.Logging;
using Moq;
using ReportMicroService.Bussiness.Abstract;
using ReportMicroService.Bussiness.Concrete;
using ReportMicroService.Entities.Model;
using ReportMicroService.Entities.Repositories;
using ReportMicroService.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.Test.Services
{
    public class ExcelFileGeneratorTests
    {
        [Fact]
        public async Task GenerateExcel_GeneratedExcelAsByteArray_PassingContactReportObjectToGenerateExcelDocument()
        {
            var newReport = new ContactReport { Location = "Hatay", NearbyPeopleCount = 2, NearbySavedPhoneCount = 1 };
            var export = new ExportToExcelService();

            var result = await export.ExportToExcel(newReport);
            Assert.NotNull(result);
            Assert.IsType<byte[]>(result);
        }

        [Fact]
        public async Task GenerateExcel_ThrowError_PassingNullObjectToGenerateExcelDocument()
        {
            var export = new ExportToExcelService();
            var result = await export.ExportToExcel((ContactReport)null);
            Assert.Null(result);
        }
    }
}
