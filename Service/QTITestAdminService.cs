﻿using AutoMapper;
using Contracts.Logger;
using Contracts.Repositories;
using Contracts.Services;
using DataTransferObjects.Creation;
using DataTransferObjects.Transfer;
using Entities.Models;
using Entities.Exceptions.QTITest;
using System.Reflection;
using System.Text.Json;

namespace Service;

public class QTITestAdminService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper, ICreateQTIProcessorService qtiPreProcessor) : IQTITestAdminService
{
    private readonly IQTITestRepository _repo = repositoryManager.QTITest;
    private readonly ILoggerManager _logger = loggerManager;
    private readonly IMapper _mapper = mapper;
    private readonly ICreateQTIProcessorService _qtiPreProcessor = qtiPreProcessor;

    public async Task<QTITest> CreateQTITest(QTITestCreationDTO qtiTest)
    {
        if (qtiTest == null) throw new QTITestForCreationBadRequestException();

        QTITest test = await _qtiPreProcessor.PreProcessAsync(qtiTest);

        return await _repo.CreateQTITestAsync(test);
    }

    public async Task<QTITestDTO> GetQTITestById(Guid id)
    {
        QTITest? test = await _repo.GetQTITestByIdAsync(id);
        if (test == null) throw new QTITestByIdNotFoundException(id);

        return _mapper.Map<QTITest, QTITestDTO>(test);
    }

    public async Task<QTITestDTO> GetQTITestByName(string name)
    {
        var tests = await _repo.GetQTITestsByConditionAsync(t => t.Name == name);
        QTITest? test = tests.FirstOrDefault();

        if (test == null) throw new QTITestByNameNotFoundException(name);

        return _mapper.Map<QTITest, QTITestDTO>(test);
    }

    public async Task<IEnumerable<QTITestDTO>> GetQTITests()
    {
        var tests = _mapper.Map<IEnumerable<QTITest>, IEnumerable<QTITestDTO>>(await _repo.GetAllQTITestsAsync());

        List<QTITestDTO> testsToReturn = new();
        foreach (var test in tests)
        {
            testsToReturn.Add(test with { PackageBase64 = "EMPTY" });
        }
        testsToReturn.Sort((a, b) => a.Uploaded < b.Uploaded ? 1 : -1);
        return testsToReturn;
    }

    public async Task DeleteQTITestByIdAsync(Guid id)
    {
        QTITest? qtiTest = await _repo.GetQTITestByIdAsync(id);
        if (qtiTest == null) throw new QTITestDeleteByIdNotFoundException(id);

        await _repo.DeleteQTITestAsync(qtiTest);
    }

    public async Task UpdateQTITestAsync(Guid id, JsonElement patchObject)
    {
        QTITest? existingTest = await _repo.GetQTITestByIdAsync(id);
        if (existingTest == null) throw new QTITestByIdNotFoundException(id);

        var properties = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(patchObject.ToString());

        bool needToUpdatePackage = false;
        foreach (var property in properties!)
        {
            var propInfo = typeof(QTITest).GetProperty(property.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (propInfo != null && propInfo.CanWrite)
            {
                if(propInfo.Name.Equals("packagebase64", StringComparison.CurrentCultureIgnoreCase))
                {
                    needToUpdatePackage = true;
                }
                object? value = JsonSerializer.Deserialize(property.Value.GetRawText(), propInfo.PropertyType);
                propInfo.SetValue(existingTest, value);
            }
        }
        if (needToUpdatePackage)
        {
            existingTest.PackageBase64 = await _qtiPreProcessor.ConvertQTIPackageAsync(existingTest.PackageBase64!);
        }
        
        await _repo.UpdateQTITestAsync(existingTest);
    }
}