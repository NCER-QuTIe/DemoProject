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

public interface IQTITestAdminService
{
    public Task<QTITestDTO> GetQTITestById(Guid id);
    public Task<QTITestDTO> GetQTITestByName(string name);
    public Task<IEnumerable<QTITestDTO>> GetQTITests();
    public Task<QTITest> CreateQTITest(QTITestCreationDTO qtiTest);
    public Task DeleteQTITestByIdAsync(Guid id);
    public Task UpdateQTITestAsync(Guid id, JsonElement pathObject);
}