using AutoMapper;
using HealthApi.Entities;
using HealthApi.Exceptions;
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
        public ActionResult Delete([FromRoute] Guid id)
        {
            _bookService.RemoveAll(id);

            return NoContent();
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] BookViewModel bookViewModel)
        {
            Book book = new Book();
            book  = _mapper.Map(bookViewModel, book);
            book.Slug = book.Name.GenerateSlug();
            book.SlugName = book.Slug.GenerateSlug();

            _bookService.Create(book);

            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };

            return response;
        }
        public IActionResult Put([FromBody] BookViewModel bookViewModel)
        {
            try
            {
                Book book = _mapper.Map<Book>(bookViewModel);
                book.Slug = book.Name.GenerateSlug();
                book.SlugName = book.Slug.GenerateSlug();
                _bookService.Update(book.Id, book);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Book> Get([FromRoute] Guid id)
        {
            Book Book = _bookService.GetById(id);
            return Ok(Book);
        }

        [HttpGet]
        public ActionResult<List<Book>> Get()
        {
            var result = _bookService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetBookAll")]
        public ActionResult<List<BookViewModel>> GetBookAll()
        {
            var result = _bookService.GetBookAll();
            return Ok(result);
        }

        [HttpGet("GetByBookId/{id}")]
        public ActionResult<BookViewModel> GetByBookId([FromRoute] Guid id)
        {
            var result = _bookService.GetByBookId(id);
            return Ok(result);
        }
    }
}
