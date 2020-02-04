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
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly ITwitterDbContext Context;

        protected IQueryable<T> Query =>
            Context.Set<T>()
            .AsNoTracking();

        protected BaseRepository(ITwitterDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> GetById(object id, CancellationToken token) =>
            await Context.Set<T>()
                .FindAsync(new[] { id }, token);

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expr, CancellationToken token) =>
            await Query
                .Where(expr)
                .ToListAsync(token);

        public Task<Result> Create(T entity, CancellationToken token) =>
            HandleAction(async () =>
            {
                await Context.Set<T>().AddAsync(entity, token);
                await Context.SaveChangesAsync(token);
            });

        public Task<Result> CreateMany(T[] entities, CancellationToken token) =>
            HandleAction(async () =>
            {
                await Context.Set<T>().AddRangeAsync(entities, token);
                await Context.SaveChangesAsync(token);
            });

        public Task<Result> Delete(object id, CancellationToken token) =>
            HandleAction(async () =>
            {
                var entity = await Context.Set<T>().FindAsync(new[] { id }, token);
                Context.Set<T>().Remove(entity);
                await Context.SaveChangesAsync(token);
            });

        public Task<Result> Delete(T entity, CancellationToken token) =>
            HandleAction(async () =>
            {
                Context.Set<T>().Remove(entity);
                await Context.SaveChangesAsync(token);
            });

        public Task<Result> Update(T entity, CancellationToken token) =>
            HandleAction(async () =>
            {
                Context.Set<T>().Update(entity);
                await Context.SaveChangesAsync(token);
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
