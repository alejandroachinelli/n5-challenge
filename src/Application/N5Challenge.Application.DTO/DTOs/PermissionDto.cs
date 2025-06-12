using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Application.DTO.DTOs;

public class PermissionDto
{
    public int Id { get; set; }
    public string EmployeeName { get; set; }
    public string EmployeeLastName { get; set; }
    public int PermissionTypeId { get; set; }
    public string PermissionTypeDescription { get; set; }
    public DateTime PermissionDate { get; set; }
}