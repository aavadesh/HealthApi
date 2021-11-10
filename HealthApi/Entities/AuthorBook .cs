using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace HealthApi.Entities
{
    public class AuthorBook
    {
       // [Key]
        public Guid BookId { get; set; }
        public Book Book { get; set; }
       // [Key]
        public virtual Guid AuthorId { get; set; }
        public virtual Author Author { get; set; }
    }
}
