using Business.Dtos.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Auth
{
    public class AuthRegisterDtoValidator : AbstractValidator<AuthRegisterDto>
    {
        public AuthRegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Poct Unvani daxil edilmelidir")
                .EmailAddress().WithMessage("Poct unvani duzgun daxil edilmelidir");

            RuleFor(x => x.Password.Length)
                .GreaterThanOrEqualTo(8)
                .WithMessage("Sifrenin uzunlugu minimum 8 simvol olmalidir");

            RuleFor(x => x.Password)
                .Equal(x => x.Confirmpassword)
                .WithMessage("Sifre ve tesdiq sifresi uygunlasmir");
        }
    }
}
