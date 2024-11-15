using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp;

namespace Acme.BookStore.Books
{
    public class BookManager : DomainService
    {
        private readonly IRepository<Book, Guid> _bookRepository;

        public BookManager(IRepository<Book, Guid> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> CreateAsync(string name,BookType type, float price, DateTime publishDate)
        {
            // Validation logic
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessException("Name cannot be null");
            }

            if (name.Length > 200)
            {
                throw new BusinessException("Book is not over 100 character.");
            }

            if (price < 0)
            {
                throw new BusinessException("Price is larger than 0 ");
            }

            if (publishDate > DateTime.Today)
            {
                throw new BusinessException("Published date must be less than or equal to today.");
            }


            // Tạo Book entity mới nếu các validation đều hợp lệ
            var book = new Book(GuidGenerator.Create(),name, type, publishDate, price);


            return await _bookRepository.InsertAsync(book);
        }

        public async Task<Book> UpdateAsync(Guid id, string name, BookType type, float price, DateTime publishDate)
        {
            // check existed book in DB 
            var book = await _bookRepository.GetAsync(id);

            if (book == null)
            {
                throw new BookAlreadyExitstedException(name);
            }

            // Validation logic
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessException("Name cannot be null");
            }

            if (name.Length > 200)
            {
                throw new BusinessException("Book is not over 100 character.");
            }

            if (price < 0)
            {
                throw new BusinessException("Price is larger than 0 ");
            }

            if (publishDate >= DateTime.Today)
            {
                throw new BusinessException("Published date must be less than or equal to today.");
            }

            // update filed data
            book.Name = name;
            book.Type = type;
            book.Price = price;
            book.PublishDate = publishDate;

            return await _bookRepository.UpdateAsync(book);
        }

        public async Task<Book> FindByNameAsync(string name)
        {
            return await _bookRepository.FirstOrDefaultAsync(b => b.Name == name);
        }


        public async Task DeleteAsync(Guid id)
        {
            await _bookRepository.DeleteAsync(id); // Xóa theo ID
        }
    }
}
