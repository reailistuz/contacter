import { Contact } from './contact';
import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { Router } from '@angular/router';

@Component({
    templateUrl: './contact-list.component.html',
})
export class ContactListComponent implements OnInit {

    contacts: Contact[];
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

}