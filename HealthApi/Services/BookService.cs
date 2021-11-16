using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HealthApi.Entities;
using HealthApi.Exceptions;
using HealthApi.Extensions;
using HealthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthApi.Services
{
    public interface IBookService
    {
        Guid Create(Book bookDto);
        Book GetById(Guid bookID);
        List<Book> GetAll();
        void RemoveById(Guid bookID);
        Book Update(Book bookDto);
        PageResult<BookDto> GetAll(int page, int pageSize);
        BookDto GetByBookId(Guid bookID);
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
        public Guid Create(Book bookDto)
        {
            _context.Books.Add(bookDto);
            bookDto.Slug = $"{bookDto.Name}".GenerateSlug();
            bookDto.SlugName = $"{bookDto.Name}".GenerateSlug();
            _context.SaveChanges();

            BookCategory bookCategory = new BookCategory
            {
                BookId = bookDto.Id,
                CategoryId = bookDto.CategoryId
            };
            _context.BookCategories.Add(bookCategory);
            _context.SaveChanges();

            AuthorBook authorBook = new AuthorBook
            {
                BookId = bookDto.Id,
                AuthorId = bookDto.AuthorId
            };
            _context.AuthorBooks.Add(authorBook);
            _context.SaveChanges();

            return bookDto.Id;
        }
        public Book Update(Book bookDto)
        {
            try
            {

                var bookEntity = GetById(bookDto.Id);
                if (bookEntity == null)
                {
                    throw new NotFoundException("Book not found");
                }
                _context.Remove(bookEntity);
                bookDto.Slug = $"{bookDto.Name}".GenerateSlug();
                bookDto.SlugName = $"{bookDto.Name}".GenerateSlug();
                _context.Books.Add(bookDto);
                _context.SaveChanges();

                BookCategory bookCategoryEntity = _context.BookCategories.FirstOrDefault(d => d.BookId == bookEntity.Id);
                if (bookEntity == null)
                {
                    throw new NotFoundException("BookCategory not found");
                }
                _context.Remove(bookCategoryEntity);
                BookCategory bookCategory = new BookCategory
                {
                    BookId = bookDto.Id,
                    CategoryId = bookDto.CategoryId
                };
                _context.BookCategories.Add(bookCategory);
                _context.SaveChanges();

                AuthorBook authorBookEntity = _context.AuthorBooks.FirstOrDefault(d => d.BookId == bookEntity.Id);
                if (bookEntity == null)
                {
                    throw new NotFoundException("Author not found");
                }
                _context.Remove(authorBookEntity);
                AuthorBook authorBook = new AuthorBook
                {
                    BookId = bookDto.Id,
                    AuthorId = bookDto.AuthorId
                };
                _context.AuthorBooks.Add(authorBook);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            return bookDto;
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
            return _context.Books.OrderBy(x => x.Id).ToList();
        }

        public BookDto GetByBookId(Guid bookID)
        {
            return _context.Books
           .Where(x => x.Id == bookID)
           .Select(a => new BookDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Slug = a.Slug,
                        SlugName = a.SlugName,
                        CategoryId = a.BookCategories.FirstOrDefault(x => x.BookId == a.Id).CategoryId,
                        AuthorId = a.AuthorBooks.FirstOrDefault(x => x.BookId == a.Id).AuthorId
           }).FirstOrDefault();
        }

        public PageResult<BookDto> GetAll(int page, int pageSize)
        {
            IQueryable<BookDto> result = _context.Books
           .Select(a =>
               new BookDto
               {
                   Id = a.Id,
                   Name = a.Name,
                   Slug = a.Slug,
                   SlugName = a.SlugName,
                   CategoryId = a.BookCategories.Select(p => p.Category.Id).FirstOrDefault(),
                   CategoryName = a.BookCategories.Select(p => p.Category.Name).FirstOrDefault(),
                   AuthorId = a.AuthorBooks.Select(p => p.Author.Id).FirstOrDefault(),
                   AuthorFullName = a.AuthorBooks.Select(p => p.Author.Name + p.Author.Surname).FirstOrDefault(),
               });
           return result.OrderBy(x => x.Id).GetPaged(page, pageSize);
        }

        public void RemoveById(Guid bookID)
        {
            Book book = _context.Books.FirstOrDefault(d => d.Id == bookID);

            _context.RemoveRange(book);
            _context.SaveChanges();

        }
    }
}
