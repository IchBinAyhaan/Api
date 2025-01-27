using Business.Wrappers;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.UserRole.Commands.UserAddToRole
{
    public class UserAddToRoleHandler : IRequestHandler<UserAddToRoleCommand, Response>
    {
        private readonly UserManager<Common.Entities.User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserAddToRoleHandler(UserManager<Common.Entities.User> userManager,
                                    RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Response> Handle(UserAddToRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await new UserAddToRoleCommand().ValidateAsync(request);
            if (result != null)
                throw new ValidationException(result.Errors);

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
                throw new NotFoundException("Istifadeci tapilmadi");

            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role is null)
                throw new NotFoundException("Rol Tapilmadi");

            var isAlreadyExist = await _userManager.IsInRoleAsync(user, role.Name);
            if (isAlreadyExist)
                throw new ValidationException("Bu rol istifadecide movcuddur");

            var addToRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
            if (!addToRoleResult.Succeeded)
                throw new ValidationException(addToRoleResult.Errors.Select(x => x.Description));

            return new Response()
            {
                Message = "Istifadeciye rol elave olundu"
            };
        }
    }
}
