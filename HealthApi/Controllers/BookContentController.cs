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
    public class BookContentController : ControllerBase
    {
        private readonly IBookContentService _bookContentService;
        public BookContentController(IBookContentService bookContentService)
        {
            _bookContentService = bookContentService;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id)
        {
            _bookContentService.RemoveAll(id);

            return NoContent();
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] BookContent bookContent)
        {
            _bookContentService.Create(bookContent);
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };

            return response;
        }

        public IActionResult Put([FromBody] BookContent bookContent)
        {
            try
            {
                _bookContentService.Update(bookContent);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("{id}")]
        public ActionResult<BookContent> Get([FromRoute] Guid id)
        {
            BookContent bookContent = _bookContentService.GetById(id);
            return Ok(bookContent);
        }

        [HttpGet]
        public ActionResult<List<BookContentViewModel>> Get()
        {
            var result = _bookContentService.GetAll();
            return Ok(result);
        }
    }
}
