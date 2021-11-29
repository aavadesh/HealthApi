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
            string sql = @"select distinct t2.Name as BookName,t2.Id as BookId, t4.Name as AuthorName, t4.Surname as AuthorSurname,
                            isnull(STUFF(
                            (SELECT  distinct ', ' + CONVERT(varchar(10),t1.PageNumber)
                            FROM BookContents t1  join Books t on t1.BookId = t.Id 
                            where t2.Id = t.Id FOR XML PATH('')), 1, 1, ''), 'Search Phase not found') PagesNumber from BookContents content
                            join Books t2 on t2.Id = content.BookId 
                            left join AuthorBooks t3 on t2.Id = t3.BookId 
                            left join Authors t4 on t3.AuthorId = t4.Id 
                            Where content.Content like '%" + searchText + "%'";

            List<SearchDto> search = _context.Set<SearchDto>().FromSqlRaw(sql).ToList();

            
            if (search is null)
            {
                throw new NotFoundException("Book not found");
            }

            return search;
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
