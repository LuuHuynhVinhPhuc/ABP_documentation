using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.FileAction
{
    public class CreateBlobInputDto
    {
        public byte[] Content { get; set; }

        [Required]
        public string FileName { get; set; }
    }
}
