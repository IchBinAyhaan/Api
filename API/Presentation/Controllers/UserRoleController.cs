
using Business.Features.UserRole.Commands.UserAddToRole;
using Business.Features.UserRole.Commands.UserRemoveRole;

using Business.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin",AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserRoleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserRoleController(IMediator mediator)
        {
             _mediator = mediator;
        }
        #region Documentation
        /// <summary>
        /// Istifadeciye rol elave olunmasi
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost]
        public async Task<Response> AddRoleToUserAsync(UserAddToRoleCommand request)
        => await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// Istifadeciden rolun silinmesi
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpDelete]
        public async Task<Response> RemoveRoleFromUserAsync(UserRemoveRoleCommand request)
        => await _mediator.Send(request);
    }
}
