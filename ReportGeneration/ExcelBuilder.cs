using Contracts;
using Contracts.Logger;
using Contracts.Repositories;
using DataTransferObjects.TestResults;
using Entities.Models;
using OfficeOpenXml;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;

namespace ReportGeneration;

public class ExcelBuilder(IRepositoryManager repositoryManager, ILoggerManager logger) : IExcelBuilder
{
    private IQTITestRepository _repo = repositoryManager.QTITest;
    private ILoggerManager _logger = logger;
     
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
                testName = qtiTest.Name!; // Name
            }

            Dictionary<string, XDocument> testItemContent = GetTestItemXDocuments(qtiTest!);

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
                
                XDocument currentDocument = testItemContent[page.ItemIdentifier!];

                foreach (var item in page.InteractionResponses!)
                {
                    IEnumerable<string> results = item.Value.Select(response => GetElementContent(currentDocument, response));

                    string response = string.Join(", ", results);
                    sheet.Cells[i, k].Value = response;
                    k++;
                }

                pageNumber++;
                i++;
            }
        }
    }

    /// <summary>
    /// Reads an XML file and returns the text content of the element with the specified identifier.
    /// </summary>
    /// <param name="xmlFilePath">The path to the XML file.</param>
    /// <param name="elementId">The identifier to search for (matches the value of the 'id' attribute).</param>
    /// <returns>The text content of the found element, or null if no matching element is found.</returns>
    public string GetElementContent(XDocument doc, string elementId)
    {
        try
        {
            XElement element = doc.Descendants().FirstOrDefault(e => (string)e.Attribute("identifier")! == elementId)!;

            return element?.Value ?? elementId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    private static XDocument ConvertToXDocument(XmlDocument xmlDoc)
    {
        using (XmlNodeReader nodeReader = new XmlNodeReader(xmlDoc))
        {
            nodeReader.MoveToContent();
            return XDocument.Load(nodeReader);
        }
    }

    private Dictionary<string, XDocument> GetTestItemXDocuments(QTITest qtiTest)
    {
        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "TestPackages");
        if(!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string packageDirectoryPath = Path.Combine(directoryPath, $"{Guid.NewGuid()}");

        try
        {
            ExtractZipFromBase64(qtiTest.PackageBase64!, packageDirectoryPath);
            string itemsDirectory = Path.Combine(packageDirectoryPath, "items");
            return LoadQtiDocuments(itemsDirectory);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error extracting test package: {ex.Message}");
            throw;
        }
        finally
        {
            Directory.Delete(packageDirectoryPath, recursive: true);
        }
    }

    /// <summary>
    /// Loads qti.xml from each subdirectory in the provided itemsDirectory.
    /// </summary>
    /// <param name="itemsDirectory">The parent directory containing multiple item directories.</param>
    /// <returns>A dictionary where each key is the item directory's name and each value is the loaded XmlDocument.</returns>
    public Dictionary<string, XDocument> LoadQtiDocuments(string itemsDirectory)
    {
        var result = new Dictionary<string, XDocument>();

        string[] itemDirectories = Directory.GetDirectories(itemsDirectory);

        foreach (string itemDirectory in itemDirectories)
        {
            string xmlFilePath = Path.Combine(itemDirectory, "qti.xml");

            if (File.Exists(xmlFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.Load(xmlFilePath);
                    string folderName = Path.GetFileName(itemDirectory);
                    result.Add(folderName, ConvertToXDocument(xmlDoc));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error loading XML from {xmlFilePath}: {ex.Message}");
                    throw;
                }
            }
            else
            {
                _logger.LogWarning($"No qti.xml file found in directory: {itemDirectory}");
            }
        }

        return result;
    }

    /// <summary>
    /// Extracts a ZIP archive (encoded as a Base64 string) to the specified directory directly from memory.
    /// </summary>
    /// <param name="base64Zip">The Base64-encoded ZIP file content.</param>
    /// <param name="extractPath">The directory path where the ZIP contents will be extracted.</param>
    public static void ExtractZipFromBase64(string base64Zip, string extractPath)
    {
        byte[] zipBytes = Convert.FromBase64String(base64Zip);

        Directory.CreateDirectory(extractPath);

        using MemoryStream ms = new MemoryStream(zipBytes);
        using ZipArchive archive = new ZipArchive(ms);

        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            string destinationPath = Path.Combine(extractPath, entry.FullName);

            if (string.IsNullOrEmpty(entry.Name))
            {
                Directory.CreateDirectory(destinationPath);
                continue;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);

            entry.ExtractToFile(destinationPath, overwrite: true);
        }
    }
}
