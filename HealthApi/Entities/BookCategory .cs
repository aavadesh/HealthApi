
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApi.Entities
{
    public class BookCategory
    {
        [Key]
        public Guid BookId { get; set; }

        [Key]
        public Guid CategoryId { get; set; }
        public Book Book { get; set; }
        public Category Category { get; set; }
    }
}
