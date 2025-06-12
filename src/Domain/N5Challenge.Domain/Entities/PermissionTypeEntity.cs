using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Domain.Entities
{
    public class PermissionTypeEntity
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public ICollection<PermissionEntity> Permissions { get; set; } = new List<PermissionEntity>();
    }
}
