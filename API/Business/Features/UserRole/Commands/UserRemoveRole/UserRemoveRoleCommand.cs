using Business.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.UserRole.Commands.UserRemoveRole
{
    public class UserRemoveRoleCommand : IRequest<Response>
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

    }
}
