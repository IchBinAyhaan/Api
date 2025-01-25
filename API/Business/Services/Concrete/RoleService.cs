using AutoMapper;
using Business.Dtos.Role;
using Business.Services.Abstract;
using Business.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concrete
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<IdentityRole> roleManager,
                           IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<Response<List<RoleDto>>> GetAllRolesAsync()
        {
            return new Response<List<RoleDto>>()
            {
                Data = _mapper.Map<List<RoleDto>>(await _roleManager.Roles.ToListAsync())
            };
        }
    }
}
