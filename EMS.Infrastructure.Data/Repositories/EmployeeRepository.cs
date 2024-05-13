using EMS.Core.Domain.Entities;
using EMS.Core.Domain.Repositories;
using EMS.Infrastructure.Data.Context;
using EMS.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EMS.Infrastructure.Repositories
{
    public class EmployeeRepository : GenericRepositoryAsync<EmployeeEntity, ApplicationContext>, IEmployeeRepository
    {
        private readonly DbSet<EmployeeEntity> _table;

        public EmployeeRepository(ApplicationContext context) : base(context)
        {
            _table = context.Set<EmployeeEntity>();
        }

        public override async Task<IEnumerable<EmployeeEntity>> GetAllAsync()
        {
            return await _table
                .Include(o => o.Address)
                .Where(m => !m.IsDeleted)
                .AsNoTracking().ToListAsync();
        }

        public virtual async Task<EmployeeEntity> GetByIdAsync(Guid id)
        {
            return _table.Include(o => o.Address)
                .FirstOrDefault(x => !x.IsDeleted && x.Id == id);
        }

        public override async Task<IEnumerable<EmployeeEntity>> GetAllAsync(Expression<Func<EmployeeEntity, bool>> expression)
        {
            return (await Task.Run(() =>
                    _table
                    .Include(o => o.Address)
                    .Where(m => !m.IsDeleted)
                    .AsNoTracking().Where(expression))).ToList();
        }
    }
}
