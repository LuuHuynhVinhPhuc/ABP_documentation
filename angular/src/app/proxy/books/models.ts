import type { BookType } from './book-type.enum';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface BookDto {
  id?: string;
  name?: string;
  type: BookType;
  publishDate?: string;
  price: number;
}

export interface CreateBookDto {
  name: string;
  type: BookType;
  publishDate: string;
  price: number;
}

export interface GetBookListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface UpdateBookDto {
  id: string;
  name: string;
  type: BookType;
  publishDate: string;
  price: number;
}
