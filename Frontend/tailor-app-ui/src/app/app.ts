import { Component } from '@angular/core';

import { CustomerForm } from './pages/customers/customer-form/customer-form';
import { CustomerList } from './pages/customers/customer-list/customer-list';

@Component({
  selector: 'app-root',
  imports: [
    CustomerForm,
    CustomerList
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
}