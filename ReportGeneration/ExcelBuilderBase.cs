using Contracts;
using DataTransferObjects.TestResults;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using Contracts.Logger;
using Contracts.Repositories;

namespace ReportGeneration;

public abstract class ExcelBuilderBase(IRepositoryManager repositoryManager, ILoggerManager logger) : IExcelBuilder
{
    private IQTITestRepository _repo = repositoryManager.QTITest;
    private ILoggerManager _logger = logger;

    public abstract Task GenerateExcelAsync(string outputPath, TestResponseBundleDTO testResponseBundle);

    /// <summary>
    /// Reads an XML file and returns the text content of the element with the specified identifier.
    /// </summary>
    /// <param name="xmlFilePath">The path to the XML file.</param>
    /// <param name="elementId">The identifier to search for (matches the value of the 'id' attribute).</param>
    /// <returns>The text content of the found element, or null if no matching element is found.</returns>
    protected string GetElementContent(XDocument doc, string elementId)
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

    protected static XDocument ConvertToXDocument(XmlDocument xmlDoc)
    {
        using (XmlNodeReader nodeReader = new XmlNodeReader(xmlDoc))
        {
            nodeReader.MoveToContent();
            return XDocument.Load(nodeReader);
        }
    }

    protected Dictionary<string, XDocument> GetTestItemXDocuments(QTITest qtiTest)
    {
        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "TestPackages");
        if (!Directory.Exists(directoryPath))
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
        catch (Exception ex)
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
    protected Dictionary<string, XDocument> LoadQtiDocuments(string itemsDirectory)
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
    protected static void ExtractZipFromBase64(string base64Zip, string extractPath)
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
