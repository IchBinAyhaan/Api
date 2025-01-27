using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Auth.Commands.AuthRegister
{
    public class AuthRegisterCommandValidator : AbstractValidator<AuthRegisterCommand>
    {
        public AuthRegisterCommandValidator()
        {
            
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Poct Unvani daxil edilmelidir")
                .EmailAddress().WithMessage("Poct unvani duzgun daxil edilmelidir");

            RuleFor(x => x.Password.Length)
                .GreaterThanOrEqualTo(8)
                .WithMessage("Sifrenin uzunlugu minimum 8 simvol olmalidir");

            RuleFor(x => x.Password)
                .Equal(x => x.ConfirmPassword)
                .WithMessage("Sifre ve tesdiq sifresi uygunlasmir");
        }
    }
}
