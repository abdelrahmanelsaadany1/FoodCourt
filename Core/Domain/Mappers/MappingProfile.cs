using AutoMapper;
using Domain.Dtos.CategoryDto;
using Domain.Entities;
using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
        }
    }
}
