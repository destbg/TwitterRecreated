using Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<Result> Create(T entity, CancellationToken token);
        Task<Result> Delete(object id, CancellationToken token);
        Task<Result> Delete(T entity, CancellationToken token);
        IQueryable<T> GetAll();
        Task<Result> Update(T entity, CancellationToken token);
        DbSet<T> GetSet();
    }
}
