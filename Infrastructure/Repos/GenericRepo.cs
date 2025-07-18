using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class, IDeletable
    {
        private readonly StoreFrontDbContext dbContext;
        private readonly DbSet<T> dbSet;

        public GenericRepo(StoreFrontDbContext context, DbSet<T> values)
        {
            dbContext = context;
            dbSet = values;
        }
        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteByIdAsync(object id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)  return false;

            dbSet.Remove(entity);
            return true;
        }
        

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            return await dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await dbSet.Where(e => e.IsDeleted == false)
                .AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            if (id == null)
            {
                throw new NotFoundException($"لا يوجد معرف بهذا الرقم, {id}");
            }
            var entity = await dbSet.FindAsync(id);
            if (entity is null)
            {
                throw new ArgumentNullException("لا يوجد كائن لهذا البحث");
            }
            if (entity.IsDeleted)
            {
                throw new IsDeletedException($"تم حذف هذا الكائن, {entity}");
            }
            return entity;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await dbContext.SaveChangesAsync() > 0;
        }

        public T Update(T entity)
        {
            dbSet.Update(entity);
            return entity;
        }
    }
}
