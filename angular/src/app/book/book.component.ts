import { CoreModule, ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { BookService, BookDto, bookTypeOptions } from '@proxy/books'; // add bookTypeOptions
import { FormGroup, FormBuilder, Validators } from '@angular/forms'; // add this
// added this line
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
// add new imports
import { ConfirmationService, Confirmation, ThemeSharedModule } from '@abp/ng.theme.shared';
import { FileUploadComponent } from '../file-upload/file-upload.component';
import { PageModule } from '@abp/ng.components/page';


@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  standalone: true,
  styleUrls: ['./book.component.scss'],
  providers: [
    ListService,
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter } // add this line
  ],
  imports: [
    CoreModule,
    ThemeSharedModule,
    PageModule,
    FileUploadComponent
  ]
})
export class BookComponent implements OnInit {
  book = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;

  selectedBook = {} as BookDto;

  form: FormGroup; // add this line

  // add bookTypes as a list of BookType enum members
  bookTypes = bookTypeOptions;

  isModalOpen = false;

  constructor(
    public readonly list: ListService,
    private bookService: BookService,
    private fb: FormBuilder, // inject FormBuilder
    private confirmation: ConfirmationService // inject the ConfirmationService
  ) { }

  ngOnInit() {
    const bookStreamCreator = (query) => this.bookService.getList(query);

    this.list.hookToQuery(bookStreamCreator).subscribe((response) => {
      this.book = response;
    });
  }

  createBook() {
    this.selectedBook = {} as BookDto; // reset the selected book
    this.buildForm();
    this.isModalOpen = true;
  }


  // Add editBook method
  // editBook(book: BookDto) {
  //   this.bookService.getID(book.id).subscribe((book) => {
  //     this.selectedBook = book;
  //     this.buildForm();
  //     this.form.patchValue({
  //       name: book.name,
  //       type: book.type,
  //       publishDate: book.publishDate,
  //       price: book.price,
  //     });
  //     this.isModalOpen = true;
  //   });
  // }
  editBook(id: string) {
    this.bookService.getID(id).subscribe((book) => {
      this.selectedBook = book;
      this.buildForm();
      this.form.patchValue({
        name: book.name,
        type: book.type,
        publishDate: book.publishDate,
        price: book.price,
      });
      this.isModalOpen = true;
    });
  }

  // add buildForm method
  buildForm() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      type: [null, Validators.required],
      publishDate: [null, Validators.required],
      price: [null, Validators.required],
    });
  }

  // change the save method
  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedBook.id
      ? this.bookService.update(this.selectedBook.id, this.form.value)
      : this.bookService.create(this.form.value);

    // request.subscribe(() => {
    //   this.isModalOpen = false;
    //   this.form.reset();
    //   this.list.get();
    // });
  };



  // Add a delete method
  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.bookService.delete(id).subscribe(() => {
          // Sau khi xóa thành công, gọi lại danh sách
          this.list.get(); // Giả sử phương thức này lấy lại danh sách sách
        });
      }
    });
  }
}
