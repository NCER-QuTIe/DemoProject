using AutoMapper;
using Contracts.Logger;
using Contracts.Repositories;
using Contracts.Services;
using DataTransferObjects.Transfer;
using Entities.Enums;
using Entities.Exceptions.ExternalTest;
using Entities.Models;

namespace Service;

public class ExternalTestService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper) : IExternalTestService
{
    private readonly IExternalTestRepository _repo = repositoryManager.ExternalTest;
    private readonly ILoggerManager _logger = loggerManager;
    private readonly IMapper _mapper = mapper;


    public async Task<ExternalTestDTO> GetExternalTestById(Guid id)
    {
        ExternalTest? test = await _repo.GetExternalTestByIdAsync(id);

        if (test == null) throw new ExternalTestByIdNotFoundException(id);
        if (test.Status == TestStatusEnum.InActive) throw new ExternalTestByIdNotAccessibleException(id);

        return _mapper.Map<ExternalTest, ExternalTestDTO>(test);
    }

    public async Task<ExternalTestDTO> GetExternalTestByName(string name)
    {
        IEnumerable<ExternalTest> tests = await _repo.GetExternalTestsByConditionAsync(t => t.Name == name);
        ExternalTest? test = tests.FirstOrDefault();

        if (test == null) throw new ExternalTestByNameNotFoundException(name);
        if (test.Status == TestStatusEnum.InActive) throw new ExternalTestByNameNotAccessibleException(name);

        return _mapper.Map<ExternalTest, ExternalTestDTO>(test);
    }

    public async Task<IEnumerable<ExternalTestDTO>> GetExternalTests()
    {
        var tests = _mapper.Map<IEnumerable<ExternalTest>, IEnumerable<ExternalTestDTO>>(await _repo.GetExternalTestsByConditionAsync(t => t.Status == TestStatusEnum.Active));

        List<ExternalTestDTO> testsToReturn = new(tests);
        testsToReturn.Sort((a, b) => a.Uploaded < b.Uploaded ? 1 : -1);
        return testsToReturn;
    }
}