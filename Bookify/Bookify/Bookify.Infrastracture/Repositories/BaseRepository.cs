using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastracture.Repositories
{
    internal abstract class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T>
        where T : Entity
    {
        protected readonly ApplicationDbContext context = context;

        public async Task AddAsync(T entity)
        =>await context.AddAsync(entity);

       

        public  IQueryable<T> FindAll(bool trackChanges)
        => trackChanges? context.Set<T>() : context.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        => trackChanges? context.Set<T>().Where(expression) : context.Set<T>().Where(expression).AsNoTracking();

        public void Remove(T entity)
        => entity.Delete();

        public void Update(T entity)
        => context.Update(entity);
    }
}
