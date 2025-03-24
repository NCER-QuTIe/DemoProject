using Contracts.Repositories;
using Entities.Models;
using Redis.OM.Contracts;
using Redis.OM.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class QTITestRepository(IRedisConnectionProvider provider) : RepositoryBase<QTITest>(provider), IQTITestRepository
{
    private IRedisCollection<QTITest> NewCollection => provider.RedisCollection<QTITest>();

    public async Task<QTITest> CreateQTITestAsync(QTITest qtiTest)
    {
        await CreateAsync(qtiTest);
        return qtiTest;
    }

    public async Task DeleteQTITestAsync(QTITest qtiTest)
    {
        await DeleteAsync(qtiTest);
    }

    public  async Task<List<QTITest>> GetAllQTITestsAsync()
    {
        return await GetAllQTITestsWithoutPackageAsync();
    }

    public async Task<QTITest?> GetQTITestByIdAsync(Guid id)
    {
        return (await FindByConditionAsync(t => t.Id == id)).FirstOrDefault();
    }

    public async Task<List<QTITest>> GetQTITestsByConditionAsync(Expression<Func<QTITest, bool>> condition)
    {
        return [.. await FindByConditionAsync(condition)];
    }

    public async Task<QTITest> UpdateQTITestAsync(QTITest qtiTest)
    {
        await UpdateAsync(qtiTest);
        return qtiTest;
    }

    private async Task<List<QTITest>> GetAllQTITestsWithoutPackageAsync()
    {
        var collection = NewCollection;

        return [.. collection.Select(t => new QTITest
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            Status = t.Status,
            Uploaded = t.Uploaded,
            PackageBase64 = "EMPTY"
        })];
    }
}
