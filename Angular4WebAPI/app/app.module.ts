import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';

import { FileUploadComponent } from './file-upload/file-upload.component';
import { FileService } from './services/file.service';

@NgModule({
    imports: [BrowserModule, HttpModule ],
    declarations: [AppComponent, FileUploadComponent ],
    bootstrap: [AppComponent],
    providers: [FileService],
})
export class AppModule { }
