﻿using HealthApi.Entities;
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
        public ActionResult DeleteById([FromRoute] Guid id)
        {
            _bookContentService.RemoveById(id);

            return NoContent();
        }

        [HttpPost]
        public HttpResponseMessage Create([FromBody] BookContent bookContentDto)
        {
            _bookContentService.Create(bookContentDto);
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };

            return response;
        }

        public IActionResult Update([FromBody] BookContent bookContentDto)
        {
            try
            {
                _bookContentService.Update(bookContentDto);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("{id}")]
        public ActionResult<BookContent> GetById([FromRoute] Guid id)
        {
            BookContent bookContent = _bookContentService.GetById(id);
            return Ok(bookContent);
        }

        [HttpGet]
        public ActionResult<List<BookContentDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize = 10)
        {
            var result = _bookContentService.GetAll(page, pageSize);
            return Ok(result);
        }
    }
}
