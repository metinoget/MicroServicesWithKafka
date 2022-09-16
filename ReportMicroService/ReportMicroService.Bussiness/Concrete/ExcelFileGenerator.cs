using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ReportMicroService.Entities.Model;

namespace ReportMicroService.Concrete
{
    public static class ExcelFileGenerator
    {

        public static byte[] GenerateExcelFile(ContactReport source)
        {
            try
            {
                var memoryStream = new MemoryStream();

                using var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());

                sheets.AppendChild(new Sheet
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Report"
                });

                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                var row1 = new Row();
                row1.AppendChild(new Cell
                {
                    CellValue = new CellValue("Konum"),
                    DataType = CellValues.String
                });
                row1.AppendChild(new Cell
                {
                    CellValue = new CellValue("Konumda yer alan rehbere kayıtlı kişi sayısı"),
                    DataType = CellValues.String
                });
                row1.AppendChild(new Cell
                {
                    CellValue = new CellValue(" O konumda yer alan rehbere kayıtlı telefon numarası sayısı"),
                    DataType = CellValues.String
                });
                sheetData?.AppendChild(row1);

                var row2 = new Row();

                row2.AppendChild(new Cell
                {
                    CellValue = new CellValue(source.Location),
                    DataType = CellValues.String
                });
                row2.AppendChild(new Cell
                {
                    CellValue = new CellValue(source.NearbyPeopleCount),
                    DataType = CellValues.String
                });
                row2.AppendChild(new Cell
                {
                    CellValue = new CellValue(source.NearbySavedPhoneCount),
                    DataType = CellValues.String
                });
                sheetData?.AppendChild(row2);
                document.Save();
                document.Close();
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Excel creation error: " + ex.Message);
                return null;
            }
        }

        public static void SaveByteArrayToFileWithStaticMethod(byte[] data, string filePath)
  => File.WriteAllBytes(filePath, data);
    }
}