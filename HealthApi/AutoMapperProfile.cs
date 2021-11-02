using HealthApi.Entities;
using HealthApi.Models;
using AutoMapper;

namespace HealthApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BookViewModel, Book>();
            CreateMap<AuthorViewModel, Author>();
        }
    }
}
