using N5Challenge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    IRepository<PermissionEntity> PermissionRepository { get; }
    IRepository<PermissionTypeEntity> PermissionTypeRepository { get; }
    Task<int> SaveChangesAsync();
}
