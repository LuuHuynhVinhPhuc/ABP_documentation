using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Blob
{
    public class Document : FullAuditedAggregateRoot<Guid>
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }

        public string MimeType { get; set; }

        public Guid? TenantId { get; set; }

        public Document()
        {
        }

        public Document(
            Guid id,
            string fileName,
            long fileSize,
            string mimeType,
            Guid? tenantId
        ) : base(id)
        {
            FileName = fileName;
            FileSize = fileSize;
            MimeType = mimeType;
            TenantId = tenantId;
        }
    }
}
