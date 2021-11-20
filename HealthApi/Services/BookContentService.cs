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
            //var bookContent = String.Join(",", _context.BookContents.Select(p => p.PageNumber));

            string sql = @"select t2.Name as BookName,t2.Id as BookId, t4.Name as AuthorName, t4.Surname as AuthorSurname,
                                   isnull(STUFF(
                                       (SELECT distinct ', ' + CONVERT(varchar(10),t1.PageNumber)
	                                  FROM BookContents t1 inner join Books t on t1.BookId = t.Id 
	                                  where t2.Id = t.Id FOR XML PATH('')), 1, 1, ''), 'Search Phase not found') PagesNumber
                                from Books t2 
	                                  inner join AuthorBooks t3 on t2.Id = t3.BookId
	                                  inner join Authors t4 on t3.AuthorId = t4.Id
	                                  Where t2.Name like '%" + searchText + "%' or t4.Name like '%" + searchText + "%' or t4.Surname like '%" + searchText + "%' ";

            List<SearchDto> searchResult = _context.Set<SearchDto>().FromSqlRaw(sql).ToList();

            List<SearchDto> result = searchResult.ToList();

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
