using AutoMapper;
using HealthApi.Entities;
using HealthApi.Models;
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
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;

        public AuthorController(IAuthorService authorService, IMapper mapper)
        {
            _authorService = authorService;
            _mapper = mapper;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteById([FromRoute] Guid id)
        {
            _authorService.RemoveById(id);

            return NoContent();
        }

        [HttpPost]
        public HttpResponseMessage Create([FromBody] AuthorDto authorDto)
        {
            _authorService.Create(authorDto);

            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };

            return response;
        }
        public IActionResult Update([FromBody] AuthorDto authorDto)
        {
            try
            {
                _authorService.Update(authorDto);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Author> GetById([FromRoute] Guid id)
        {
            var result = _authorService.GetByAuthorId(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAuthorAll")]
        public ActionResult<List<AuthorDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize = 10)
        {
           var result = _authorService.GetAll(page, pageSize);
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<List<Category>> GetAll()
        {
            var result = _authorService.GetAll();
            return Ok(result);
        }
    }
}
