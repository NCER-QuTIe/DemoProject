using Contracts;
using Contracts.Repositories;
using DataTransferObjects.TestResults;
using OfficeOpenXml;

namespace ReportGeneration;

public class ExcelBuilder(IRepositoryManager repositoryManager) : IExcelBuilder
{
    IQTITestRepository _repo = repositoryManager.QTITest;

    public async Task GenerateExcelAsync(string outputPath, TestResponseBundleDTO testResponseBundle)
    {

        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template.xlsx");
        FileInfo templateFile = new FileInfo(templatePath);
        FileInfo outputFile = new FileInfo(outputPath);
        Directory.CreateDirectory(outputFile.DirectoryName!);

        using var package = new ExcelPackage(outputFile, templateFile);
        var worksheets = package.Workbook.Worksheets;

        var summarySheet = package.Workbook.Worksheets["Sheet1"];
        var testSheet = package.Workbook.Worksheets["Sheet2"];
        var pageSheet = package.Workbook.Worksheets["Sheet3"];

        if (summarySheet == null || testSheet == null || pageSheet == null)
        {
            throw new Exception("Template file is missing required sheets");
        }

        FillSummarySheet(summarySheet, testResponseBundle);
        await FillTestSheetAsync(testSheet, testResponseBundle);
        await FillPageSheetAsync(pageSheet, testResponseBundle);

        package.SaveAs(outputFile);
    }

    private void FillSummarySheet(ExcelWorksheet sheet, TestResponseBundleDTO content)
    {
        sheet.Cells["A2"].Value = content.StudentName;
        sheet.Cells["B2"].Value = content.TestResponses!.Count;
        sheet.Cells["C2"].Value = content.TestResponses!.Sum(x => x.ItemResponses!.Sum(x => x.Points!.Received));
        sheet.Cells["D2"].Value = content.TestResponses!.Sum(x => x.ItemResponses!.Sum(x => x.Points!.Maximal));
    }

    private async Task FillTestSheetAsync(ExcelWorksheet sheet, TestResponseBundleDTO content)
    {
        int i = 2;
        foreach (var test in content.TestResponses!)
        {
            DateTime startDate = DateTimeOffset.FromUnixTimeMilliseconds(test.StartTimeEpoch).UtcDateTime.AddHours(4); // Georgian time zone
            DateTime endDate = DateTimeOffset.FromUnixTimeMilliseconds(test.EndTimeEpoch).UtcDateTime.AddHours(4); // Georgian time zone

            sheet.Cells[i, 1].Value = i - 1; // Index

            var qtiTest = (await _repo.GetQTITestByIdAsync(test.TestId));
            if (qtiTest != null)
            {
                sheet.Cells[i, 2].Value = qtiTest.Name; // Name
            }

            sheet.Cells[i, 3].Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss";
            sheet.Cells[i, 3].Value = startDate;
            
            sheet.Cells[i, 4].Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss";
            sheet.Cells[i, 4].Value = endDate;
            
            sheet.Cells[i, 5].Style.Numberformat.Format = @"hh\:mm\:ss";
            sheet.Cells[i, 5].Value = (endDate - startDate).ToString(@"hh\:mm\:ss");

            sheet.Cells[i, 6].Value = test.ItemResponses!.Count; // Number of pages
            sheet.Cells[i, 7].Value = test.ItemResponses!.Sum(x => x.Points!.Received); // Received points
            sheet.Cells[i, 8].Value = test.ItemResponses!.Sum(x => x.Points!.Maximal); // Max possible points
            sheet.Cells[i, 9].Value = $"https://test.ncer.gov.ge/#/test/{test.TestId}"; // Link to test

            i++;
        }
    }

    private async Task FillPageSheetAsync(ExcelWorksheet sheet, TestResponseBundleDTO content)
    {
        int i = 2;
        foreach (var test in content.TestResponses!)
        {
            string testName = "";
            var qtiTest = (await _repo.GetQTITestByIdAsync(test.TestId));
            if (qtiTest != null)
            {
                testName = qtiTest.Name; // Name
            }


            int pageNumber = 1;
            foreach (var page in test.ItemResponses!)
            {
                DateTime startDate = DateTimeOffset.FromUnixTimeMilliseconds(test.StartTimeEpoch).UtcDateTime.AddHours(4); // Georgian time zone
                DateTime endDate = DateTimeOffset.FromUnixTimeMilliseconds(test.EndTimeEpoch).UtcDateTime.AddHours(4); // Georgian time zone

                sheet.Cells[i, 1].Value = testName;
                sheet.Cells[i, 2].Value = pageNumber;
                sheet.Cells[i, 3].Style.Numberformat.Format = @"hh\:mm\:ss";
                sheet.Cells[i, 3].Value = (endDate - startDate).ToString(@"hh\:mm\:ss");
                sheet.Cells[i, 4].Value = page.Points!.Received;
                sheet.Cells[i, 5].Value = page.Points!.Maximal;

                int k = 6;
                foreach (var item in page.ItemResponses!)
                {
                    sheet.Cells[i, k].Value = item.Value;
                    k++;
                }

                pageNumber++;
                i++;
            }

        }
    }
}
