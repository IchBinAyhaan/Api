using Business.Features.UserRole.Commands.UserRemoveRole;
using Common.Exceptions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Handlers.Role.Command
{
    public class UserRemoveRoleTests
    {
        private readonly Mock<UserManager<Common.Entities.User>> _mockUserManager;
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private readonly UserRemoveRoleHandler _handler;

        public UserRemoveRoleTests()
        {
            _mockUserManager = new Mock<UserManager<Common.Entities.User>>(
                new Mock<IUserStore<Common.Entities.User>>().Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                null,
                null,
                null,
                null);

            _handler = new UserRemoveRoleHandler(_mockUserManager.Object, _mockRoleManager.Object);
        }

        [Fact]
        public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            
            var command = new UserRemoveRoleCommand { UserId = "1", RoleId = "2" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("UserId", "User ID is required") });

            _mockUserManager.Setup(x => x.FindByIdAsync(command.UserId)).ReturnsAsync((Common.Entities.User)null);
            _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId)).ReturnsAsync((IdentityRole)null);

            
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("User ID is required", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ShouldThrowNotFoundException()
        {
            
            var command = new UserRemoveRoleCommand { UserId = "1", RoleId = "2" };

            _mockUserManager.Setup(x => x.FindByIdAsync(command.UserId)).ReturnsAsync((Common.Entities.User)null);

            
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Istifadeci tapilmadi", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenRoleNotFound_ShouldThrowNotFoundException()
        {
            
            var command = new UserRemoveRoleCommand { UserId = "1", RoleId = "2" };
            var user = new Common.Entities.User { Id = "1", UserName = "TestUser" };

            _mockUserManager.Setup(x => x.FindByIdAsync(command.UserId)).ReturnsAsync(user);
            _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId)).ReturnsAsync((IdentityRole)null);

            
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Rol tapilmadi", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenUserNotInRole_ShouldThrowValidationException()
        {
          
            var command = new UserRemoveRoleCommand { UserId = "1", RoleId = "2" };
            var user = new Common.Entities.User { Id = "1", UserName = "TestUser" };
            var role = new IdentityRole { Id = "2", Name = "Admin" };

            _mockUserManager.Setup(x => x.FindByIdAsync(command.UserId)).ReturnsAsync(user);
            _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId)).ReturnsAsync(role);
            _mockUserManager.Setup(x => x.IsInRoleAsync(user, role.Name)).ReturnsAsync(false); 

           
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Istifadeci bu rolda deyil", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenRoleRemovedSuccessfully_ShouldReturnSuccessResponse()
        {
          
            var command = new UserRemoveRoleCommand { UserId = "1", RoleId = "2" };
            var user = new Common.Entities.User { Id = "1", UserName = "TestUser" };
            var role = new IdentityRole { Id = "2", Name = "Admin" };

            _mockUserManager.Setup(x => x.FindByIdAsync(command.UserId)).ReturnsAsync(user);
            _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId)).ReturnsAsync(role);
            _mockUserManager.Setup(x => x.IsInRoleAsync(user, role.Name)).ReturnsAsync(true); 
            _mockUserManager.Setup(x => x.RemoveFromRoleAsync(user, role.Name)).ReturnsAsync(IdentityResult.Success);

          
            var result = await _handler.Handle(command, CancellationToken.None);

           
            _mockUserManager.Verify(x => x.RemoveFromRoleAsync(user, role.Name), Times.Once);
            Assert.Equal("Rol istifadeciden ugurla silindi", result.Message);
        }
    }
}
