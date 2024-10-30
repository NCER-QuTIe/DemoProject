using Contracts.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Redis.OM.Contracts;
using Redis.OM.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public abstract class RepositoryBase<T>(IRedisConnectionProvider provider) : IRepositoryBase<T> where T : notnull
{
    private IRedisConnection _connection = provider.Connection;
    private IRedisCollection<T> NewCollection => provider.RedisCollection<T>();

    public async virtual Task CreateAsync(T entity)
    {
        var collection = NewCollection;
        await collection.InsertAsync(entity);
        await collection.SaveAsync();
    }

    public async virtual Task DeleteAsync(T entity)
    {
        var collection = NewCollection;
        await collection.DeleteAsync(entity);
        await collection.SaveAsync();
    }

    public async virtual Task UpdateAsync(T entity)
    {
        var collection = NewCollection;
        await collection.UpdateAsync(entity);
        await collection.SaveAsync();
    }

    public virtual Task<IQueryable<T>> FindAllAsync()
    {
        var collection = NewCollection;
        return Task.FromResult(collection.AsQueryable());
    }

    public virtual Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> condition)
    {
        var collection = NewCollection;
        return Task.FromResult(collection.AsQueryable().Where(condition));
    }
}
