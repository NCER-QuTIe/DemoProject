using AutoMapper;
using Contracts.Logger;
using Contracts.Repositories;
using Contracts.Services;
using DataTransferObjects.Creation;
using Entities.Models;
using System;
using Entities.Enums;
using System.Collections.Generic;
using Entities.Exceptions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects.Transfer;

namespace Service;

public class QTITestAdminService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper) : IQTITestAdminService
{
    private readonly IQTITestRepository _repo = repositoryManager.QTITest;
    private readonly ILoggerManager _logger = loggerManager;
    private readonly IMapper _mapper = mapper;

    public async Task<QTITest> CreateQTITest(QTITestCreationDTO qtiTest)
    {
        if (qtiTest == null) throw new QTITestForCreationBadRequestException();

        return await _repo.CreateQTITestAsync(_mapper.Map<QTITestCreationDTO, QTITest>(qtiTest));
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
        return _mapper.Map<IEnumerable<QTITest>, IEnumerable<QTITestDTO>>(await _repo.GetAllQTITestsAsync());
    }

    public async Task PatchQTITestStatus(Guid id, TestStatusEnum status)
    {
        QTITest? test = await _repo.GetQTITestByIdAsync(id);
        if (test == null) throw new QTITestByIdNotFoundException(id);

        try
        {
            test.Status = status;
            await _repo.UpdateQTITestAsync(test);
        }
        catch(Exception e)
        {
            throw new QTITestStatusPatchException(id, status, e);
        }
    }

    public async Task DeleteQTITestByIdAsync(Guid id)
    {
        QTITest? qtiTest = await _repo.GetQTITestByIdAsync(id);
        if (qtiTest == null) throw new QTITestDeleteByIdNotFoundException(id);
        
        await _repo.DeleteQTITestAsync(qtiTest);
    }
}