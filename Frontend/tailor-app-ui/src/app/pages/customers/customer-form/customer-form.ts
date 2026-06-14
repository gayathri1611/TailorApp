import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CustomerService } from '../../../services/customer.service';
import { Customer } from '../../../models/customer';

@Component({
  selector: 'app-customer-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './customer-form.html',
  styleUrl: './customer-form.css'
})
export class CustomerForm implements OnInit {

  customer: Customer = {
    shopId: 1,
    firstName: '',
    lastName: '',
    phoneNumber: '',
    email: '',
    address: ''
  };

  isEdit    = false;
  customerId?: number;
  loading   = false;
  saving    = false;
  submitted = false;
  error     = '';
  successMsg = '';
  private allCustomers: Customer[] = [];

  constructor(
    private customerService: CustomerService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.params['id'];
    this.customerId = idParam ? +idParam : undefined;
    this.isEdit = !!this.customerId && this.router.url.includes('edit');

    this.customerService.getCustomers().subscribe({
      next: (list) => { this.allCustomers = list; }
    });

    if (this.isEdit && this.customerId) {
      this.loading = true;
      this.customerService.getCustomer(this.customerId).subscribe({
        next: (c) => {
          this.customer = c;
          this.loading  = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.error   = 'Failed to load customer details. Please try again.';
          this.loading = false;
          this.cdr.detectChanges();
        }
      });
    }
  }

  // Allow only digits in phone field
  onPhoneInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    input.value = input.value.replace(/\D/g, '').slice(0, 10);
    this.customer.phoneNumber = input.value;
  }

  // Validate before save
  isValid(): boolean {
    if (!this.customer.firstName?.trim()) {
      this.error = 'First name is required.';
      return false;
    }
    if (!this.customer.phoneNumber?.trim()) {
      this.error = 'Phone number is required.';
      return false;
    }
    if (!/^[0-9]{10}$/.test(this.customer.phoneNumber)) {
      this.error = 'Phone number must be exactly 10 digits.';
      return false;
    }
    if (this.customer.email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(this.customer.email)) {
      this.error = 'Please enter a valid email address.';
      return false;
    }

    // Duplicate checks against existing customers (exclude self in edit mode)
    const others = this.allCustomers.filter(c => c.customerId !== this.customerId);

    const dupPhone = others.find(c =>
      c.phoneNumber === this.customer.phoneNumber
    );
    if (dupPhone) {
      this.error = `Phone number already used by ${dupPhone.firstName} ${dupPhone.lastName ?? ''}.`.trim() + '.';
      return false;
    }

    if (this.customer.email?.trim()) {
      const dupEmail = others.find(c =>
        c.email?.toLowerCase() === this.customer.email!.toLowerCase()
      );
      if (dupEmail) {
        this.error = `Email already used by ${dupEmail.firstName} ${dupEmail.lastName ?? ''}.`.trim() + '.';
        return false;
      }
    }

    const dupName = others.find(c =>
      c.firstName.trim().toLowerCase() === this.customer.firstName.trim().toLowerCase() &&
      (c.lastName ?? '').trim().toLowerCase() === (this.customer.lastName ?? '').trim().toLowerCase()
    );
    if (dupName) {
      this.error = `A customer named "${(this.customer.firstName + ' ' + (this.customer.lastName ?? '')).trim()}" already exists.`;
      return false;
    }

    return true;
  }

  saveCustomer(): void {
    this.submitted = true;
    this.error     = '';
    this.successMsg = '';

    if (!this.isValid()) {
      this.cdr.detectChanges();
      return;
    }

    this.saving = true;

    const call = this.isEdit && this.customerId
      ? this.customerService.updateCustomer(this.customerId, this.customer)
      : this.customerService.addCustomer(this.customer);

    call.subscribe({
      next: () => this.router.navigate(['/customers']),
      error: (err) => {
        // Show specific error from API if available
        const apiError = err?.error;
        if (typeof apiError === 'string') {
          this.error = apiError;
        } else if (apiError?.errors) {
          const messages = Object.values(apiError.errors).flat() as string[];
          this.error = messages[0];
        } else if (err.status === 409) {
          this.error = 'A customer with this phone number already exists.';
        } else if (err.status === 0) {
          this.error = 'Cannot connect to server. Please check your connection.';
        } else {
          this.error = `Save failed (Error ${err.status}). Please try again.`;
        }
        this.saving = false;
        this.cdr.detectChanges();
      }
    });
  }
}