using Acme.BookStore.Blob;
using Acme.BookStore.FileAction;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.Blobs
{
    public class FileAppService : ApplicationService
    {
        private readonly IBlobContainer<FileContainer> _fileContainer;
        private readonly IRepository<Document, Guid> _repository;
        public FileAppService(IBlobContainer<FileContainer> blobContainer, IRepository<Document, Guid> repository)
        {
            _fileContainer = blobContainer;
            _repository = repository;
        }


        public async Task<List<DocumentDto>> UploadFileAsync([FromForm] List<IRemoteStreamContent> files)
        {
            var output = new List<DocumentDto>();
            foreach (var file in files)
            {
                using var memoryStream = new MemoryStream();
                await file.GetStream().CopyToAsync(memoryStream).ConfigureAwait(false);  // Lấy dữ liệu từ stream
                var id = Guid.NewGuid();
                var newFile = new Document(id, file.FileName, memoryStream.Length, file.ContentType, CurrentTenant.Id);
                var created = await _repository.InsertAsync(newFile);
                await _fileContainer.SaveAsync(id.ToString(), memoryStream.ToArray()).ConfigureAwait(false);
                output.Add(ObjectMapper.Map<Document, DocumentDto>(newFile));
            }

            return output;
        }


        public async Task<List<DocumentDto>> GetAllFilesAsync()
        {
            // Lấy tất cả các file từ repository
            var files = await _repository.GetListAsync();

            // Chuyển đổi dữ liệu sang FileDto
            return files.Select(file => new DocumentDto
            {
                Id = file.Id,
                FileName = file.FileName,
                MimeType = file.MimeType,
                FileSize = file.FileSize
            }).ToList();
        }

        public async Task<FileResult> DownloadFileAsync(Guid id)
        {
            // Tìm file trong cơ sở dữ liệu hoặc Blob Storage
            var currentFile = await _repository.FindAsync(id);
            if (currentFile == null)
            {
                throw new FileNotFoundException($"File with id {id} not found.");
            }

            // Lấy nội dung file từ Blob Storage (hoặc từ cơ sở dữ liệu)
            var fileContent = await _fileContainer.GetAllBytesOrNullAsync(id.ToString());

            // Trả về file dưới dạng FileContentResult
            return new FileContentResult(fileContent, currentFile.MimeType)
            {
                FileDownloadName = currentFile.FileName  // Tên file khi tải về
            };
        }

        public async Task<FileResult> DownloadFileByNameAsync(string fileName)
        {
            // Tìm file theo tên trong cơ sở dữ liệu hoặc Blob Storage
            var currentFile = await _repository.FirstOrDefaultAsync(f => f.FileName == fileName);
            if (currentFile == null)
            {
                throw new FileNotFoundException($"File with name {fileName} not found.");
            }

            // Lấy nội dung file từ Blob Storage (hoặc từ cơ sở dữ liệu)
            var fileContent = await _fileContainer.GetAllBytesOrNullAsync(currentFile.Id.ToString());

            // Trả về file dưới dạng FileContentResult
            return new FileContentResult(fileContent, currentFile.MimeType)
            {
                FileDownloadName = currentFile.FileName  // Tên file khi tải về
            };
        }

        // Delete file
        public async Task DeleteFileAsync(Guid id)
        {
            // Tìm file trong cơ sở dữ liệu
            var file = await _repository.FindAsync(x => x.Id == id);
            if (file == null)
            {
                throw new FileNotFoundException("File không tồn tại trong cơ sở dữ liệu.");
            }

            // Xóa nội dung file khỏi blob storage
            await _fileContainer.DeleteAsync(id.ToString());

            // Xóa metadata của file khỏi cơ sở dữ liệu
            await _repository.DeleteAsync(file);
        }

        // Helper method để lấy content type dựa trên phần mở rộng của file
        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".txt" => "text/plain",
                ".pdf" => "application/pdf",
                ".jpg" => "image/jpeg",
                ".png" => "image/png",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".zip" => "application/zip",
                _ => "application/octet-stream",
            };
        }
    }
}
