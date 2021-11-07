using System;
using System.Collections.Generic;
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
        Guid Create(BookContent bookContent);
        BookContent Update(BookContent bookContent);
        BookContent GetById(Guid bookID);
        List<BookContentViewModel> GetAll();
        void RemoveAll(Guid bookID);
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
        public List<BookContentViewModel> GetAll()
        {
            return (from e in _context.BookContents
                    join d in _context.Books on e.BookId equals d.Id
                    select new BookContentViewModel
                    {
                        Id = e.Id,
                        BookName = d.Name,
                        BookId = d.Id,
                        Content = e.Content,
                        pageNumber = e.PageNumber
                    }).OrderBy(x => x.BookName).ThenBy(x => x.pageNumber).ToList();
        }

        public Guid Create(BookContent obj)
        {
            BookContent bookContent = _mapper.Map<BookContent>(obj);

            _context.BookContents.Add(bookContent);
            _context.SaveChanges();

            return bookContent.BookId;
        } 
        public BookContent GetById(Guid bookID)
        {
           return _context.BookContents.FirstOrDefault(x => x.BookId == bookID);
        }

        public void RemoveAll(Guid bookID)
        {
            BookContent bookContent = _context.BookContents.FirstOrDefault(d => d.BookId == bookID);
            _context.RemoveRange(bookContent);
        } 
        public BookContent Update(BookContent bookContent)
        {
            try
            {
                var bookContentEntity = GetById(bookContent.BookId);
                if (bookContentEntity == null)
                {
                    throw new NotFoundException("BookContent not found");
                }

                _context.Remove(bookContentEntity);

                _context.BookContents.Add(bookContent);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            return bookContent;
        }
    }
}
