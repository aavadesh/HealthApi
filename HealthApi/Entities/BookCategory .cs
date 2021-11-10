
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApi.Entities
{
    public class BookCategory
    {
       // [Key]
        public Guid BookId { get; set; }

       // [Key]
        public Guid CategoryId { get; set; }
        public virtual Book Book { get; set; }
        public virtual Category Category { get; set; }
    }
}
