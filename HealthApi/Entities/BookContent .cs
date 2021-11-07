
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApi.Entities
{
    public class BookContent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        [MaxLength(8000)]
        public string Content { get; set; }
        public int PageNumber { get; set; }
    }
}
