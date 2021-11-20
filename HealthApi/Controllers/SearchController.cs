using HealthApi.Models;
using HealthApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HealthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }


        [HttpGet]
        public ActionResult<List<SearchDto>> GetAll([FromQuery] string q)
        {
            var result = _searchService.GetByString(q);
            if (result.Count > 0)
            {
                return Ok(result);
            }

            return NotFound();
        }
    }
}
