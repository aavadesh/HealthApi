using System;
using System.ComponentModel.DataAnnotations;

namespace HealthApi.Models
{
    public class SearchDto
    {
        public Guid AuthorId { get; set; }
        public string AuthorFullName { get; set; }
        public int PagesNumber { get; set; }
        public Guid BookId { get; set; }
        public string BookName { get; set; }
    }
}
