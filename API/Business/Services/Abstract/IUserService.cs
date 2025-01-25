using Business.Dtos.User;
using Business.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstract
{
    public interface IUserService
    {
        Task<Response<List<UserDto>>> GetAllUsersAsync();
    }
}
