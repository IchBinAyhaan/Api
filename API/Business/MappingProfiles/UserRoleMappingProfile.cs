using AutoMapper;
using Business.Features.Auth.Commands.AuthRegister;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
    public class UserRoleMappingProfile : Profile
    {
        public UserRoleMappingProfile()
        {
            CreateMap<AuthRegisterCommand, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

        }
    }
}
