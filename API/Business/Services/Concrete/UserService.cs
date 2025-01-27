using AutoMapper;
using Business.Dtos.User;
using Business.Services.Abstract;
using Business.Wrappers;
using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager,
                           IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Response<List<UserDto>>> GetAllUsersAsync()
        {
            return new Response<List<UserDto>>()
            {
                Data = _mapper.Map<List<UserDto>>(await _userManager.Users.ToListAsync())
            };
        }
    }
}
