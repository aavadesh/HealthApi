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
    public interface IAuthorService
    {
        Guid Create(AuthorViewModel authorViewModel);
        Author GetById(Guid authorID);
        List<Author> GetAll();
        void RemoveAll(Guid authorID);
        Author Update(AuthorViewModel authorViewModel);
        List<AuthorViewModel> GetAuthorAll();
        AuthorViewModel GetByAuthorId(Guid bookID);
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
        public Guid Create(AuthorViewModel authorViewModel)
        {

            Author author = new Author();
            author = _mapper.Map(authorViewModel, author);
            author.Slug = $"{author.Name.GenerateSlug()} {author.Surname.GenerateSlug()}";
            _context.Authors.Add(author);
            _context.SaveChanges();

            AuthorBook authorBook = new AuthorBook();
            authorBook.BookId = authorViewModel.BookId;
            authorBook.AuthorId = author.Id;
            _context.AuthorBooks.Add(authorBook);
            _context.SaveChanges();

            return author.Id;
        }
        public Author Update(AuthorViewModel authorViewModel)
        {
            Author author = null;
            try
            {
                var authorEntity = GetById(authorViewModel.Id);
                if (authorEntity == null)
                {
                    throw new NotFoundException("Author not found");
                }

                _context.Remove(authorEntity);

                AuthorBook authorBookEntity = _context.AuthorBooks.FirstOrDefault(d => d.AuthorId == authorViewModel.Id);
                if (authorBookEntity == null)
                {
                    throw new NotFoundException("AuthorBook not found");
                }
                _context.Remove(authorBookEntity);

                author = new Author();
                author = _mapper.Map(authorViewModel, author);
                author.Slug = $"{author.Name.GenerateSlug()} {author.Surname.GenerateSlug()}";
                _context.Authors.Add(author);
                _context.SaveChanges();

                AuthorBook authorBook = new AuthorBook();
                authorBook.BookId = authorViewModel.BookId;
                authorBook.AuthorId = author.Id;
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

        public AuthorViewModel GetByAuthorId(Guid authorID)
        {
            return _context.Authors.Where(x => x.Id == authorID)
            .Include(a => a.AuthorBooks)
            .Select(a =>
                new AuthorViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Slug = a.Slug,
                    Surname = a.Surname,
                    BookId = a.AuthorBooks.Select(p => p.Book.Id).FirstOrDefault()
                }).FirstOrDefault();
        }

        public List<AuthorViewModel> GetAuthorAll()
        {
            return _context.Authors
            .Include(a => a.AuthorBooks)
            .Select(a =>
                new AuthorViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Slug = a.Slug,
                    Surname = a.Surname,
                    BookId = a.AuthorBooks.Select(p => p.Book.Id).FirstOrDefault(),
                    BookName = a.AuthorBooks.Select(p => p.Book.Name).FirstOrDefault()
                }).ToList();
        }

        public void RemoveAll(Guid authorID)
        {
            Author author = _context.Authors.FirstOrDefault(d => d.Id == authorID);
            AuthorBook authorBook  = _context.AuthorBooks.FirstOrDefault(d => d.AuthorId == authorID);

            _context.RemoveRange(author);
            _context.RemoveRange(authorBook);
            _context.SaveChanges();

        }
    }
}
