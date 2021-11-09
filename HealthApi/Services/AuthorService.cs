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
    public interface IAuthorService
    {
        Guid Create(AuthorDto authorDto);
        Author GetById(Guid authorID);
        List<Author> GetAll();
        void RemoveById(Guid authorID);
        Author Update(AuthorDto authorDto);
        PageResult<AuthorDto> GetAll(int page, int pageSize);
        AuthorDto GetByAuthorId(Guid bookID);
    }

    public class AuthorService : IAuthorService
    {
        private readonly HealthDbContext _context;
        private readonly IMapper _mapper;

        public AuthorService(HealthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Guid Create(AuthorDto authorDto)
        {

            Author author = new Author();
            author = _mapper.Map(authorDto, author);
            author.Slug = $"{author.Name} {author.Surname}".GenerateSlug();
            _context.Authors.Add(author);
            _context.SaveChanges();

            AuthorBook authorBook = new AuthorBook();
            authorBook.BookId = authorDto.BookId;
            authorBook.AuthorId = author.Id;
            _context.AuthorBooks.Add(authorBook);
            _context.SaveChanges();

            return author.Id;
        }
        public Author Update(AuthorDto authorDto)
        {
            Author author = null;
            try
            {
                var authorEntity = GetById(authorDto.Id);
                if (authorEntity == null)
                {
                    throw new NotFoundException("Author not found");
                }

                _context.Remove(authorEntity);

                AuthorBook authorBookEntity = _context.AuthorBooks.FirstOrDefault(d => d.AuthorId == authorDto.Id);
                if (authorBookEntity == null)
                {
                    throw new NotFoundException("AuthorBook not found");
                }
                _context.Remove(authorBookEntity);

                author = new Author();
                author = _mapper.Map(authorDto, author);
                author.Slug = $"{author.Name} {author.Surname}".GenerateSlug();
                _context.Authors.Add(author);
                _context.SaveChanges();

                AuthorBook authorBook = new AuthorBook
                {
                    BookId = authorDto.BookId,
                    AuthorId = author.Id
                };
                _context.AuthorBooks.Add(authorBook);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            return author;
        }
        public Author GetById(Guid authorID)
        {
            Author author = _context.Authors.FirstOrDefault(d => d.Id == authorID);
            if (author is null)
            {
                throw new NotFoundException("Author not found");
            }

            return author;
        }

        public List<Author> GetAll()
        {
            return _context.Authors.ToList();
        }

        public AuthorDto GetByAuthorId(Guid authorID)
        {
            return _context.Authors.Where(x => x.Id == authorID)
            .Select(a =>
                new AuthorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Slug = a.Slug,
                    Surname = a.Surname,
                    BookId = a.AuthorBooks.Select(p => p.Book.Id).FirstOrDefault()
                }).FirstOrDefault();
        }

        public PageResult<AuthorDto> GetAll(int page, int pageSize)
        {
            IQueryable<AuthorDto> result = _context.Authors
            .Select(a =>
                new AuthorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Slug = a.Slug,
                    Surname = a.Surname,
                    BookId = a.AuthorBooks.Select(p => p.Book.Id).FirstOrDefault(),
                    BookName = a.AuthorBooks.Select(p => p.Book.Name).FirstOrDefault()
                });

            return result.GetPaged(page, pageSize);
        }

        public void RemoveById(Guid authorID)
        {
            Author author = _context.Authors.FirstOrDefault(d => d.Id == authorID);
            AuthorBook authorBook  = _context.AuthorBooks.FirstOrDefault(d => d.AuthorId == authorID);

            _context.RemoveRange(author);
            if (authorBook != null)
                _context.RemoveRange(authorBook);
            _context.SaveChanges();

        }
    }
}
