using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.FilmsMapper
{
    public class FilmsMappers : Profile 
    {
        public FilmsMappers()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Film, FilmDto>().ReverseMap();
            CreateMap<Film, FilmCreateDto>().ReverseMap();
            CreateMap<Film, FilmUpdateDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
