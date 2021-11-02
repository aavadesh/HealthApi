using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HealthApi.Entities;

namespace HealthApi.Services
{
    public interface IBookContentService
    {
        List<BookContent> GetAll();
    }

    public class BookContentService : IBookContentService
    {
        private readonly HealthDbContext _context;
        private readonly IMapper _mapper;

        public BookContentService(HealthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<BookContent> GetAll()
        {
            return _context.BookContents.ToList();
        }
    }
}
