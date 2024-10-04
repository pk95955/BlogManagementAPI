using AutoMapper;
using BlogManagementAPI.DTOs.Request;
using BlogManagementAPI.DTOs.Response;
using BlogManagementAPI.Models;

namespace BlogManagementAPI.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Domain Model to DTO
            CreateMap<BlogPost, BlogPostDTO>();
            //DTO to Model
            CreateMap<BlogPostRequestDTO, BlogPost>();
        }
    }
}
