using Business.Dtos.Auth;
using Business.Services.Abstract;
using Business.Wrappers;
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
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
        public async Task<Response> RegisterAsync(AuthRegisterDto model)
        => await _authService.RegisterAsync(model);

        #region Documentation
        /// <summary>
        /// Istifadecinin daxil olmasi
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(typeof(Response<AuthLoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost("login")]
        public async Task<Response<AuthLoginResponseDto>> LoginAsync(AuthLoginDto model)
        => await _authService.LoginAsync(model);
    }
}
