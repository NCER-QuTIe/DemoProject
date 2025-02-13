using Amazon.Runtime.Internal.Util;
using Amazon.S3.Transfer;
using AutoMapper;
using Contracts.Logger;
using Contracts.Repositories;
using DataTransferObjects.Creation;
using Entities.Exceptions;
using Entities.Models;
using LoggerService;
using Microsoft.Extensions.Configuration;
using Redis.OM.Common;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Service;

public class CreateQTIProcessorService : ICreateQTIProcessorService
{
    private readonly string S3BaseURL = "https://s3.amazonaws.com/assessmentqti/";
    private readonly HttpClient _client = new();
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;


    public CreateQTIProcessorService(IConfiguration configuration, IMapper mapper, ILoggerManager logger)
    {
        _client.BaseAddress = new Uri(configuration["ConverterAPIBaseURL"]!);
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Converts QTI package version to 3.0
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public async Task<QTITest> PreProcessAsync(QTITestCreationDTO test)
    {
        //Guid urlGuid = new Guid();
        //string rootS3URL = $"{S3BaseURL}{urlGuid}/";

        test.PackageBase64 = await ConvertQTIPackageAsync(test.PackageBase64!);

        return _mapper.Map<QTITestCreationDTO, QTITest>(test);

        //processedQTITest.RootS3URLGuid = urlGuid.ToString();
        //processedQTITest.S3Bucket = S3BaseURL;

    }


    //TODO: Implement PostProcessAsync
    /// <summary>
    /// Uploads converted QTI Package files to s3 bucket
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public async Task<QTITest> PostProcessAsync(QTITest test)
    {
        //string base64EncodedZip = test.PackageBase64!;
        //string extractPath = Path.Combine(Directory.GetCurrentDirectory(), "ExtractedFiles");
        //byte[] zipBytes = Convert.FromBase64String(base64EncodedZip);
        //string tempZipPath = Path.Combine(Directory.GetCurrentDirectory(), $"{test.RootS3URLGuid}.zip");
        //File.WriteAllBytes(tempZipPath, zipBytes);

        //_logger.LogInfo($"decoding base64 to zip file in {tempZipPath}. Test name - {test.Name}, Test id - {test.Id}");

        //if (Directory.Exists(extractPath))
        //{
        //    Directory.Delete(extractPath, true); // Clean up old files if the directory exists
        //}
        //Directory.CreateDirectory(extractPath); // Ensure the directory exists
        //ZipFile.ExtractToDirectory(tempZipPath, extractPath);

        //_logger.LogInfo($"Extracted zip file to {extractPath}. Test name - {test.Name}, Test id - {test.Id}");

        //try
        //{
        //    bool isUploaded = await UploadToS3(extractPath, test);
        //}
        //finally {
        //    File.Delete(tempZipPath);
        //    Directory.Delete(extractPath, true);
        //}
        throw new NotImplementedException();
    }

    //private async Task<bool> UploadToS3(string directoryPath, QTITest test)
    //{

    //}

    public async Task<String> ConvertQTIPackageAsync(string Base64Package)
    {
        var requestContent = new StringContent(JsonSerializer.Serialize(new { zipFile = Base64Package }), Encoding.UTF8, "application/json");
        HttpResponseMessage response;

        try
        {
            response = await _client.PostAsync($"/convert", requestContent);
        }
        catch(Exception e)
        {
            throw new ConverterAPIServiceConnectionException(e);
        }

        EnsureStatusCode(response);
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseJson = JsonSerializer.Deserialize<Dictionary<String, String>>(responseBody);
        return responseJson!["zipFile"];
    }

    private bool EnsureStatusCode(HttpResponseMessage message)
    {
        var statusCode = message.StatusCode;
        if (statusCode == HttpStatusCode.OK) return true;
        
        var errorMessage = message.Content.ReadAsStringAsync().Result;


        bool isValid = statusCode switch
        {
            HttpStatusCode.BadRequest => throw new ConverterAPIServiceBadRequestException(errorMessage),
            HttpStatusCode.InternalServerError => throw new ConverterAPIServiceException($"There was an internal server error detected in the converter API. {errorMessage}"),
            _ => throw new ConverterAPIServiceException($"There was an unknown server error detected in the converter API. {errorMessage}")
        };

        return isValid;
    }
}