using System;
using System.ComponentModel.DataAnnotations;

namespace HealthApi.Models
{
    public class BookContentViewModel
    {
        public Guid Id { get; set; }
        [MaxLength(8000)]
        public string Content { get; set; }
        public int PageNumer { get; set; }
        public string BookName { get; set; }

        public Guid BookId { get; set; }
    }
}
