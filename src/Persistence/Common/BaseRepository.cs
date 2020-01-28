using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Common
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly ITwitterDbContext _context;

        public BaseRepository(ITwitterDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> GetById(object id, CancellationToken token) =>
            await _context.Set<T>().FindAsync(new[] { id }, token);

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expr, CancellationToken token) =>
            await _context.Set<T>().AsNoTracking().Where(expr).ToListAsync(token);

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

        protected async Task<Result> HandleAction(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (SqlException ex)
            {
                return Result.Failure(ex.Errors.Cast<string>());
            }
            catch (DbException ex)
            {
                return Result.Failure(new[] { ex.Message });
            }
            return Result.Success();
        }
    }
}
