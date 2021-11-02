using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApi.Entities
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Slug { get; set; }

        [MaxLength(200)]
        public string SlugName { get; set; }
        [NotMapped]
        public Guid CategoryId { get; set; }
        public ICollection<BookCategory> BookCategories { get; set; }
        public ICollection<AuthorBook> AuthorBooks { get; set; }

    }
}
