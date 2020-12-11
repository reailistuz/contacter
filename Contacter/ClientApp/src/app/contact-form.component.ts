import { Contact } from './contact';
import { Component, Input } from '@angular/core';

@Component({
    selector: "contact-form",
    templateUrl: './contact-form.component.html'
})
export class ContactFormComponent {
    @Input() contact: Contact;
}