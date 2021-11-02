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
        Guid Create(BookCategory bookCategory);
        BookCategory GetById(Guid bookCategoryID);
        List<BookCategory> GetAll();
        void RemoveAll(Guid bookCategoryID);
        BookCategory Update(Guid bookCategoryID, BookCategory obj);
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
        public Guid Create(BookCategory obj)
        {
            BookCategory bookCategory = _mapper.Map<BookCategory>(obj);

            _context.BookCategories.Add(bookCategory);
            _context.SaveChanges();

            return bookCategory.BookId;
        }
        public BookCategory Update(Guid bookCategoryID, BookCategory obj)
        {
            try
            {

                var bookCategoryEntity = GetById(bookCategoryID);
                if (bookCategoryEntity == null)
                {
                    throw new NotFoundException("BookCategory not found");
                }

                _context.Remove(bookCategoryEntity);

                _context.BookCategories.Add(obj);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
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

        public void RemoveAll(Guid bookID)
        {
            BookCategory bookCategory = _context.BookCategories.FirstOrDefault(d => d.BookId == bookID);

            _context.RemoveRange(bookID);
            _context.SaveChanges();

        }
    }
}
