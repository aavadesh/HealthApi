using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HealthApi.Entities;
using HealthApi.Exceptions;
using HealthApi.Extensions;
using HealthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthApi.Services
{
    public interface ISearchService
    {
        List<SearchDto> GetByString(string searchText);
    }

    public class SearchService : ISearchService
    {
        private readonly HealthDbContext _context;
        private readonly IMapper _mapper;

        public SearchService(HealthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<SearchDto> GetByString(string searchText)
        {
            List<SearchDto> result = (List<SearchDto>)(from book in _context.Books where book.Name.Contains(searchText)
                                 join authorBook in _context.AuthorBooks on book.Id equals authorBook.BookId
                                 join author in _context.Authors on authorBook.AuthorId equals author.Id where author.Name.Contains(searchText) || author.Surname.Contains(searchText)
                                 join bookContent in _context.BookContents on book.Id equals bookContent.BookId
                                 select new SearchDto
                                 {
                                     BookId = book.Id,
                                     BookName = book.Name,
                                     PagesNumber = bookContent.PageNumber,
                                     AuthorFullName = author.Name + " " + author.Surname
                                 }).OrderBy(o => o.PagesNumber).ToList().GroupBy(h => h.PagesNumber);

            if (result is null)
            {
                throw new NotFoundException("Book not found");
            }

            return result;
        }
    }
}
