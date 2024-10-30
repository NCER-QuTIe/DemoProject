using DataTransferObjects.Creation;
using DataTransferObjects.Transfer;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Services;

public interface IQTITestService
{
    public Task<QTITestDTO> GetQTITestById(Guid id);
    public Task<QTITestDTO> GetQTITestByName(string name);
    public Task<IEnumerable<QTITestDTO>> GetQTITests();
}