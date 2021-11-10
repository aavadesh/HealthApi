using HealthApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<SearchDto> GetByText([FromRoute] string searchText)
        {
            //var result = _bookService.GetByBookId(id);
            return Ok(null);
        }
    }
}
