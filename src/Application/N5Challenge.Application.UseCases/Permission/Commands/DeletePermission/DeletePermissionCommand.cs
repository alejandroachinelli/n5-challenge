using MediatR;
using N5Challenge.Transversal.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Application.UseCases.Permission.Commands.DeletePermission
{
    public class DeletePermissionCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
    }
}
