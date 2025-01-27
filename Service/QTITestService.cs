using AutoMapper;
using Contracts.Logger;
using Contracts.Repositories;
using Contracts.Services;
using DataTransferObjects.Transfer;
using Entities.Enums;
using Entities.Exceptions;
using Entities.Models;

namespace Service;

public class QTITestService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper) : IQTITestService
{
    private readonly IQTITestRepository _repo = repositoryManager.QTITest;
    private readonly ILoggerManager _logger = loggerManager;
    private readonly IMapper _mapper = mapper;


    public async Task<QTITestDTO> GetQTITestById(Guid id)
    {
        QTITest? test = await _repo.GetQTITestByIdAsync(id);

        if (test == null) throw new QTITestByIdNotFoundException(id);
        if (test.Status == TestStatusEnum.InActive) throw new QTITestByIdNotAccessibleException(id);

        return _mapper.Map<QTITest, QTITestDTO>(test);
    }

    public async Task<QTITestDTO> GetQTITestByName(string name)
    {
        IEnumerable<QTITest> tests = await _repo.GetQTITestsByConditionAsync(t => t.Name == name);
        QTITest? test = tests.FirstOrDefault();

        if (test == null) throw new QTITestByNameNotFoundException(name);
        if (test.Status == TestStatusEnum.InActive) throw new QTITestByNameNotAccessibleException(name);

        return _mapper.Map<QTITest, QTITestDTO>(test);
    }

    public async Task<IEnumerable<QTITestDTO>> GetQTITests()
    {
        var tests = _mapper.Map<IEnumerable<QTITest>, IEnumerable<QTITestDTO>>(await _repo.GetQTITestsByConditionAsync(t => t.Status == TestStatusEnum.Active));

        List<QTITestDTO> testsToReturn = new();
        foreach(var test in tests)
        {
            testsToReturn.Add(test with { PackageBase64 = "EMPTY"});
        }
        testsToReturn.Sort((a, b) => a.Uploaded < b.Uploaded ? 1 : -1);
        return testsToReturn;
    }
}