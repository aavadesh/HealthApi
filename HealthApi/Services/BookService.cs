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
    public interface IBookService
    {
        Guid Create(Book book);
        Book GetById(Guid bookID);
        List<Book> GetAll();
        void RemoveAll(Guid bookID);
        Book Update(Guid bookID, Book obj);
        List<BookViewModel> GetBookAll();
        BookViewModel GetByBookId(Guid bookID);
    }

    public class BookService : IBookService
    {
        private readonly HealthDbContext _context;
        private readonly IMapper _mapper;

        public BookService(HealthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Guid Create(Book obj)
        {
            _context.Books.Add(obj);
            _context.SaveChanges();

            BookCategory bookCategory = new BookCategory();
            bookCategory.BookId = obj.Id;
            bookCategory.CategoryId = obj.CategoryId;
            _context.BookCategories.Add(bookCategory);
            _context.SaveChanges();

            return obj.Id;
        }
        public Book Update(Guid bookID, Book obj)
        {
            try
            {

                var bookEntity = GetById(bookID);
                if (bookEntity == null)
                {
                    throw new NotFoundException("Book not found");
                }

                _context.Remove(bookEntity);

                BookCategory bookCategoryEntity = _context.BookCategories.FirstOrDefault(d => d.BookId == bookID);
                if (bookEntity == null)
                {
                    throw new NotFoundException("BookCategory not found");
                }

                _context.Remove(bookCategoryEntity);

                _context.Books.Add(obj);
                _context.SaveChanges();

                BookCategory bookCategory = new BookCategory();
                bookCategory.BookId = obj.Id;
                bookCategory.CategoryId = obj.CategoryId;
                _context.BookCategories.Add(bookCategory);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
        public Book GetById(Guid bookID)
        {
            Book book = _context.Books.FirstOrDefault(d => d.Id == bookID);
            if (book is null)
            {
                throw new NotFoundException("Book not found");
            }

            return book;
        }

        public List<Book> GetAll()
        {
            return _context.Books.ToList();
        }

        public BookViewModel GetByBookId(Guid bookID)
        {
            return (from e in _context.Books
                    join d in _context.BookCategories on e.Id equals d.BookId
                    where e.Id == bookID
                    select new BookViewModel
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Slug = e.Slug,
                        SlugName = e.SlugName,
                        CategoryId = d.CategoryId
                    }).FirstOrDefault();
        }

        public List<BookViewModel> GetBookAll()
        {
            return _context.Books
           .Include(a => a.BookCategories)
           .Select(a =>
               new BookViewModel
               {
                   Id = a.Id,
                   Name = a.Name,
                   Slug = a.Slug,
                   SlugName = a.SlugName,
                   CategoryId = a.BookCategories.Select(p => p.Category.Id).FirstOrDefault(),
                   CategoryName = a.BookCategories.Select(p => p.Category.Name).FirstOrDefault()
               }).ToList();
        }

        public void RemoveAll(Guid bookID)
        {
            Book book = _context.Books.FirstOrDefault(d => d.Id == bookID);
            BookCategory bookCategory = _context.BookCategories.FirstOrDefault(d => d.BookId == bookID);

            _context.RemoveRange(book); 
            _context.RemoveRange(bookCategory);
            _context.SaveChanges();

        }
    }
}
