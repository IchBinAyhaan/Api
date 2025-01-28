using AutoMapper;
using Business.Features.Auth.Commands.AuthRegister;
using Common.Constants;
using Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Handlers.Auth.Command
{
    public class AuthRegisterHandlerTests
    {
        private readonly Mock<UserManager<Common.Entities.User>> _mockUserManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AuthRegisterHandler _handler;

        public AuthRegisterHandlerTests()
        {
            _mockUserManager = new Mock<UserManager<Common.Entities.User>>(
                new Mock<IUserStore<Common.Entities.User>>().Object,
                null, null, null, null, null, null, null, null);

            _mockMapper = new Mock<IMapper>();
            _handler = new AuthRegisterHandler(_mockUserManager.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
        {

            var command = new AuthRegisterCommand { Email = "test@example.com", Password = "Password123" };

            var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Email", "Invalid email") });

            var validatorMock = new Mock<AuthRegisterCommandValidator>();
            validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _handler = new AuthRegisterHandler(_mockUserManager.Object, _mockMapper.Object);


            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Invalid email", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenEmailAlreadyExists_ShouldThrowValidationException()
        {

            var command = new AuthRegisterCommand { Email = "test@example.com", Password = "Password123" };

            var existingUser = new Common.Entities.User { Email = "test@example.com" };
            _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync(existingUser);


            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Email already exists.", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenUserCreationFails_ShouldThrowValidationException()
        {

            var command = new AuthRegisterCommand { Email = "test@example.com", Password = "Password123" };

            var user = new Common.Entities.User { Email = command.Email };
            _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync((Common.Entities.User)null);
            _mockMapper.Setup(x => x.Map<Common.Entities.User>(command)).Returns(user);

            var createResult = IdentityResult.Failed(new IdentityError { Description = "Error creating user" });
            _mockUserManager.Setup(x => x.CreateAsync(user, command.Password)).ReturnsAsync(createResult);


            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Error creating user", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenUserCreatedSuccessfully_ShouldReturnSuccessMessage()
        {

            var command = new AuthRegisterCommand { Email = "test@example.com", Password = "Password123" };

            var user = new Common.Entities.User { Email = command.Email };
            _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync((Common.Entities.User)null);
            _mockMapper.Setup(x => x.Map<Common.Entities.User>(command)).Returns(user);

            var createResult = IdentityResult.Success;
            _mockUserManager.Setup(x => x.CreateAsync(user, command.Password)).ReturnsAsync(createResult);

            var addToRoleResult = IdentityResult.Success;
            _mockUserManager.Setup(x => x.AddToRoleAsync(user, UserRoles.User.ToString())).ReturnsAsync(addToRoleResult);


            var result = await _handler.Handle(command, CancellationToken.None);


            Assert.Equal("Istifadeci ugurla elave edildi", result.Message);
        }
    }
}
