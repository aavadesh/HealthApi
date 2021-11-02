using HealthApi.Entities;
using HealthApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HealthApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookContentController : ControllerBase
    {
        private readonly IBookContentService _contentService;
        public BookContentController(IBookContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet]
        public ActionResult<List<BookContent>> Get()
        {
            var result = _contentService.GetAll();
            return Ok(result);
        }
    }
}
