using System;
using System.ComponentModel.DataAnnotations;

namespace HealthApi.Models
{
    public class BookContentDto
    {
        [MaxLength(8000)]
        public string Content { get; set; }
        public int pageNumber { get; set; }
        public string BookName { get; set; }

        public Guid BookId { get; set; }

        public Guid Id { get; set; }
    }
}
