using Business.Dtos.User;
using Business.Dtos.UserRole;
using Business.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstract
{
    public interface IUserRoleService
    {
        Task<Response> AddRoleToUserAsync(UserAddToRoleDto model);
        Task<Response> RemoveRoleFromUserAsync(UserRemoveRoleDto model);
    }
}
