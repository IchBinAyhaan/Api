using AutoMapper;
using Business.Features.Role.Dtos;
using Business.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Role.Queries.GetAllRoles
{
    public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, Response<List<RoleDto>>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public GetAllRolesHandler(RoleManager<IdentityRole> roleManager,
                                  IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<Response<List<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            return new Response<List<RoleDto>>()
            {
                Data = _mapper.Map<List<RoleDto>>(await _roleManager.Roles.ToListAsync())
            };
        }
    }
}
