import { CommonModule } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FileService } from '@proxy/blobs';
import { DocumentDto } from '@proxy/file-action';

@Component({
  selector: 'app-file-upload',
  imports: [CommonModule, FormsModule],
  templateUrl: './file-upload.component.html',
  standalone: true,
  styleUrl: './file-upload.component.scss'
})
export class FileUploadComponent {
  selectedFiles: File[] = [];  // Lưu trữ tất cả các tệp đã chọn
  uploadedFiles: DocumentDto[] = []; // Mảng tệp đã tải lên
  isUploading = false;
  uploadSuccess = false;
  uploadError = false;

  // download parameter
  fileName: string = ''; // Tên file được nhập
  isDownloading: boolean = false; // Trạng thái tải xuống
  downloadError: string | null = null; // Lỗi khi tải file

  constructor(private fileService: FileService, private http: HttpClient) { }

  // when user choose files
  onFilesSelected(event: any): void {
    const files = Array.from(event.target.files) as File[];  // Chuyển đổi các tệp thành mảng
    this.selectedFiles = [...this.selectedFiles, ...files];  // Thêm các tệp mới vào danh sách đã có
  }

  // Hàm xử lý sự kiện dragover
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    event.dataTransfer!.dropEffect = 'copy';  // Chỉ cho phép copy
  }

  // Hàm xử lý sự kiện dragleave
  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }

  // Hàm xử lý sự kiện drop
  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();

    const files = event.dataTransfer?.files;  // Lấy tệp được kéo thả
    if (files) {
      this.selectedFiles = [...this.selectedFiles, ...Array.from(files)];
    }
  }

  // Hàm tải danh sách tệp từ server
  loadUploadedFiles(): void {
    this.fileService.getAllFiles().subscribe({
      next: (files: DocumentDto[]) => {
        this.uploadedFiles = files;  // Lưu danh sách tệp vào mảng uploadedFiles
      },
      error: (err) => {
        console.error('Error fetching files', err);
      }
    });
  }

  // upload files
  uploadFiles(): void {
    if (this.selectedFiles.length === 0) {
      return; // Không có file để upload
    }

    this.isUploading = true;
    this.uploadSuccess = false;
    this.uploadError = false;

    this.fileService.uploadFile(this.selectedFiles).subscribe(
      (response) => {
        this.isUploading = false;
        this.uploadSuccess = true;
        console.log('Files uploaded successfully', response);
      },
      (error) => {
        this.isUploading = false;
        this.uploadError = true;
        console.error('Error uploading files', error);
      }
    );
  }

  // download file with name
  downloadFile() {
    if (!this.fileName) {
      this.downloadError = "File name is required!";
      return;
    }

    const params = new HttpParams().set('fileName', this.fileName);

    this.http.post('https://localhost:44376/api/app/file/download-file-by-name', null, {
      params: params,
      responseType: 'blob', // Để xử lý file dưới dạng blob
    }).subscribe({
      next: (response: Blob) => {
        const downloadUrl = window.URL.createObjectURL(response);
        const a = document.createElement('a');
        a.href = downloadUrl;
        a.download = this.fileName; // Đặt tên file khi tải về
        a.click();
        window.URL.revokeObjectURL(downloadUrl);
      },
      error: (error) => {
        console.error(error);
        this.downloadError = 'An error occurred while downloading the file.';
      }
    });
  }


  // Hàm tạo ID ngẫu nhiên cho tệp (giả lập)
  generateFileId(): string {
    return Math.random().toString(36).substr(2, 9); // Tạo ID ngẫu nhiên
  }
  // Hàm xóa tệp
  removeFile(file: File): void {
    this.selectedFiles = this.selectedFiles.filter(f => f !== file);  // Xóa tệp khỏi danh sách
  }
}
