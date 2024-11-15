import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DocumentDto } from '../file-action/models';
import type { FileResult } from '../microsoft/asp-net-core/mvc/models';

@Injectable({
  providedIn: 'root',
})
export class FileService {
  apiName = 'Default';


  deleteFile = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/file/${id}/file`,
    },
      { apiName: this.apiName, ...config });


  downloadFile = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileResult>({
      method: 'POST',
      url: `/api/app/file/${id}/download-file`,
    },
      { apiName: this.apiName, ...config });


  downloadFileByName = (fileName: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileResult>({
      method: 'POST',
      url: '/api/app/file/download-file-by-name',
      params: { fileName },
      responseType: 'blob'
    },
      { apiName: this.apiName, ...config });


  getAllFiles = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DocumentDto[]>({
      method: 'GET',
      url: '/api/app/file/files',
    },
      { apiName: this.apiName, ...config });


  uploadFile = (files: File[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, DocumentDto[]>({
      method: 'POST',
      url: '/api/app/file/upload-file',
      body: this.createFormData(files), // Sử dụng FormData
    },
      { apiName: this.apiName, ...config });

  // Tạo FormData từ danh sách file
  private createFormData(files: File[]): FormData {
    const formData = new FormData();
    files.forEach(file => formData.append('files', file, file.name));
    return formData;
  }

  constructor(private restService: RestService) { }
}
