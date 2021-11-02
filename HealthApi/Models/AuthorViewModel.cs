using System;
using System.ComponentModel.DataAnnotations;

namespace HealthApi.Models
{
    public class AuthorViewModel
    {
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Slug { get; set; }

        [MaxLength(100)]
        public string Surname { get; set; }

        public Guid BookId { get; set; }
        public string BookName { get; set; }
    }
}
