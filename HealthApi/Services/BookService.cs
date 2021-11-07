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
        Book Update(Book book);
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

            AuthorBook authorBook = new AuthorBook();
            authorBook.BookId = obj.Id;
            authorBook.AuthorId = obj.AuthorId;
            _context.AuthorBooks.Add(authorBook);
            _context.SaveChanges();

            return obj.Id;
        }
        public Book Update(Book book)
        {
            try
            {

                var bookEntity = GetById(book.Id);
                if (bookEntity == null)
                {
                    throw new NotFoundException("Book not found");
                }
                _context.Remove(bookEntity);
                _context.Books.Add(book);
                _context.SaveChanges();

                BookCategory bookCategoryEntity = _context.BookCategories.FirstOrDefault(d => d.BookId == bookEntity.Id);
                if (bookEntity == null)
                {
                    throw new NotFoundException("BookCategory not found");
                }
                _context.Remove(bookCategoryEntity);
                BookCategory bookCategory = new BookCategory();
                bookCategory.BookId = book.Id;
                bookCategory.CategoryId = book.CategoryId;
                _context.BookCategories.Add(bookCategory);
                _context.SaveChanges();

                AuthorBook authorBookEntity = _context.AuthorBooks.FirstOrDefault(d => d.BookId == bookEntity.Id);
                if (bookEntity == null)
                {
                    throw new NotFoundException("Author not found");
                }
                _context.Remove(authorBookEntity);
                AuthorBook authorBook = new AuthorBook();
                authorBook.BookId = book.Id;
                authorBook.AuthorId = book.AuthorId;
                _context.AuthorBooks.Add(authorBook);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            return book;
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
            return _context.Books
           .Where(x => x.Id == bookID)
           .Select(a => new BookViewModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Slug = a.Slug,
                        SlugName = a.SlugName,
                        CategoryId = a.BookCategories.FirstOrDefault(x => x.BookId == a.Id).CategoryId,
                        AuthorId = a.AuthorBooks.FirstOrDefault(x => x.BookId == a.Id).AuthorId
           }).FirstOrDefault();
        }

        public List<BookViewModel> GetBookAll()
        {
            return _context.Books
           .Select(a =>
               new BookViewModel
               {
                   Id = a.Id,
                   Name = a.Name,
                   Slug = a.Slug,
                   SlugName = a.SlugName,
                   CategoryId = a.BookCategories.Select(p => p.Category.Id).FirstOrDefault(),
                   CategoryName = a.BookCategories.Select(p => p.Category.Name).FirstOrDefault(),
                   AuthorId = a.AuthorBooks.Select(p => p.Author.Id).FirstOrDefault(),
                   AuthorFullName = a.AuthorBooks.Select(p => p.Author.Name + p.Author.Surname).FirstOrDefault(),
               }).ToList();
        }

        public void RemoveAll(Guid bookID)
        {
            Book book = _context.Books.FirstOrDefault(d => d.Id == bookID);
            BookCategory bookCategory = _context.BookCategories.FirstOrDefault(d => d.BookId == bookID);

            _context.RemoveRange(book);
            if (bookCategory != null)
                _context.RemoveRange(bookCategory);
            _context.SaveChanges();

        }
    }
}
