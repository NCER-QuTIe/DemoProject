using AutoMapper;
using Contracts.Logger;
using Contracts.Repositories;
using Contracts.Services;
using DataTransferObjects.Creation;
using DataTransferObjects.Transfer;
using Entities.Models;
using Entities.Exceptions.ExternalTest;
using System.Reflection;
using System.Text.Json;

namespace Service;

public class ExternalTestAdminService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper) : IExternalTestAdminService
{
    private readonly IExternalTestRepository _repo = repositoryManager.ExternalTest;
    private readonly ILoggerManager _logger = loggerManager;
    private readonly IMapper _mapper = mapper;

    public async Task<ExternalTest> CreateExternalTest(ExternalTestCreationDTO externalTest)
    {
        if (externalTest == null) throw new ExternalTestForCreationBadRequestException();

        ExternalTest test = _mapper.Map<ExternalTest>(externalTest);

        return await _repo.CreateExternalTestAsync(test);
    }

    public async Task<ExternalTestDTO> GetExternalTestById(Guid id)
    {
        ExternalTest? test = await _repo.GetExternalTestByIdAsync(id);
        if (test == null) throw new ExternalTestByIdNotFoundException(id);

        return _mapper.Map<ExternalTest, ExternalTestDTO>(test);
    }

    public async Task<ExternalTestDTO> GetExternalTestByName(string name)
    {
        var tests = await _repo.GetExternalTestsByConditionAsync(t => t.Name == name);
        ExternalTest? test = tests.FirstOrDefault();

        if (test == null) throw new ExternalTestByNameNotFoundException(name);

        return _mapper.Map<ExternalTest, ExternalTestDTO>(test);
    }

    public async Task<IEnumerable<ExternalTestDTO>> GetExternalTests()
    {
        var tests = _mapper.Map<IEnumerable<ExternalTest>, IEnumerable<ExternalTestDTO>>(await _repo.GetAllExternalTestsAsync());

        List<ExternalTestDTO> testsToReturn = new(tests);

        testsToReturn.Sort((a, b) => a.Uploaded < b.Uploaded ? 1 : -1);
        return testsToReturn;
    }

    public async Task DeleteExternalTestByIdAsync(Guid id)
    {
        ExternalTest? externalTest = await _repo.GetExternalTestByIdAsync(id);
        if (externalTest == null) throw new ExternalTestDeleteByIdNotFoundException(id);

        await _repo.DeleteExternalTestAsync(externalTest);
    }

    public async Task UpdateExternalTestAsync(Guid id, JsonElement patchObject)
    {
        ExternalTest? existingTest = await _repo.GetExternalTestByIdAsync(id);
        if (existingTest == null) throw new ExternalTestByIdNotFoundException(id);

        var properties = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(patchObject.ToString());

        foreach (var property in properties!)
        {
            var propInfo = typeof(ExternalTest).GetProperty(property.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (propInfo != null && propInfo.CanWrite)
            {
                object? value = JsonSerializer.Deserialize(property.Value.GetRawText(), propInfo.PropertyType);
                propInfo.SetValue(existingTest, value);
            }
        }

        await _repo.UpdateExternalTestAsync(existingTest);
    }
}