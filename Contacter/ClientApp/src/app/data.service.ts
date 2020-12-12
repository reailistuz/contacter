import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Contact } from './contact';
import { Observable } from 'rxjs';

@Injectable()
export class DataService {

    private url = "/api/contacts";
    public something;

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

    // public getJSON(): Observable<any> {
    //     this.something = this.http.get("./api/contacts");
    //     console.log(this.something);
    //     return this.something;
    // }

    downloadFile(): any {
        return this.http.get('https://localhost:44303/api/contacts', { responseType: 'blob' });
    }
}