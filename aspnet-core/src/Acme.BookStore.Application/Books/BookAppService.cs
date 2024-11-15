using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    public class BookAppService : BookStoreAppService
    {
        // readonly 
        private readonly BookManager _bookManager;
        private readonly IBookRepository _bookRepository;

        public BookAppService(BookManager bookManager, IBookRepository bookRepository)
        {
            _bookManager = bookManager;
            _bookRepository = bookRepository;
        }

        public async Task<BookDto> CreateAsync(CreateBookDto input)
        {
            var book = await _bookManager.CreateAsync(input.Name, input.Type, input.Price, input.PublishDate);

            return ObjectMapper.Map<Book, BookDto>(book);
        }
        public async Task UpdateAsync(Guid id ,UpdateBookDto input)
        {
            var book = await _bookRepository.GetAsync(id);

            input.Name = book.Name;
            input.Type = book.Type;
            input.Price = book.Price;
            input.PublishDate = book.PublishDate;

            await _bookRepository.UpdateAsync(book);
        }

        public async Task<PagedResultDto<BookDto>> GetListAsync(GetBookListDto input)
        {

            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Book.Name);
            }

            var bookList = await _bookRepository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Filter
            );

            var totalCount = input.Filter == null
                ? await _bookRepository.CountAsync()
                : await _bookRepository.CountAsync(
                    author => author.Name.Contains(input.Filter));

            return new PagedResultDto<BookDto>(
                totalCount,
                ObjectMapper.Map<List<Book>, List<BookDto>>(bookList)
            );

        }

        public async Task<BookDto> GetIDAsync(Guid id)
        {
            var book = await _bookRepository.FindAsync(id);
            return ObjectMapper.Map<Book, BookDto>(book);
        }

        public async Task<BookDto> GetBookByNameAsync(string name)
        {
            var book = await _bookManager.FindByNameAsync(name);

            return ObjectMapper.Map<Book, BookDto>(book);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _bookRepository.DeleteAsync(id);
        }
    }
}
