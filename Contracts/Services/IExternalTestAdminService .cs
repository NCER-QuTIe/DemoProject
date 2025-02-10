using DataTransferObjects.Creation;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Enums;
using DataTransferObjects.Transfer;
using System.Text.Json;

namespace Contracts.Services;

public interface IExternalTestAdminService
{
    public Task<ExternalTestDTO> GetExternalTestById(Guid id);
    public Task<ExternalTestDTO> GetExternalTestByName(string name);
    public Task<IEnumerable<ExternalTestDTO>> GetExternalTests();
    public Task<ExternalTest> CreateExternalTest(ExternalTestCreationDTO externalTest);
    public Task DeleteExternalTestByIdAsync(Guid id);
    public Task UpdateExternalTestAsync(Guid id, JsonElement pathObject);
}