using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books
{
    public interface IBookAppService : IApplicationService
    {
        Task<BookDto> CreateAsync(CreateBookDto input);
        Task UpdateAsync(UpdateBookDto input);

        Task<PagedResultDto<BookDto>> GetListAsync(GetBookListDto input);

        Task DeleteAsync(Guid id);

        Task<byte[]> ExportBooksToExcelAsync();

        Task ImportBooksFromExcelAsync(IFormFile file);
    }
}
