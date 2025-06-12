using N5Challenge.Application.Interfaces.Persistence;
using N5Challenge.Domain.Entities;
using N5Challenge.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IRepository<PermissionEntity> PermissionRepository { get; }
    public IRepository<PermissionTypeEntity> PermissionTypeRepository { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        PermissionRepository = new PermissionRepository(_context);
        PermissionTypeRepository = new PermissionTypeRepository(_context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
