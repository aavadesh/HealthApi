using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HealthApi.Entities;
using HealthApi.Exceptions;
using HealthApi.Extensions;
using HealthApi.Models;

namespace HealthApi.Services
{
    public interface ICategoryService
    {
        Guid Create(Category categoryDto);
        Category Update(Guid categoryID, Category categoryDto);
        Category GetById(Guid authorID);
        PageResult<Category> GetAll(int page, int pageSize);
        void RemoveById(Guid authorID);
        List<Category> GetAll();
    }

    public class CategoryService : ICategoryService
    {
        private readonly HealthDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(HealthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Guid Create(Category categoryDto)
        {
            categoryDto.Slug = $"{categoryDto.Name}".GenerateSlug();
            _context.Categories.Add(categoryDto);
            _context.SaveChanges();

            return categoryDto.Id;
        }

        public Category Update(Guid categoryID, Category categoryDto)
        {
            try
            {

                var categoryEntity = GetById(categoryID);
                if (categoryEntity == null)
                {
                    throw new NotFoundException("Category not found");
                }

                _context.Remove(categoryEntity);

                categoryDto.Slug = $"{categoryDto.Name}".GenerateSlug();
                _context.Categories.Add(categoryDto);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            return categoryDto;
        }

        public Category GetById(Guid authorID)
        {
            Category category = _context.Categories.FirstOrDefault(d => d.Id == authorID);
            if (category is null)
            {
                throw new NotFoundException("Category not found");
            }

            return category;
        }
        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }
        public PageResult<Category> GetAll(int page, int pageSize)
        {
            return _context.Categories.GetPaged(page, pageSize);
        }

        public void RemoveById(Guid authorID)
        {
            Category category = _context.Categories.FirstOrDefault(d => d.Id == authorID);

            _context.RemoveRange(category);
            _context.SaveChanges();

        }
    }
}
