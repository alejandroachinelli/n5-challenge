using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Application.DTO.DTOs.Elasticsearch;

public class PermissionElasticDto
{
    public int Id { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeLastName { get; set; } = string.Empty;
    public int PermissionTypeId { get; set; }
    public string PermissionTypeDescription { get; set; } = string.Empty;
    public DateTime PermissionDate { get; set; }
}