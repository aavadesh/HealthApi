using System;
using System.ComponentModel.DataAnnotations;

namespace HealthApi.Models
{
    public class BookDto
    {
        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Slug { get; set; }

        [MaxLength(200)]
        public string SlugName { get; set; }

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Guid AuthorId { get; set; }
        public string AuthorFullName { get; set; }
    }
}
