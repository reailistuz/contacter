import { Contact } from './contact';
import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { Router } from '@angular/router';
import * as fileSaver from 'file-saver';
import *  as  data from './contacts.json';

@Component({
    templateUrl: './contact-list.component.html',
})
export class ContactListComponent implements OnInit {

    contacts: Contact[];
    fileToUpload: File = null;
    constructor(private dataService: DataService, private router: Router) { }

    ngOnInit() {
        this.load();
    }
    load() {
        this.dataService.getContacts().subscribe((data: Contact[]) => this.contacts = data);
    }
    delete(id: number) {
        this.dataService.deleteContact(id).subscribe(data => this.load());
    }

    uploadFile(file: FileList) {
        this.fileToUpload = file.item(0);
        this.dataService.uploadFile(this.fileToUpload).subscribe(data => {
            this.ngOnInit();
        }, error => {
            console.log(error);
        });
    }


    downloadFile() {
        this.dataService.downloadFile().subscribe(response => {
            let blob: any = new Blob([response], { type: 'text/json; charset=utf-8' });
            const url = window.URL.createObjectURL(blob);
            window.open(url);
            // window.location.href = response.url;
            fileSaver.saveAs(blob, 'contacts.json');
        }), error => console.log('Error downloading the file'),
            () => console.info('File downloaded successfully');
    }

}