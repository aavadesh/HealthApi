using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace HealthApi.Entities
{
    public class AuthorBook
    {
        [Key]
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        [Key]
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
