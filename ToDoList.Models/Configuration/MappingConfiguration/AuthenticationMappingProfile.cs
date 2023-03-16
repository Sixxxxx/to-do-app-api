using AutoMapper;
using ToDoList.Models.Dtos.Request;
using ToDoList.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models.Configuration.MappingConfiguration
{
    public class AuthenticationMappingProfile : Profile
    {
        public AuthenticationMappingProfile()
        {
            CreateMap<CreateStaffRequest, UserProfile>();
        }
    }
}
