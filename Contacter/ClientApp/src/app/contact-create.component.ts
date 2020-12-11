import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from './data.service';
import { Contact } from './contact'

@Component({
    templateUrl: './contact-create.component.html'
})
export class ContactCreateComponent {

    contact: Contact = new Contact();    // добавляемый объект
    constructor(private dataService: DataService, private router: Router) { }
    save() {
        this.dataService.createContact(this.contact).subscribe(data => this.router.navigateByUrl("/"));
    }
}