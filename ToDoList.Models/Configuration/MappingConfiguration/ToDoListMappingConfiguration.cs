using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Dtos.Requests;
using ToDoList.Models.Dtos.Responses;
using ToDoList.Models.Entities;

namespace ToDoList.Models.Configuration.MappingConfiguration
{
    

    public class ToDoListMappingConfiguration : Profile
    {
        public ToDoListMappingConfiguration()
        {
            CreateMap<CreateToDoItemRequest, ToDoItem>();
            CreateMap<ToDoItem, CreateToDoItemRequest>();
            CreateMap<ToDoItem, ToDoItemResponse>();
        }
    }
}
