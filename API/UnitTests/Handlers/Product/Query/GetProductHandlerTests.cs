using AutoMapper;
using Business.Features.Product.Dtos;
using Business.Features.Product.Queries.GetProduct;
using Business.Wrappers;
using Common.Exceptions;
using Data.Repositories.Product;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Handlers.Product.Query
{   
    public class GetProductHandlerTests
    {
        private readonly Mock<IProductReadRepository> _productReadRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly GetProductHandler _handler;
        public GetProductHandlerTests()
        {
            _mapper = new Mock<IMapper>();
            _productReadRepository = new Mock<IProductReadRepository>();
            _handler = new GetProductHandler(_productReadRepository.Object, _mapper.Object);
        }
        [Fact]
        public async Task Handle_WhenProductNotFound_ShouldThrowNotFoundException()
        {
            var request = new GetProductQuery { Id = It.IsAny<int>() };

            _productReadRepository.Setup(x => x.GetAsync(request.Id))
                .ReturnsAsync(value: null);

            Func<Task> func = async () => await _handler.Handle(request, It.IsAny<CancellationToken>());

            var exception = await Assert.ThrowsAsync<NotFoundException>(func);
            Assert.Contains("Mehsul tapilmadi", exception.Errors);
        }
        [Fact]
        public async Task Handle_WhenLowIsSucceeded_ReturnsResponseModel()
        {
            var request = new GetProductQuery { Id = It.IsAny<int>() };
            var product = new Common.Entities.Product();
            var productDto = new ProductDto();
            _productReadRepository.Setup(x => x.GetAsync(request.Id))
               .ReturnsAsync(product);

            _mapper.Setup(x => x.Map<ProductDto>(product))
                .Returns(productDto);

            var response = await _handler.Handle(request,It.IsAny<CancellationToken>());

            Assert.IsType<Business.Wrappers.Response<ProductDto>>(response);
            Assert.Equal(productDto,response.Data);
        }
    }
}
