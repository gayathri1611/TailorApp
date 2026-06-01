import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CustomerService } from '../../../services/customer.service';
import { Customer } from '../../../models/customer';

@Component({
  selector: 'app-customer-form',
  imports: [FormsModule],
  templateUrl: './customer-form.html',
  styleUrl: './customer-form.css',
})
export class CustomerForm {

  customer: Customer = {
    shopId: 1,
    firstName: '',
    lastName: '',
    phoneNumber: '',
    email: '',
    address: ''
  };

  constructor(private customerService: CustomerService) {}

  saveCustomer() {
    this.customerService.addCustomer(this.customer).subscribe({
      next: (response) => {
        alert('Customer saved successfully');
        console.log(response);

        this.customer = {
          shopId: 1,
          firstName: '',
          lastName: '',
          phoneNumber: '',
          email: '',
          address: ''
        };
      },
      error: (error) => {
        console.error(error);
        alert('Error while saving customer');
      }
    });
  }
}