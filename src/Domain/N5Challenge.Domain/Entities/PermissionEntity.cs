using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Domain.Entities
{
    public class PermissionEntity
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string EmployeeLastName { get; set; } = null!;
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }

        public PermissionTypeEntity PermissionType { get; set; } = null!;
    }
}
