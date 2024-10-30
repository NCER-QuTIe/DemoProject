//TODO



//using Redis.OM.Contracts;
//using StackExchange.Redis;
//using Microsoft.Extensions.Caching.Distributed;

//namespace Repository;

//public class CachableRepositoryBase<T>(IRedisConnectionProvider provider, IDistributedCache cache) : RepositoryBase<T>(provider) where T : notnull
//{
//    private readonly IDistributedCache _cache = cache;
//    private readonly string _indexName = typeof(T).Name;

//    public override async Task Create(T entity)
//    {
//        await base.Create(entity);
//        await _cache.SetAsync(_indexName, null);
//    }

//    public override async Task Delete(T entity)
//    {
//        await base.Delete(entity);
//        await _cache.RemoveAsync(typeof(T).Name);
//    }

//    public async Task Update(T entity)
//    {
//        await base.Update(entity);
//        await _cache.RemoveAsync(typeof(T).Name);
//    }

//    public IQueryable<T> FindAll()
//    {
//        var cacheKey = typeof(T).Name;
//        var cachedData = await _cache.GetAsync<T[]>(cacheKey);
//        if (cachedData != null)
//        {
//            return cachedData.AsQueryable();
//        }

//        var data = base.FindAll();
//        await _cache.SetAsync(cacheKey, data.ToArray());
//        return data;
//    }

//    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition)
//    {
//        var cacheKey = typeof(T).Name;
//        var cachedData = await _cache.GetAsync<T[]>(cacheKey);
//        if (cachedData != null)
//        {
//            return cachedData.AsQueryable().Where(condition);
//        }

//        var data = base.FindByCondition(condition);
//        await _cache.SetAsync(cacheKey, data.ToArray());
//        return data;
//    }


//    private Guid GetEntityGuid(T entity)
//    {

//    }
//}
