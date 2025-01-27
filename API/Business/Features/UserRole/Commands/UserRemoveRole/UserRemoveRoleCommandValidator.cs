using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.UserRole.Commands.UserRemoveRole
{
    public class UserRemoveRoleCommandValidator : AbstractValidator<UserRemoveRoleCommand>
    {
        public UserRemoveRoleCommandValidator()
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
