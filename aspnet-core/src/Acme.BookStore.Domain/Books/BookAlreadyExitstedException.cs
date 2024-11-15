using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Acme.BookStore.Books
{
    public class BookAlreadyExitstedException : BusinessException
    {
        public BookAlreadyExitstedException(string name) : base(BookStoreDomainErrorCodes.BookAlreadyExist)
        {
            WithData("name", name);
        }
    }
}
