import type { BookDto, CreateBookDto, GetBookListDto, UpdateBookDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  apiName = 'Default';
  

  create = (input: CreateBookDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookDto>({
      method: 'POST',
      url: '/api/app/book',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/book/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getBookByName = (name: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookDto>({
      method: 'GET',
      url: '/api/app/book/book-by-name',
      params: { name },
    },
    { apiName: this.apiName,...config });
  

  getID = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookDto>({
      method: 'GET',
      url: `/api/app/book/${id}/i-d`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetBookListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<BookDto>>({
      method: 'GET',
      url: '/api/app/book',
      params: { filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateBookDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/book/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
