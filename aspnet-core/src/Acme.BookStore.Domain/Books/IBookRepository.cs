using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Books
{
    public interface IBookRepository : IRepository<Book, Guid>
    {
        Task<Book> FindByNameAsync(string name);

        Task<Book> FindByIDAsync(Guid id);

        Task<List<Book>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string filter = null
        );
        Task<int> CountAsync(Expression<Func<Book, bool>> predicate = null);

        Task DeleteAsync(Guid id);

        Task<List<Book>> GetAllListAsync();
    }
}
