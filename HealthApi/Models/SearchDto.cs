using System;
using System.ComponentModel.DataAnnotations;

namespace HealthApi.Models
{
    public class SearchDto
    {
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public string PagesNumber { get; set; }
        public Guid BookId { get; set; }
        public string BookName { get; set; }
    }
}
