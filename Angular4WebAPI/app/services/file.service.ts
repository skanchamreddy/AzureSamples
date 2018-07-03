import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class FileService {
    _baseURL: string = '/api/fileupload/'
    constructor(private http: Http) { }
    upload(files:any, parameters:any) {
        let headers = new Headers();
        let options = new RequestOptions({ headers: headers });
        options.params = parameters;
        return this.http.post(this._baseURL + 'upload', files, options)
            .map(responce => responce.json() )
            .catch(exception => Observable.throw(exception));      
    }
    getImages() {
        return this.http.get(this._baseURL + "getimages").map(responce => responce.json()).catch(exeption => Observable.throw(exeption));
    }
}