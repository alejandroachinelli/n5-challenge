using MediatR;
using N5Challenge.Transversal.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Application.UseCases.Permission.Commands.RequestPermission;

public record RequestPermissionCommand(
    string EmployeeName,
    string EmployeeLastName,
    int PermissionTypeId,
    DateTime PermissionDate
) : IRequest<Response<int>>;