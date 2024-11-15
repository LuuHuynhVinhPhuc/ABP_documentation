using Acme.BookStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.Books
{
    public class BookRepository : EfCoreRepository<BookStoreDbContext, Book, Guid>, IBookRepository
    {
        public BookRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<int> CountAsync(Expression<Func<Book, bool>> predicate = null)
        {
            var dbSet = await GetDbSetAsync();
            return predicate == null
                ? await dbSet.CountAsync()
                : await dbSet.CountAsync(predicate);
        }

        public async Task<Book> FindByIDAsync(Guid id)
        {
            return await (await GetDbSetAsync()).FindAsync(id);
        }

        public async Task<Book> FindByNameAsync(string name)
        {
            var dbSet = await GetDbSetAsync(); // Đảm bảo gọi GetDbSetAsync ở đây
            return await dbSet.FirstOrDefaultAsync(book => book.Name == name);
        }

        public async Task<List<Book>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null)
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet.AsQueryable();

            // Áp dụng bộ lọc nếu có
            query = query.WhereIf(
                !string.IsNullOrWhiteSpace(filter),
                book => book.Name.Contains(filter)
            );

            // Sử dụng OrderBy từ System.Linq.Dynamic.Core để sắp xếp theo chuỗi
            query = query.OrderBy(sorting ?? nameof(Book.Name)); // Đặt mặc định là sắp xếp theo Name nếu không có sorting

            return await query
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await DbSet.FindAsync(id); // Lấy đối tượng Book
            await DeleteAsync(book); // Gọi phương thức xóa
        }

        public async Task<List<Book>> GetAllListAsync()
        {
            return await (await GetDbSetAsync()).ToListAsync();
        }


    }
}
