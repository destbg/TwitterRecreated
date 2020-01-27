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
        Task<IEnumerable<T>> GetAll(CancellationToken token);
        Task<T> GetById(object id, CancellationToken token);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expr, CancellationToken token);
        Task<Result> Create(T entity, CancellationToken token);
        Task<Result> Delete(object id, CancellationToken token);
        Task<Result> Delete(T entity, CancellationToken token);
        Task<Result> Update(T entity, CancellationToken token);
    }
}
