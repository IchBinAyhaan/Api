
using Business.Features.Auth.Commands.AuthLogin;
using Business.Features.Auth.Commands.AuthRegister;
using Business.Features.Auth.Dtos;

using Business.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Seller",AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Documentation
        /// <summary>
        /// Istifadecinin qeydiyyatdan kecmesi
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion 
        [HttpPost("register")]
        public async Task<Response> RegisterAsync(AuthRegisterCommand request)
        => await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// Istifadecinin daxil olmasi
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(typeof(Response<ResponseTokenDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost("login")]
        public async Task<Response<ResponseTokenDto>> LoginAsync(AuthLoginCommand request)
        => await _mediator.Send(request);
    }
}
