import { EventEmitter, Injectable, Output } from '@angular/core';
import { HttpClient, HttpEventType, HttpResponse } from '@angular/common/http';
import { Contact } from './contact';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class DataService {

    private url = "/api/contacts";
    public progress: number;
    public message: string;

    constructor(private http: HttpClient) {
    }

    getContacts() {
        return this.http.get(this.url);
    }

    getContact(id: number) {
        return this.http.get(this.url + '/' + id);
    }

    createContact(contact: Contact) {
        return this.http.post(this.url, contact, { observe: 'response' });
    }

    updateContact(contact: Contact) {

        return this.http.put(this.url, contact);
    }
    deleteContact(id: number) {
        return this.http.delete(this.url + '/' + id);
    }

    @Output() public onUploadFinished = new EventEmitter();

    uploadFile(fileToUpload: File): Observable<boolean> {
        const formData: FormData = new FormData();
        formData.append('fileKey', fileToUpload, fileToUpload.name);
        return this.http
            .post('https://localhost:44303/api/uploadFile', formData, { reportProgress: true, observe: 'events' })
            .pipe(map(() => { return true; }));
    }


    downloadFile(): any {
        return this.http.get('https://localhost:44303/api/contacts', { responseType: 'blob' });
    }
}