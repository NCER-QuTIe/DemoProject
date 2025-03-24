using Contracts.Logger;
using Contracts.Repositories;
using DataTransferObjects.TestResults;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace ReportGeneration;

public class ExcelBuilderV2(IRepositoryManager repositoryManager, ILoggerManager logger) : ExcelBuilderBase(repositoryManager, logger)
{
    public override async Task GenerateExcelAsync(string outputPath, TestResponseBundleDTO testResponseBundle)
    {
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template2.xlsx");
        FileInfo templateFile = new FileInfo(templatePath);
        FileInfo outputFile = new FileInfo(outputPath);
        Directory.CreateDirectory(outputFile.DirectoryName!);

        using var package = new ExcelPackage(outputFile, templateFile);
        var worksheets = package.Workbook.Worksheets;
        var sheet = package.Workbook.Worksheets["Sheet1"];

        PopulateHeader(sheet, testResponseBundle);
        await PopulateTests(sheet, testResponseBundle);

        package.SaveAs(outputFile);
    }

    private void PopulateHeader(ExcelWorksheet sheet, TestResponseBundleDTO content)
    {
        sheet.Cells["A2"].Value = content.StudentName;
        sheet.Cells["B2"].Value = content.TestResponses!.Count;
        try
        {
            sheet.Cells["C2"].Value = content.TestResponses!.Sum(x => x.ItemResponses!.Sum(x => x.Points!.Received));
        }
        catch
        {
            sheet.Cells["C2"].Value = 0;
        }

        try
        {
            sheet.Cells["D2"].Value = content.TestResponses!.Sum(x => x.ItemResponses!.Sum(x => x.Points!.Maximal));
        }
        catch
        {
            sheet.Cells["D2"].Value = content.TestResponses!.Sum(x => x.ItemResponses!.Sum(x => x.Points!.Maximal));
        }
    }

