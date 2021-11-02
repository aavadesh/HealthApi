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
        public ActionResult Delete([FromRoute] Guid id)
        {
            _authorService.RemoveAll(id);

            return NoContent();
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] AuthorViewModel authorViewModel)
        {
            _authorService.Create(authorViewModel);

            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };

            return response;
        }
        public IActionResult Put([FromBody] AuthorViewModel authorViewModel)
        {
            try
            {
                _authorService.Update(authorViewModel);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Author> Get([FromRoute] Guid id)
        {
            Author author = _authorService.GetById(id);
            return Ok(author);
        }

        [HttpGet]
        public ActionResult<List<Author>> Get()
        {
            var result = _authorService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAuthorAll")]
        public ActionResult<List<AuthorViewModel>> GetAuthorAll()
        {
            var result = _authorService.GetAuthorAll();
            return Ok(result);
        }

        [HttpGet("GetByAuthorId/{id}")]
        public ActionResult<AuthorViewModel> GetByAuthorId([FromRoute] Guid id)
        {
            var result = _authorService.GetByAuthorId(id);
            return Ok(result);
        }
    }
}
