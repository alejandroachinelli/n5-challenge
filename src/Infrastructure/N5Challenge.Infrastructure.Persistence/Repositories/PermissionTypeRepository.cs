using Microsoft.EntityFrameworkCore;
using N5Challenge.Application.Interfaces.Persistence;
using N5Challenge.Domain.Entities;
using N5Challenge.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Infrastructure.Persistence.Repositories
{
    public class PermissionTypeRepository : IRepository<PermissionTypeEntity>
    {
        private readonly ApplicationDbContext _context;
        public PermissionTypeRepository(ApplicationDbContext context) => _context = context;
        public Task AddAsync(PermissionTypeEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PermissionTypeEntity>> GetAllAsync() =>
            await _context.PermissionTypes.ToListAsync();

        public Task<PermissionTypeEntity?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(PermissionTypeEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(PermissionTypeEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
