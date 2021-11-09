using HealthApi.Entities;
using HealthApi.Models;
using AutoMapper;

namespace HealthApi
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<BookDto, Book>();
            CreateMap<AuthorDto, Author>();
        }
    }
}
