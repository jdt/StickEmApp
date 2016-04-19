using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using StickEmApp.Entities;

namespace StickEmApp.Service
{
    [Export(typeof(IExcelExporter))]
    public class ExcelExporter : IExcelExporter
    {
        private readonly IResourceManager _resourceManager;

        [ImportingConstructor]
        public ExcelExporter(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public void Export(IReadOnlyCollection<Vendor> vendors, string destination)
        {
            using (var package = new ExcelPackage(new FileInfo(destination)))
            {
                var worksheet = package.Workbook.Worksheets.Add(_resourceManager.GetString("StickerSales"));
                
                worksheet.Cells[1, 1].Value = _resourceManager.GetString("Name");
                worksheet.Cells[1, 2].Value = _resourceManager.GetString("StickersReceived");
                worksheet.Cells[1, 3].Value = _resourceManager.GetString("StickersReturned");
                worksheet.Cells[1, 4].Value = _resourceManager.GetString("ChangeReceived");
                worksheet.Cells[1, 5].Value = _resourceManager.GetString("AmountRequired");
                worksheet.Cells[1, 6].Value = _resourceManager.GetString("AmountReturned");
                worksheet.Cells[1, 7].Value = _resourceManager.GetString("Difference");
                worksheet.Cells[1, 8].Value = "500";
                worksheet.Cells[1, 9].Value = "200";
                worksheet.Cells[1, 10].Value = "100";
                worksheet.Cells[1, 11].Value = "50";
                worksheet.Cells[1, 12].Value = "20";
                worksheet.Cells[1, 13].Value = "10";
                worksheet.Cells[1, 14].Value = "5";
                worksheet.Cells[1, 15].Value = "2";
                worksheet.Cells[1, 16].Value = "1";
                worksheet.Cells[1, 17].Value = "0.50";
                worksheet.Cells[1, 18].Value = "0.20";
                worksheet.Cells[1, 19].Value = "0.10";
                worksheet.Cells[1, 20].Value = "0.05";
                worksheet.Cells[1, 21].Value = "0.02";
                worksheet.Cells[1, 22].Value = "0.01";
                worksheet.Cells[1, 23].Value = _resourceManager.GetString("StartedAt");

                const int rowOffset = 2;
                var lastRowInSheet = rowOffset;
                for (var i = 0; i < vendors.Count; i++)
                {
                    var vendor = vendors.ElementAt(i);
                    worksheet.Cells[i + rowOffset, 1].Value = vendor.Name;
                    worksheet.Cells[i + rowOffset, 2].Value = vendor.NumberOfStickersReceived;
                    worksheet.Cells[i + rowOffset, 3].Value = vendor.NumberOfStickersReturned;
                    worksheet.Cells[i + rowOffset, 4].Value = vendor.ChangeReceived;
                    worksheet.Cells[i + rowOffset, 5].Value = vendor.CalculateTotalAmountRequired();
                    worksheet.Cells[i + rowOffset, 6].Value = vendor.CalculateTotalAmountReturned();
                    worksheet.Cells[i + rowOffset, 7].Value = vendor.CalculateSalesResult().Difference;
                    worksheet.Cells[i + rowOffset, 8].Value = vendor.AmountReturned.FiveHundreds;
                    worksheet.Cells[i + rowOffset, 9].Value = vendor.AmountReturned.TwoHundreds;
                    worksheet.Cells[i + rowOffset, 10].Value = vendor.AmountReturned.Hundreds;
                    worksheet.Cells[i + rowOffset, 11].Value = vendor.AmountReturned.Fifties;
                    worksheet.Cells[i + rowOffset, 12].Value = vendor.AmountReturned.Twenties;
                    worksheet.Cells[i + rowOffset, 13].Value = vendor.AmountReturned.Tens;
                    worksheet.Cells[i + rowOffset, 14].Value = vendor.AmountReturned.Fives;
                    worksheet.Cells[i + rowOffset, 15].Value = vendor.AmountReturned.Twos;
                    worksheet.Cells[i + rowOffset, 16].Value = vendor.AmountReturned.Ones;
                    worksheet.Cells[i + rowOffset, 17].Value = vendor.AmountReturned.FiftyCents;
                    worksheet.Cells[i + rowOffset, 18].Value = vendor.AmountReturned.TwentyCents;
                    worksheet.Cells[i + rowOffset, 19].Value = vendor.AmountReturned.TenCents;
                    worksheet.Cells[i + rowOffset, 20].Value = vendor.AmountReturned.FiveCents;
                    worksheet.Cells[i + rowOffset, 21].Value = vendor.AmountReturned.TwoCents;
                    worksheet.Cells[i + rowOffset, 22].Value = vendor.AmountReturned.OneCents;
                    worksheet.Cells[i + rowOffset, 23].Value = vendor.StartedAt.ToString("dd-MM-yyyy");

                    lastRowInSheet = i + rowOffset;
                }

                using (var range = worksheet.Cells[1, 1, 1, 23])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Black);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                worksheet.Cells[2, 4, lastRowInSheet, 7].Style.Numberformat.Format = "#,##0.00";

                worksheet.Cells.AutoFitColumns(0);

                package.Save();
            }
        }
    }
}