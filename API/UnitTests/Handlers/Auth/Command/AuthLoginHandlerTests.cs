using AutoMapper;
using Business.Features.Auth.Commands.AuthLogin;
using Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Handlers.Auth.Command
{
    public class AuthLoginHandlerTests
    {
        private readonly Mock<UserManager<Common.Entities.User>> _mockUserManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthLoginHandler _handler;

        public AuthLoginHandlerTests()
        {
            _mockUserManager = new Mock<UserManager<Common.Entities.User>>(
                new Mock<IUserStore<Common.Entities.User>>().Object,
                null, null, null, null, null, null, null, null);

            _mockMapper = new Mock<IMapper>();
            _mockConfiguration = new Mock<IConfiguration>();

            _handler = new AuthLoginHandler(
                _mockUserManager.Object,
                _mockMapper.Object,
                _mockConfiguration.Object
            );
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ShouldThrowUnauthorizedException()
        {

            var command = new AuthLoginCommand { Email = "test@example.com", Password = "Password123" };
            _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync((Common.Entities.User)null);


            var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Email ve ya sifre yanlisdir", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenPasswordIsIncorrect_ShouldThrowUnauthorizedException()
        {

            var command = new AuthLoginCommand { Email = "test@example.com", Password = "WrongPassword" };
            var user = new Common.Entities.User { Id = "1", Email = "test@example.com" };

            _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, command.Password)).ReturnsAsync(false);


            var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Email ve ya sifre yanlisdir", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenLoginSuccessful_ShouldReturnToken()
        {

            var command = new AuthLoginCommand { Email = "test@example.com", Password = "Password123" };
            var user = new Common.Entities.User { Id = "1", Email = "test@example.com" };
            var roles = new List<string> { "Admin", "User" };

            _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, command.Password)).ReturnsAsync(true);
            _mockUserManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(roles);
            _mockConfiguration.Setup(x => x["JWT:SigningKey"]).Returns("MyStrongPassword123!MyStrongPassword123!");
            _mockConfiguration.Setup(x => x["JWT:Issuer"]).Returns("https://localhost:7060/");
            _mockConfiguration.Setup(x => x["JWT:Audience"]).Returns("https://localhost:7060/");


            var result = await _handler.Handle(command, CancellationToken.None);


            Assert.NotNull(result);
            Assert.NotNull(result.Data.Token);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(result.Data.Token) as JwtSecurityToken;
            Assert.NotNull(token);
            Assert.Equal(user.Id, token?.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Assert.Equal(user.Email, token?.Claims.First(c => c.Type == ClaimTypes.Email).Value);
            Assert.Contains(roles, role => token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == role);
        }
    }
}