    private async Task PopulateTests(ExcelWorksheet sheet, TestResponseBundleDTO content)
    {

        List<ExcelRowV2> rows = await GetExcelRowsAsync(sheet, content);
        OutlineBorder(6, 1, 6 + rows.Count, 9, sheet);

        for (int i = 0; i < rows.Count; i++)
        {
            var row = rows[i];

            int rowIndex = i + 7;

            sheet.Cells[$"A{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[$"A{rowIndex}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[$"B{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[$"B{rowIndex}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[$"C{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[$"C{rowIndex}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[$"D{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[$"D{rowIndex}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[$"E{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[$"E{rowIndex}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[$"F{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[$"F{rowIndex}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[$"G{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[$"G{rowIndex}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[$"H{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[$"H{rowIndex}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[$"I{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[$"I{rowIndex}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[$"A{rowIndex}"].Value = row.TestName;
            sheet.Cells[$"A{rowIndex}"].Hyperlink = new Uri(row.Link);

            if (rows.Exists(r => r.TestName == row.TestName && r.ItemNumber == row.ItemNumber && r.QuestionNumber > 1))
            {
                sheet.Cells[$"B{rowIndex}"].Value = "შეკითხვა " + row.ItemNumber + '.' + row.QuestionNumber;
            }
            else
            {
                sheet.Cells[$"B{rowIndex}"].Value = "შეკითხვა " + row.ItemNumber;
            }

            sheet.Cells[$"C{rowIndex}"].Value = row.StudentAnswer;
            sheet.Cells[$"C{rowIndex}"].Style.WrapText = true;
            sheet.Cells[$"C{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[$"C{rowIndex}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[$"D{rowIndex}"].Value = row.CorrectAnswer;
            sheet.Cells[$"D{rowIndex}"].Style.WrapText = true;
            sheet.Cells[$"D{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[$"D{rowIndex}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Row(rowIndex).CustomHeight = false;


            if (row.CorrectAnswer != "")
            {
                sheet.Cells["F" + rowIndex].Value = 1;
                if (row.Correct)
                {
                    sheet.Cells["E" + rowIndex].Value = 1;
                }
                else
                {
                    sheet.Cells["E" + rowIndex].Value = 0;
                }
            }

            sheet.Cells[$"G{rowIndex}"].Style.Numberformat.Format = "hh:mm:ss";
            sheet.Cells[$"G{rowIndex}"].Value = row.TimeTaken;

            if (DateTimeOffset.FromUnixTimeMilliseconds(0).UtcDateTime != row.DateStarted)
            {

                sheet.Cells[$"H{rowIndex}"].Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss";
                sheet.Cells[$"I{rowIndex}"].Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss";
                sheet.Cells[$"H{rowIndex}"].Value = row.DateStarted;
                sheet.Cells[$"I{rowIndex}"].Value = row.DateEnded;
            }
            else
            {
                sheet.Cells[$"H{rowIndex}"].Value = "";
                sheet.Cells[$"I{rowIndex}"].Value = "";
            }

            if (row.Open == false)
            {
                if (row.Correct == true)
                {
                    sheet.Cells[$"C{rowIndex}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[$"C{rowIndex}"].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                }
                else
                {
                    sheet.Cells[$"C{rowIndex}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[$"C{rowIndex}"].Style.Fill.BackgroundColor.SetColor(Color.PaleVioletRed);
                }
            }
        }
    }

    private async Task<List<ExcelRowV2>> GetExcelRowsAsync(ExcelWorksheet sheet, TestResponseBundleDTO content)
    {
        List<ExcelRowV2> rows = new List<ExcelRowV2>();

        foreach (var testResponse in content.TestResponses!)
        {
            ExcelRowV2 row = new ExcelRowV2();
            var qtiTest = (await _repo.GetQTITestByIdAsync(testResponse.TestId));
            row.TestName = qtiTest!.Name!;
            row.Link = $"https://test.ncer.gov.ge/#/test/{qtiTest.Id}";

            Dictionary<string, XDocument> testItemContent = GetTestItemXDocuments(qtiTest!);

            foreach (var item in testResponse.ItemResponses!)
            {
                XDocument currentDocument = testItemContent[item.ItemIdentifier!];

                ExcelRowV2 itemRow = row;
                itemRow.ItemNumber = item.ItemNumber + 1;

                int interactionIndex = 0;
                foreach (var interaction in item.InteractionResponses!)
                {
                    ExcelRowV2 questionRow = itemRow;

                    if (questionRow.ItemNumber == 1 && interactionIndex == 0)
                    {
                        questionRow.DateStarted = DateTimeOffset.FromUnixTimeMilliseconds(testResponse.StartTimeEpoch).UtcDateTime.AddHours(4);
                        questionRow.DateEnded = DateTimeOffset.FromUnixTimeMilliseconds(testResponse.EndTimeEpoch).UtcDateTime.AddHours(4);
                    }
                    else
                    {
                        questionRow.DateStarted = DateTimeOffset.FromUnixTimeMilliseconds(0).UtcDateTime;
                        questionRow.DateEnded = DateTimeOffset.FromUnixTimeMilliseconds(0).UtcDateTime;
                    }

                    if (interactionIndex == 0)
                    {
                        questionRow.PointsReceived = item.Points!.Received;
                        questionRow.PointsMaximal = item.Points!.Maximal;
                        questionRow.TimeTaken = new TimeOnly(0, 0, item.DurationSeconds);
                    }
                    else
                    {
                        questionRow.PointsReceived = -1;
                        questionRow.PointsMaximal = -1;
                    }

                    questionRow.QuestionNumber = ++interactionIndex;
                    questionRow.StudentAnswer = string.Join("\n ", interaction.Value.Select(response => GetElementContent(currentDocument, response)).Order());
                    questionRow.CorrectAnswer = string.Join("\n ", GetCorrectResponse(currentDocument, interaction.Key).Order());

                    var orderedAnswers = interaction.Value.Order();

                    if (questionRow.CorrectAnswer == "")
                    {
                        questionRow.Open = true;
                    }
                    else if (questionRow.StudentAnswer == questionRow.CorrectAnswer)
                    {
                        questionRow.Correct = true;
                    }

                    rows.Add(questionRow);
                }
            }
        }

        return rows;
    }

    public static void OutlineBorder(int startRow, int startCol, int endRow, int endCol, ExcelWorksheet ws)
    {
        // Define the rectangular range.
        var tableRange = ws.Cells[startRow, startCol, endRow, endCol];
        
        // Apply border styling so each cell in the range has a black outline.
        tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        tableRange.Style.Border.Top.Color.SetColor(Color.Black);
        tableRange.Style.Border.Left.Color.SetColor(Color.Black);
        tableRange.Style.Border.Right.Color.SetColor(Color.Black);
        tableRange.Style.Border.Bottom.Color.SetColor(Color.Black);
    }
}
