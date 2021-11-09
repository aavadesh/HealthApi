using AutoMapper;
using HealthApi.Entities;
using HealthApi.Models;
using HealthApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult DeleteById([FromRoute] Guid id)
        {
            _categoryService.RemoveById(id);

            return NoContent();
        }

        [HttpPost]
        public HttpResponseMessage Create([FromBody] Category categoryDto)
        {
            _categoryService.Create(categoryDto);
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };

            return response;
        }

        public IActionResult Update([FromBody] Category categoryDto)
        {
            try
            {
                _categoryService.Update(categoryDto.Id, categoryDto);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("{id}")]
        public ActionResult<Category> GetById([FromRoute] Guid id)
        {
            Category category = _categoryService.GetById(id);
            return Ok(category);
        }

        [HttpGet]
        [Route("GetCategoryAll")]
        public ActionResult<List<Category>> GetAll([FromQuery] int page, [FromQuery] int pageSize = 10)
        {
            var result = _categoryService.GetAll(page, pageSize);
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<List<Category>> GetAll()
        {
            var result = _categoryService.GetAll();
            return Ok(result);
        }
    }
}
