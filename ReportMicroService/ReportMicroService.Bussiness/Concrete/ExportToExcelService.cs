using ReportMicroService.Concrete;
using ReportMicroService.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.Reports
{
    public class ExportToExcelService
    {
        public Task<byte[]> ExportToExcel(ContactReport source) => Task.FromResult(ExcelFileGenerator.GenerateExcelFile(source));

        public void SaveAsFile(byte[] source,string filePath) => ExcelFileGenerator.SaveByteArrayToFileWithStaticMethod(source, filePath);
    
    }
}
