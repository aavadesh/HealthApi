using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HealthApi.Entities;
using HealthApi.Exceptions;
using HealthApi.Models;

namespace HealthApi.Services
{
    public interface IBookContentService
    {
        Guid Create(BookContent bookContentDto);
        BookContent Update(BookContent bookContentDto);
        BookContent GetById(Guid bookID);
        PageResult<BookContentDto> GetAll(int page, int pageSize);
        void RemoveById(Guid bookID);
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
                        Content = e.Content,
                        pageNumber = e.PageNumber
                    }).OrderBy(x => x.BookName).ThenBy(x => x.pageNumber);

            return result.GetPaged(page, pageSize);
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
