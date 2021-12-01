using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AutoMapper;
using HealthApi.Entities;
using HealthApi.Exceptions;
using HealthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthApi.Services
{
    public interface IBookContentService
    {
        Guid Create(BookContent bookContentDto);
        BookContent Update(BookContent bookContentDto);
        BookContent GetById(Guid bookID);
        PageResult<BookContentDto> GetAll(int page, int pageSize);
        void RemoveById(Guid bookID);
        int IsExists(BookContent bookContentDto);

        List<SearchDto> GetByString(string searchText);
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
        public PageResult<BookContentDto> GetAll(int page, int pageSize)
        {
            IQueryable<BookContentDto> result = (from e in _context.BookContents
                    join d in _context.Books on e.BookId equals d.Id
                    select new BookContentDto
                    {
                        Id = e.Id,
                        BookName = d.Name,
                        BookId = d.Id,
                        Content = e.Content.Substring(0, 200),
                        pageNumber = e.PageNumber
                    }).OrderBy(x => x.BookName).ThenBy(x => x.pageNumber);

            return result.GetPaged(page, pageSize);
        }

        public List<SearchDto> GetByString(string searchText)
        {
            var bookContentList = (from bookContent in _context.BookContents.AsEnumerable()
                                   where bookContent.Content.Contains(searchText)
                      group bookContent by bookContent.BookId into g
                                   join book in _context.Books on g.Key equals book.Id
                                   select new { book.Name, g.Key, Items = string.Join(",", g.Select(sel => sel.PageNumber.ToString())), g }).ToList();

            var final = bookContentList.GroupBy(p => p.Key).Select(s => s.First());
            List<SearchDto> TstList = new List<SearchDto>();
            SearchDto searchDto = new SearchDto();
            foreach (var item in final)
            {
                searchDto = new SearchDto();
                searchDto.BookId = item.Key;
                searchDto.BookName = item.Name;
                searchDto.PagesNumber = item.Items;
                TstList.Add(searchDto);
            }

            var result = (from bookContent in TstList
                          join bookAuthor in _context.AuthorBooks on bookContent.BookId equals bookAuthor.BookId
                          join author in _context.Authors on bookAuthor.AuthorId equals author.Id
                          select new SearchDto
                          {
                              PagesNumber = bookContent.PagesNumber,
                              BookId = bookContent.BookId,
                              BookName = bookContent.BookName,
                              AuthorName = author.Name,
                              AuthorSurname = author.Surname
                          }).ToList();

            if (result is null)
            {
                throw new NotFoundException("Book not found");
            }

            return result;
        }

        public int IsExists(BookContent bookContentDto)
        {
            return _context.BookContents.Count(x => x.BookId == bookContentDto.BookId && x.PageNumber == bookContentDto.PageNumber);
        }

        public Guid Create(BookContent bookContentDto)
        {
            _context.BookContents.Add(bookContentDto);
            _context.SaveChanges();

            return bookContentDto.BookId;
        } 


        public BookContent GetById(Guid Id)
        {
           return _context.BookContents.FirstOrDefault(x => x.Id == Id);
        }

        public void RemoveById(Guid Id)
        {
            BookContent bookContent = _context.BookContents.FirstOrDefault(d => d.Id == Id);
            _context.RemoveRange(bookContent);
            _context.SaveChanges();
        } 
        public BookContent Update(BookContent bookContentDto)
        {
            try
            {
                var bookContentEntity = GetById(bookContentDto.Id);
                if (bookContentEntity == null)
                {
                    throw new NotFoundException("BookContent not found");
                }

                _context.Remove(bookContentEntity);

                _context.BookContents.Add(bookContentDto);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            return bookContentDto;
        }
    }
}
