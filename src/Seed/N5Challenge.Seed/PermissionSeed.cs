using N5Challenge.Domain.Entities;
using System.Collections.Generic;

namespace N5Challenge.Seed;

public class PermissionSeed
{
    public static List<PermissionEntity> InitialData()
    {
        return new List<PermissionEntity>
        {
            new PermissionEntity() { EmployeeName = "Juan", EmployeeLastName = "Pérez", PermissionTypeId = 1, PermissionDate = new DateTime(2024, 10, 1) },
            new PermissionEntity() { EmployeeName = "Ana", EmployeeLastName = "López", PermissionTypeId = 2, PermissionDate = new DateTime(2024, 12, 15) },
            new PermissionEntity() { EmployeeName = "Mario", EmployeeLastName = "García", PermissionTypeId = 3, PermissionDate = new DateTime(2025, 1, 10) },
            new PermissionEntity() { EmployeeName = "Laura", EmployeeLastName = "Torres", PermissionTypeId = 4, PermissionDate = new DateTime(2025, 3, 22) },
            new PermissionEntity() { EmployeeName = "Ricardo", EmployeeLastName = "Díaz", PermissionTypeId = 1, PermissionDate = new DateTime(2025, 5, 5) },
            new PermissionEntity() { EmployeeName = "Carla", EmployeeLastName = "Méndez", PermissionTypeId = 5, PermissionDate = new DateTime(2025, 4, 1) },
        };
    }
}
