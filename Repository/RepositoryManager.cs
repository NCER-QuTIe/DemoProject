using Contracts.Repositories;
using Redis.OM.Contracts;
using Repository;

namespace Repository;

public class RepositoryManager(IRedisConnectionProvider provider) : IRepositoryManager
{
    private readonly IRedisConnectionProvider _provider = provider;

    private readonly Lazy<IQTITestRepository> _qtiTestRepository = new Lazy<IQTITestRepository>(() => new QTITestRepository(provider));

    public IQTITestRepository QTITest => _qtiTestRepository.Value;

    public Task SaveAsync()
    {
        return Task.CompletedTask;
    }
}
