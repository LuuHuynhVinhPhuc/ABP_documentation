using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Books
{
    public class Book : AuditedAggregateRoot<Guid> 
    {
        public Book() // default constructor
        {
        }

        public Book(Guid id, string name, BookType type, DateTime publishDate, float price) 
        {
            Id = id;
            Name = name;
            Type = type;
            PublishDate = publishDate;
            Price = price;
        }

        public string Name { get; set; }
        public BookType Type { get; set; }
        public DateTime PublishDate { get; set; }
        public float Price { get; set; }


    }
}
