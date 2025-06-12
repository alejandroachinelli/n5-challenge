using MediatR;
using N5Challenge.Application.DTO.DTOs;
using N5Challenge.Transversal.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Application.UseCases.PermissionType.Queries.GetAllPermissionsTypes
{
    public class GetAllPermissionsTypesQuery : IRequest<Response<List<PermissionTypeDto>>>;
}
