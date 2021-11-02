using AutoMapper;
using HealthApi.Entities;
using HealthApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace HealthApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id)
        {
            _categoryService.RemoveAll(id);

            return NoContent();
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] Category category)
        {
            category.Slug = category.Name.GenerateSlug();
            _categoryService.Create(category);
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };

            return response;
        }

        public IActionResult Put([FromBody] Category category)
        {
            try
            {
                category.Slug = category.Name.GenerateSlug();
                _categoryService.Update(category.Id, category);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }


            [HttpGet("{id}")]
        public ActionResult<Category> Get([FromRoute] Guid id)
        {
            Category category = _categoryService.GetById(id);
            return Ok(category);
        }

        [HttpGet]
        public ActionResult<List<Category>> Get()
        {
            var result = _categoryService.GetAll();
            return Ok(result);
        }
    }
}
