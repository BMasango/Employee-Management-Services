using EMS.Core.Domain.Entities;
using EMS.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EMS.Infrastructure.Data.Repositories
{
    public class GenericRepositoryAsync<T, TContext> : IGenericRepositoryAsync<T>
        where T : BaseEntity
        where TContext : DbContext
    {

        private readonly TContext _context;
        private readonly DbSet<T> _table;

        public GenericRepositoryAsync(TContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public virtual async Task<Guid> AddAsync(T item)
        {

            var entity = await _table.AddAsync(item);
            await _context.SaveChangesAsync();
            return entity.Entity.Id;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var entity = await _table.FindAsync(id);
            if (entity == null)
                return false;

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _table
                .Where(o => !o.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return (await Task.Run(() => _table.Where(o => !o.IsDeleted).AsNoTracking().Where(expression))).ToList();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
