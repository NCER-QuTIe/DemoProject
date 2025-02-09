using Contracts.Repositories;
using Entities.Models;
using Redis.OM.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class ExternalTestRepository(IRedisConnectionProvider provider) : RepositoryBase<ExternalTest>(provider), IExternalTestRepository
{
    public async Task<ExternalTest> CreateExternalTestAsync(ExternalTest externalTest)
    {
        await CreateAsync(externalTest);
        return externalTest;
    }

    public async Task DeleteExternalTestAsync(ExternalTest externalTest)
    {
        await DeleteAsync(externalTest);
    }

    public  async Task<List<ExternalTest>> GetAllExternalTestsAsync()
    {
        return [.. await FindAllAsync()];
    }

    public async Task<ExternalTest?> GetExternalTestByIdAsync(Guid id)
    {
        return (await FindByConditionAsync(t => t.Id == id)).FirstOrDefault();
    }

    public async Task<List<ExternalTest>> GetExternalTestsByConditionAsync(Expression<Func<ExternalTest, bool>> condition)
    {
        return [.. await FindByConditionAsync(condition)];
    }

    public async Task<ExternalTest> UpdateExternalTestAsync(ExternalTest externalTest)
    {
        await UpdateAsync(externalTest);
        return externalTest;
    }
}
