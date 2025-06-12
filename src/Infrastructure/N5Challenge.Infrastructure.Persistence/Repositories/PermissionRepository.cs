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
    public class PermissionRepository : IRepository<PermissionEntity>
    {
        private readonly ApplicationDbContext _context;
        public PermissionRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<PermissionEntity>> GetAllAsync() =>
            await _context.Permissions.Include(p => p.PermissionType).ToListAsync();

        public async Task<PermissionEntity?> GetByIdAsync(int id) =>
            await _context.Permissions.Include(p => p.PermissionType).FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(PermissionEntity entity) => await _context.Permissions.AddAsync(entity);

        public void Update(PermissionEntity entity) => _context.Permissions.Update(entity);

        public void Remove(PermissionEntity entity) => _context.Permissions.Remove(entity);
    }
}
