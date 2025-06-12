using FluentValidation;
using N5Challenge.Application.UseCases.Permission.Commands.RequestPermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Application.UseCases.Permission.Validations;

public class RequestPermissionValidator : AbstractValidator<RequestPermissionCommand>
{
    public RequestPermissionValidator()
    {
        RuleFor(x => x.EmployeeName).NotEmpty();
        RuleFor(x => x.EmployeeLastName).NotEmpty();
        RuleFor(x => x.PermissionTypeId).GreaterThan(0);
        RuleFor(x => x.PermissionDate).LessThanOrEqualTo(DateTime.Now);
    }
}