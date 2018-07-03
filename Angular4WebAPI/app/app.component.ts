
import { Component, OnInit } from '@angular/core';
import { FileService } from './services/file.service';

@Component({
  selector: 'my-app',
    templateUrl: 'app/app.component.html',
})
export class AppComponent implements OnInit {
        title = 'Image Gallery';
    errorMessage: string;
    images: Array<any> = [];
    constructor(private fileService: FileService) { }
    ngOnInit() {
        this.getImageData();
    }
    getImageData() {
        
        this.fileService.getImages().subscribe(
            data => {
                this.images = data
                console.log(data);
            },
            error => {
                this.errorMessage = error
                console.log(error);
            }
        )
    }
    refreshImages(status: any) {
        if (status == true) {
            console.log("Uploaded successfully!");
            setTimeout(this.getImageData(), 10000);            
        }
    }
}
