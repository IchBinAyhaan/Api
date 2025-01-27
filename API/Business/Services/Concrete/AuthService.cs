using AutoMapper;
using Business.Dtos.Auth;
using Business.Services.Abstract;
using Business.Validators.Auth;
using Business.Wrappers;
using Common.Constants;
using Common.Entities;
using Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<User> userManager,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

      

        public async Task<Response> RegisterAsync(AuthRegisterDto model)
        {
            var result = await new AuthRegisterDtoValidator().ValidateAsync(model);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is not null)
                throw new ValidationException("Istifadeci artiq movcuddur");

            user = _mapper.Map<User>(model);

            var registerResult = await _userManager.CreateAsync(user, model.Password);
            if (!registerResult.Succeeded)
                throw new ValidationException(registerResult.Errors.Select(x => x.Description));

            var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
            if (!addToRoleResult.Succeeded)
                throw new ValidationException(addToRoleResult.Errors.Select(x => x.Description));

            return new Response()
            {
                Message = "Istifadeci ugurla elave olundu"
            };
        }

        public async Task<Response<AuthLoginResponseDto>> LoginAsync(AuthLoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                throw new UnauthorizedException("Email ve ya sifre yanlisdir");

            var isSuccededCheck = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isSuccededCheck)
                throw new UnauthorizedException("Email ve ya sifre yanlisdir");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));


            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _configuration.GetSection("JWT:Issuer").Value,
                audience: _configuration.GetSection("JWT:Audience").Value,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                );

            return new Response<AuthLoginResponseDto>
            {
                Data = new AuthLoginResponseDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                }
            };
        }
    }
}
