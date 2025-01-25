using Business.Dtos.Role;
using Business.Services.Abstract;
using Business.Wrappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #region Documentation
        /// <summary>
        /// Rollarin siyahisi
        /// </summary>
        [ProducesResponseType(typeof(Response<List<RoleDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpGet]
        public async Task<Response<List<RoleDto>>> GetAllRolesAsync()
        => await _roleService.GetAllRolesAsync();
    }
}
