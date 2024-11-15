using Acme.BookStore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;
using Volo.Abp.AspNetCore.Mvc;
using Acme.BookStore.Books;

namespace Acme.BookStore.Controllers;

/* Inherit your controllers from this class.
 */
[ApiController]
[Route("api/[controller]")]
public class BookStoreController : AbpControllerBase
{
   
}
