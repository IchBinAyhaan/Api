using Business.Features.Product.Commands.DeleteProduct;
using Common.Exceptions;
using Data.Repositories.Product;
using Data.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Handlers.Product.Command
{
    public class DeleteProductHandlerTests
    {
        private readonly Mock<IProductReadRepository> _mockProductReadRepository;
        private readonly Mock<IProductWriteRepository> _mockProductWriteRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly DeleteProductHandler _handler;

        public DeleteProductHandlerTests()
        {
            _mockProductReadRepository = new Mock<IProductReadRepository>();
            _mockProductWriteRepository = new Mock<IProductWriteRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _handler = new DeleteProductHandler(
                _mockProductReadRepository.Object,
                _mockProductWriteRepository.Object,
                _mockUnitOfWork.Object
            );
        }

        [Fact]
        public async Task Handle_WhenProductNotFound_ShouldThrowNotFoundException()
        {
            var command = new DeleteProductCommand { Id = 1 };
            _mockProductReadRepository.Setup(x => x.GetAsync(command.Id)).ReturnsAsync((Common.Entities.Product)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Mehsul tapilmadi", exception.Message);
        }

        [Fact]
        public async Task Handle_WhenProductExists_ShouldDeleteProduct()
        {
            var command = new DeleteProductCommand { Id = 1 };
            var product = new Common.Entities.Product { Id = 1, Name = "Test Product" };

            _mockProductReadRepository.Setup(x => x.GetAsync(command.Id)).ReturnsAsync(product);
            _mockProductWriteRepository.Setup(x => x.Delete(product));

            var result = await _handler.Handle(command, CancellationToken.None);

            _mockProductWriteRepository.Verify(x => x.Delete(product), Times.Once);
            _mockUnitOfWork.Verify(x => x.CommitAsync(), Times.Once);
            Assert.Equal("Test Product ugurla silindi", result.Message);
        }
    }
}
