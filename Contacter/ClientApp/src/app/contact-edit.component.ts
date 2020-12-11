import { Contact } from './contact';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DataService } from './data.service';

@Component({
    templateUrl: './contact-edit.component.html'
})
export class ContactEditComponent implements OnInit {

    id: number;
    contact: Contact;    // изменяемый объект
    loaded: boolean = false;

    constructor(private dataService: DataService, private router: Router, activeRoute: ActivatedRoute) {
        this.id = Number.parseInt(activeRoute.snapshot.params["id"]);
    }

    ngOnInit() {
        if (this.id)
            this.dataService.getContact(this.id)
                .subscribe((data: Contact) => {
                    this.contact = data;
                    if (this.contact != null) this.loaded = true;
                });
    }

    save() {
        this.dataService.updateContact(this.contact).subscribe(data => this.router.navigateByUrl("/"));
    }
}