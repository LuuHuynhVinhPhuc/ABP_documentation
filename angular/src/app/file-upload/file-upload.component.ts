import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FileService } from '@proxy/blobs';
import { DocumentDto } from '@proxy/file-action';

@Component({
  selector: 'app-file-upload',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.scss'
})
export class FileUploadComponent {
  selectedFiles: File[] = [];  // Lưu trữ tất cả các tệp đã chọn
  uploadedFiles: DocumentDto[] = []; // Mảng tệp đã tải lên
  isUploading = false;
  uploadSuccess = false;
  uploadError = false;

  constructor(private fileService: FileService) { }

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
  downloadFile(fileId: string): void {
    // Thực hiện tải xuống theo ID tệp
    console.log('Downloading file with ID:', fileId);
    // Ở đây bạn có thể gọi API để tải tệp từ backend theo ID
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
