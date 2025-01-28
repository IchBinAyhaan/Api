using AutoMapper;
using Business.Features.Product.Commands.UpdateProduct;
using Common.Exceptions;
using Data.Repositories.Product;
using Data.UnitOfWork;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Handlers.Product.Command
{
    public class UpdateProductHandlerTests
    {
        private readonly Mock<IProductReadRepository> _mockProductReadRepository;
        private readonly Mock<IProductWriteRepository> _mockProductWriteRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _mockProductReadRepository = new Mock<IProductReadRepository>();
            _mockProductWriteRepository = new Mock<IProductWriteRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            _handler = new UpdateProductHandler(
                _mockProductReadRepository.Object,
                _mockProductWriteRepository.Object,
                _mockUnitOfWork.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task Handle_WhenProductNotFound_ShouldThrowNotFoundException()
        {
           
            var command = new UpdateProductCommand { Id = 1 };

            _mockProductReadRepository.Setup(x => x.GetAsync(command.Id)).ReturnsAsync((Common.Entities.Product)null);

            
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Mehsul tapilmadi", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            
            var command = new UpdateProductCommand { Id = 1, Name = "Product" };

            var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Invalid name") });

            var validatorMock = new Mock<IValidator<UpdateProductCommand>>();
            validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            _mockProductReadRepository.Setup(x => x.GetAsync(command.Id)).ReturnsAsync(new Common.Entities.Product());
            _mockMapper.Setup(x => x.Map(It.IsAny<UpdateProductCommand>(), It.IsAny<Common.Entities.Product>()));

           
            var exception = await Assert.ThrowsAsync<Common.Exceptions.ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Invalid name", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenProductIsUpdated_ShouldReturnSuccessMessage()
        {
           
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "Updated Product",
                Price = 200,
                Quantity = 50,
                Description = "Updated description",
                Photo = "newpicture.jpg"
            };

            var product = new Common.Entities.Product { Id = 1, Name = "Old Product", Price = 100, Quantity = 20, Description = "Old description" };

            _mockProductReadRepository.Setup(x => x.GetAsync(command.Id)).ReturnsAsync(product);
            _mockMapper.Setup(x => x.Map(command, product));
            _mockProductWriteRepository.Setup(x => x.Update(product));
            _mockUnitOfWork.Setup(x => x.CommitAsync());

           
            var result = await _handler.Handle(command, CancellationToken.None);

            
            Assert.Equal("Mehsul ugurla yenilendi", result.Message);
            _mockProductWriteRepository.Verify(x => x.Update(It.IsAny<Common.Entities.Product>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.CommitAsync(), Times.Once);
        }
    }
}
