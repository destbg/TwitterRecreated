using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Common
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly ITwitterDbContext _context;

        public GenericRepository(ITwitterDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<T> GetAll() =>
            _context.Set<T>().AsNoTracking();

        public DbSet<T> GetSet() =>
            _context.Set<T>();

        public Task<Result> Create(T entity, CancellationToken token) =>
            HandleAction(async () =>
            {
                await _context.Set<T>().AddAsync(entity, token);
                await _context.SaveChangesAsync(token);
            });

        public Task<Result> Delete(object id, CancellationToken token) =>
            HandleAction(async () =>
            {
                var entity = await _context.Set<T>().FindAsync(new[] { id }, token);
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync(token);
            });

        public Task<Result> Delete(T entity, CancellationToken token) =>
            HandleAction(async () =>
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync(token);
            });

        public Task<Result> Update(T entity, CancellationToken token) =>
            HandleAction(async () =>
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync(token);
            });

        private async Task<Result> HandleAction(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (SqlException ex)
            {
                return Result.Failure(ex.Errors.Cast<string>());
            }
            return Result.Success();
        }
    }
}
