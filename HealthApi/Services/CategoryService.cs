using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HealthApi.Entities;
using HealthApi.Exceptions;

namespace HealthApi.Services
{
    public interface ICategoryService
    {
        Guid Create(Category category);
        Category Update(Guid categoryID, Category obj);
        Category GetById(Guid authorID);
        List<Category> GetAll();
        void RemoveAll(Guid authorID);
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
        public Guid Create(Category obj)
        {
            Category category = _mapper.Map<Category>(obj);

            _context.Categories.Add(category);
            _context.SaveChanges();

            return category.Id;
        }

        public Category Update(Guid categoryID, Category obj)
        {
            try
            {

                var categoryEntity = GetById(categoryID);
                if (categoryEntity == null)
                {
                    throw new NotFoundException("Category not found");
                }

                _context.Remove(categoryEntity);

                _context.Categories.Add(obj);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
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

        public void RemoveAll(Guid authorID)
        {
            Category category = _context.Categories.FirstOrDefault(d => d.Id == authorID);

            _context.RemoveRange(category);
            _context.SaveChanges();

        }
    }
}
