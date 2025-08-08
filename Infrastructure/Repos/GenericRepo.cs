using Core.Interfaces;
using Core.Models;
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

        public GenericRepo(StoreFrontDbContext context)
        {
            dbContext = context;
            dbSet = dbContext.Set<T>();
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

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbContext.Set<T>().AnyAsync(predicate);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await dbSet.Where(e => e.IsDeleted == false)
                .AsNoTracking().ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GetAllForAdminAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            if (id is null)
                throw new NotFoundException($"لا يوجد معرف بهذا الرقم, {id}");

            var entity = await dbSet.FindAsync(id)
                ?? throw new InValidObjectException("لا يوجد كائن لهذا البحث");

            if (entity.IsDeleted)
                throw new IsDeletedException($"تم حذف هذا الكائن, {entity}");

            return entity;
        }
        public async Task<T?> GetByIdForAdminAsync(object id)
        {
            if (id is null)
                throw new NotFoundException($"لا يوجد معرف بهذا الرقم, {id}");

            var entity = await dbSet.FindAsync(id)
                ?? throw new InValidObjectException("لا يوجد كائن لهذا البحث");

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

        public async Task<Product?> GetProductWithVariantsAsync(Guid id)
        {
            return await dbContext.Products
                .Include(p => p.ProductVariants)
                    //.ThenInclude(v => v.ProductVariantImages)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
