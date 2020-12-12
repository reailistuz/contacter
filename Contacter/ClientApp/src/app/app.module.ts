
import { NgModule, Component } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ContactFormComponent } from './contact-form.component';
import { DataService } from './data.service';
import { NotFoundComponent } from './not-found.component';
import { ContactEditComponent } from './contact-edit.component';
import { ContactCreateComponent } from './contact-create.component';
import { ContactListComponent } from './contact-list.component';

const appRoutes: Routes = [
    { path: '', component: ContactListComponent },
    { path: 'edit/:id', component: ContactEditComponent },
    { path: 'create', component: ContactCreateComponent },
    { path: '**', component: NotFoundComponent },
];

@NgModule({
    imports: [BrowserModule, FormsModule, HttpClientModule, RouterModule.forRoot(appRoutes)],
    declarations: [AppComponent, ContactListComponent, ContactCreateComponent, ContactEditComponent, NotFoundComponent, ContactFormComponent],
    providers: [DataService],
    bootstrap: [AppComponent]
})
export class AppModule { }