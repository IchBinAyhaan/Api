using Business.Wrappers;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.UserRole.Commands.UserRemoveRole
{
    public class UserRemoveRoleHandler : IRequestHandler<UserRemoveRoleCommand, Response>
    {
        private readonly UserManager<Common.Entities.User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRemoveRoleHandler(UserManager<Common.Entities.User> userManager,
                                         RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Response> Handle(UserRemoveRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await new UserRemoveRoleCommand().ValidateAsync(request);
            if (result != null)
                throw new ValidationException(result.Errors);

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
                throw new NotFoundException("Istifadeci tapilmadi");

            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role is null)
                throw new NotFoundException("Rol tapilmadi");

            var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
            if (!isInRole)
                throw new ValidationException("Istifadeci bu rolda deyil");

            var removeResult = await _userManager.RemoveFromRoleAsync(user, role.Name);
            if (!removeResult.Succeeded)
                throw new ValidationException(removeResult.Errors.Select(x => x.Description));

            return new Response()
            {
                Message = "Rol istifadeciden ugurla silindi"
            };
        }
    }
}

