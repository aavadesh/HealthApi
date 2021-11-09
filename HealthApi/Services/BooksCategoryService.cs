using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HealthApi.Entities;
using HealthApi.Exceptions;

namespace HealthApi.Services
{
    public interface IBooksCategoryService
    {
        Guid Create(BookCategory bookCategoryDto);
        BookCategory GetById(Guid bookCategoryID);
        List<BookCategory> GetAll();
        void RemoveById(Guid bookCategoryID);
        BookCategory Update(Guid bookCategoryID, BookCategory bookCategoryDto);
    }

    public class BooksCategoryService : IBooksCategoryService
    {
        private readonly HealthDbContext _context;
        private readonly IMapper _mapper;

        public BooksCategoryService(HealthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Guid Create(BookCategory bookCategoryDto)
        {
            _context.BookCategories.Add(bookCategoryDto);
            _context.SaveChanges();

            return bookCategoryDto.BookId;
        }
        public BookCategory Update(Guid bookCategoryID, BookCategory bookCategoryDto)
        {
            try
            {

                var bookCategoryEntity = GetById(bookCategoryID);
                if (bookCategoryEntity == null)
                {
                    throw new NotFoundException("BookCategory not found");
                }

                _context.Remove(bookCategoryEntity);

                _context.BookCategories.Add(bookCategoryDto);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            return bookCategoryDto;
        }
        public BookCategory GetById(Guid bookCategoryID)
        {
            BookCategory bookCategory = _context.BookCategories.FirstOrDefault(d => d.BookId == bookCategoryID);
            if (bookCategory is null)
            {
                throw new NotFoundException("BookCategory not found");
            }

            return bookCategory;
        }

        public List<BookCategory> GetAll()
        {
            return _context.BookCategories.ToList();
        }

        public void RemoveById(Guid bookID)
        {
            BookCategory bookCategory = _context.BookCategories.FirstOrDefault(d => d.BookId == bookID);

            _context.RemoveRange(bookID);
            _context.SaveChanges();

        }
    }
}
