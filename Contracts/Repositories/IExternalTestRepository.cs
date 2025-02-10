using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts.Repositories;

public interface IExternalTestRepository
{
    public Task<List<ExternalTest>> GetAllExternalTestsAsync();
    public Task<List<ExternalTest>> GetExternalTestsByConditionAsync(Expression<Func<ExternalTest, bool> > condition);
    public Task<ExternalTest?> GetExternalTestByIdAsync(Guid id);
    public Task<ExternalTest> CreateExternalTestAsync(ExternalTest ExternalTestTest);
    public Task<ExternalTest> UpdateExternalTestAsync(ExternalTest ExternalTestTest);
    public Task DeleteExternalTestAsync(ExternalTest ExternalTestTest);
}
