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
                    var res = from bookContent in _context.BookContents

                          /* Books */
                      join book in _context.Books on bookContent.BookId equals book.Id

                      /* Categories */
                      join pivotBookCategories in _context.BookCategories on book.Id equals pivotBookCategories.BookId
                      join category in _context.Categories on pivotBookCategories.CategoryId equals category.Id

                      /* Authors */
                      join pivotAuthorBooks in _context.AuthorBooks on book.Id equals pivotAuthorBooks.BookId
                      join author in _context.Authors on pivotAuthorBooks.AuthorId equals author.Id

                      where bookContent.Content.Contains(searchText)
                      select new
                      {
                          PagesNumber = bookContent.PageNumber,
                          BookId = book.Id,
                          BookName = book.Name,
                          AuthorName = author.Name,
                          AuthorSurname = author.Surname
                      };

            var result = res.ToList();
            var usersGroupedByCountry = result.GroupBy(user => user.BookId);
            List<SearchDto> list = new List<SearchDto>();
            SearchDto searchDto;
            foreach (var group in usersGroupedByCountry)
            {
                searchDto = new SearchDto();

                searchDto.BookName = group.Select(x => x.BookName).FirstOrDefault();
                searchDto.BookId = group.Select(x => x.BookId).FirstOrDefault();
                searchDto.AuthorName = group.Select(x => x.AuthorName).FirstOrDefault();
                searchDto.AuthorSurname = group.Select(x => x.AuthorSurname).FirstOrDefault();

                foreach (var user in group)
                {
                    if (!string.IsNullOrEmpty(user.PagesNumber.ToString()))
                    {
                        searchDto.PagesNumber += user.PagesNumber.ToString() + ",";
                    }
                    else
                    {
                        searchDto.PagesNumber += "Search phrase not found";
                    }
                }

                searchDto.PagesNumber = searchDto.PagesNumber.TrimEnd(',');

                list.Add(searchDto);
            }
            if (list is null)
            {
                throw new NotFoundException("Book not found");
            }

            return list;
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


        public BookContent GetById(Guid bookID)
        {
           return _context.BookContents.FirstOrDefault(x => x.BookId == bookID);
        }

        public void RemoveById(Guid bookID)
        {
            BookContent bookContent = _context.BookContents.FirstOrDefault(d => d.BookId == bookID);
            _context.RemoveRange(bookContent);
            _context.SaveChanges();
        } 
        public BookContent Update(BookContent bookContentDto)
        {
            try
            {
                var bookContentEntity = GetById(bookContentDto.BookId);
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
