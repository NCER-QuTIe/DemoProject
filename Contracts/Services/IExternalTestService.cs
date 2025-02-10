using DataTransferObjects.Creation;
using DataTransferObjects.Transfer;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Services;

public interface IExternalTestService
{
    public Task<ExternalTestDTO> GetExternalTestById(Guid id);
    public Task<ExternalTestDTO> GetExternalTestByName(string name);
    public Task<IEnumerable<ExternalTestDTO>> GetExternalTests();
}