using HealthApi.Models;
using HealthApi.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{searchPhrase}")]
        public ActionResult<SearchDto> GetByString([FromRoute] string searchPhrase)
        {
            var result = _searchService.GetByString(searchPhrase);
            return Ok(result);
        }
    }
}
