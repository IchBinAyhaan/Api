using AutoMapper;
using Business.Dtos.User;
using Business.Dtos.UserRole;
using Business.Services.Abstract;
using Business.Validators.User;
using Business.Wrappers;
using Common.Entities;
using Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = Common.Exceptions.ValidationException;

namespace Business.Services.Concrete
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleService(UserManager<User> userManager,
                           RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Response> AddRoleToUserAsync(UserAddToRoleDto model)
        {
            var result = await new UserAddToRoleDtoValidator().ValidateAsync(model);
            if (result != null)
                throw new ValidationException(result.Errors);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is null)
                throw new NotFoundException("Istifadeci tapilmadi");

            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role is null)
                throw new NotFoundException("Rol tapilmadi");

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

        public async Task<Response> RemoveRoleFromUserAsync(UserRemoveRoleDto model)
        {
            var result = await new UserRemoveRoleDtoValidatior().ValidateAsync(model);
            if (result != null)
                throw new ValidationException(result.Errors);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is null)
                throw new NotFoundException("Istifadeci tapilmadi");

            var role = await _roleManager.FindByIdAsync(model.RoleId);
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
