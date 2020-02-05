using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Repositories
{
    public interface IRepository<T>
    {
        ValueTask<T> GetById(object id, CancellationToken token);
        Task<List<T>> Find(Expression<Func<T, bool>> expr, CancellationToken token);
        Task<Result> Create(T entity, CancellationToken token);
        Task<Result> CreateMany(IEnumerable<T> entities, CancellationToken token);
        Task<Result> Delete(object id, CancellationToken token);
        Task<Result> Delete(T entity, CancellationToken token);
        Task<Result> DeleteMany(IEnumerable<T> entities, CancellationToken token);
        Task<Result> DeleteMany(Expression<Func<T, bool>> expr, CancellationToken token);
        Task<Result> Update(T entity, CancellationToken token);
        Task<Result> UpdateMany(IEnumerable<T> entities, CancellationToken token);
    }
}
