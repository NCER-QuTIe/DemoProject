using Contracts;
using Contracts.Logger;
using DataTransferObjects.TestResults;
using System.Text.Json;

namespace Service;

public class ContentToExcelService(ILoggerManager logger, IExcelBuilder excelBuilder)
{
    private IExcelBuilder _excelBuilder = excelBuilder;

    public async Task<string> GenerateJsonFileAsync(TestResponseBundleDTO testResponseBundle)
    {
        string json = JsonSerializer.Serialize(testResponseBundle);
        string folderPath = Path.Combine("JsonFiles");
        string filePath = Path.Combine(folderPath, $"data_{Guid.NewGuid()}.json");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        try
        {
            logger.LogInfo("Started generating json file");
            await File.WriteAllTextAsync(filePath, json);
            logger.LogInfo($"Generated json file on {filePath}");
        }
        catch (Exception e)
        {
            logger.LogError($"Error while generating json file. {e.Message} \n Failed content: {JsonSerializer.Serialize(testResponseBundle)}");
            if (!File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            throw;
        }

        return filePath;
    }

    public async Task<string> GenerateExcelFileAsync(TestResponseBundleDTO testResponseBundle)
    {
        string json = JsonSerializer.Serialize(testResponseBundle);
        string folderPath = Path.Combine("ExcelFiles");
        string filePath = Path.Combine(folderPath, $"{testResponseBundle.StudentName}_{Guid.NewGuid()}.xlsx");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        try
        {
            logger.LogInfo("Started generating Excel file");
            await _excelBuilder.GenerateExcelAsync(filePath, testResponseBundle);
            logger.LogInfo($"Generated Excel file on {filePath}");
        }
        catch (Exception e)
        {
            logger.LogError($"Error while generating excel file. {e.Message} \nFailed content: {JsonSerializer.Serialize(testResponseBundle)}");
            if (!File.Exists(filePath))
            {
                //File.Delete(filePath);
            }
            throw;
        }

        return filePath;
    }
}
