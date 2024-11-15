import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { BookRoutingModule } from './book-routing.module';
import { BookComponent } from './book.component';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { FileUploadComponent } from '../file-upload/file-upload.component';

@NgModule({
  declarations: [],
  imports: [
    BookComponent,
    BookRoutingModule,
    SharedModule,
    NgbDatepickerModule, // add this line

  ]
})
export class BookModule { }

