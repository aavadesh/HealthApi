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
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService; 
        private readonly IMapper _mapper;

        public BookController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteById([FromRoute] Guid id)
        {
            _bookService.RemoveById(id);

            return NoContent();
        }

        [HttpPost]
        public HttpResponseMessage Create([FromBody] BookDto bookDto)
        {
            Book book = new Book();
            book  = _mapper.Map(bookDto, book);

            _bookService.Create(book);

            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };

            return response;
        }
        public IActionResult Update([FromBody] BookDto bookDto)
        {
            try
            {
                Book book = _mapper.Map<Book>(bookDto);
                _bookService.Update(book);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetById([FromRoute] Guid id)
        {
            var result = _bookService.GetByBookId(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetBookAll")]
        public ActionResult<List<BookDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize = 10)
        {
            var result = _bookService.GetAll(page, pageSize);
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<List<BookDto>> GetAll()
        {
            var result = _bookService.GetAll();
            return Ok(result);
        }
    }
}
