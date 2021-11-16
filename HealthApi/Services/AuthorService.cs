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
        Guid Create(Author authorDto);
        Author GetById(Guid authorID);
        List<Author> GetAll();
        void RemoveById(Guid authorID);
        Author Update(Author authorDto);
        PageResult<Author> GetAll(int page, int pageSize);
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
        public Guid Create(Author authorDto)
        {
            authorDto.Slug = $"{authorDto.Name} {authorDto.Surname}".GenerateSlug();
            _context.Authors.Add(authorDto);
            _context.SaveChanges();
            return authorDto.Id;
        }
        public Author Update(Author authorDto)
        {
            try
            {
                var authorEntity = GetById(authorDto.Id);
                if (authorEntity == null)
                {
                    throw new NotFoundException("Author not found");
                }
                _context.Remove(authorEntity);

                authorDto.Slug = $"{authorDto.Name} {authorDto.Surname}".GenerateSlug();
                _context.Authors.Add(authorDto);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            return authorDto;
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
            return _context.Authors.OrderBy(x => x.Id).ToList();
        }

        public PageResult<Author> GetAll(int page, int pageSize)
        {
            return _context.Authors.OrderBy(x => x.Id).GetPaged(page, pageSize);
        }

        public void RemoveById(Guid authorID)
        {
            Author author = _context.Authors.FirstOrDefault(d => d.Id == authorID);

            _context.RemoveRange(author);
            _context.SaveChanges();

        }
    }
}
