using Business.Features.Product.Commands.CreateProduct;
using Business.Features.Product.Commands.DeleteProduct;
using Business.Features.Product.Commands.UpdateProduct;
using Business.Features.Product.Dtos;
using Business.Features.Product.Queries.GetAllProducts;
using Business.Features.Product.Queries.GetProduct;

using Business.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #region Documentation
        /// <summary>
        /// Mehsullarin siyahisi
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpGet]
        public async Task<Response<List<ProductDto>>> GetAllProductAsync()
            => await _mediator.Send(new GetAllProductsQuery());
        #region Documentation
        /// <summary>
        /// Mehsulu id-sine gore goturmek ucun
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(Response<ProductDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpGet("{id}")]
        public async Task<Response<ProductDto>> GetProductAsync(int id)
            => await _mediator.Send(new GetProductQuery { Id = id });
        #region Documentation
        /// <summary>
        /// Mehsulu yaratmaq ucun
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li><b>Type:</b><p>0-New,1-Sold</p></li>
        /// </ul>
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost]
        public async Task<Response> CreateProductAsync(CreateProductCommand request)
        => await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// Mehsulun yenilenmesi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPut("{id}")]
        public async Task<Response> UpdateProductAsync(int id, UpdateProductCommand request)
        {
            request.Id = id;
            return await _mediator.Send(request);
        }
        #region 
        /// <summary>
        /// Mehsulun silinmesi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpDelete]
        public async Task<Response> DeleteProductAsync(int id)
            => await _mediator.Send(new DeleteProductCommand { Id = id });
    }
}
