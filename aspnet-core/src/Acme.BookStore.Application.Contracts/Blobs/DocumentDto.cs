﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.FileAction
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }

        public long FileSize { get; set; }

        public string FileUrl { get; set; }

        public string MimeType { get; set; }
    }
}
