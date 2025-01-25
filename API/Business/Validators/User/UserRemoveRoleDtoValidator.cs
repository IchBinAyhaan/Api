using Business.Dtos.User;
using Business.Dtos.UserRole;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.User
{
    public class UserRemoveRoleDtoValidatior : AbstractValidator<UserRemoveRoleDto>
    {
        public UserRemoveRoleDtoValidatior()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Istifadeci daxil edilmelidir");

            RuleFor(x => x.RoleId)
                .NotEmpty()
                .WithMessage("Rol daxil edilmelidir");
        }
    }
}
