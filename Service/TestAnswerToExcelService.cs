using Contracts.Logger;
using DataTransferObjects.TestAnswer;
using System.Text.Json;

namespace Service;

public class ContentToExcelService(ILoggerManager logger)
{
    /// <summary>
    /// Generate Excel file from test response bundle
    /// </summary>
    /// <param name="testResponseBundle"></param>
    /// <returns>Address of the excel file</returns>
    public async Task<string> GenerateExcelFileAsync(TestResponseBundleDTO testResponseBundle)
    {
        string json = JsonSerializer.Serialize(testResponseBundle);
        string folderPath = Path.Combine("JsonFiles");
        string filePath = Path.Combine(folderPath, $"data_{Guid.NewGuid()}.json");
        
        if(!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        try
        {
            logger.LogInfo("Started generating json file");

            await File.WriteAllTextAsync(filePath, json);

            logger.LogInfo($"Generating json file on {filePath}");
        }
        catch(Exception e)
        {
            logger.LogError($"Error while generating json file. {e.Message}");
            throw;
        }

        return filePath;
    }
}
