using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories;

public interface IRepositoryBase<T>
{
    Task<IQueryable<T>> FindAllAsync();
    Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> condition);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
