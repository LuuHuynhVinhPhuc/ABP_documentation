using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.Books
{
    public class CreateBookDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public BookType Type { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        [Required]
        [Range(0.01, float.MaxValue)]
        public float Price { get; set; }
    }
}
