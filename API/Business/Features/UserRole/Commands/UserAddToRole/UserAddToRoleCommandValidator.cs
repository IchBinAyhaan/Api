using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.UserRole.Commands.UserAddToRole
{
    public class UserAddToRoleCommandValidator : AbstractValidator<UserAddToRoleCommand>
    {
        public UserAddToRoleCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Istifadeci daxil edilmedilir");

            RuleFor(x => x.RoleId)
                .NotEmpty()
                .WithMessage("Role daxil edilmelidir");
        }
    }
}
