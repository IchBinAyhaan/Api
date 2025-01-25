using Business.Dtos.Role;
using Business.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstract
{
    public interface IRoleService
    {
        Task<Response<List<RoleDto>>> GetAllRolesAsync();
    }
}
