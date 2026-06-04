import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { OrderService } from '../../../services/order.service.js';
import { CustomerService } from '../../../services/customer.service.js';
import { Customer } from '../../../models/customer.js';
import { ORDER_STATUSES } from '../../../models/order.js';

@Component({
  selector: 'app-order-form',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './order-form.html',
  styleUrls: ['./order-form.css']
})
export class OrderForm implements OnInit {
  form!: FormGroup;
  customers: Customer[] = [];
  isEdit = false;
  orderId?: number;
  loading = false;
  saving = false;
  error = '';
  statuses = ORDER_STATUSES;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private orderService: OrderService,
    private customerService: CustomerService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadCustomers();

    this.orderId = this.route.snapshot.params['id']
      ? +this.route.snapshot.params['id']
      : undefined;
    this.isEdit = !!this.orderId && this.router.url.includes('edit');

    if (this.isEdit && this.orderId) {
      this.loading = true;
      this.orderService.getById(this.orderId).subscribe({
        next: o => {
          this.form.patchValue({
            customerId:   o.customerId,
            shopId:       o.shopId,
            deliveryDate: o.deliveryDate ? o.deliveryDate.substring(0, 10) : '',
            status:       o.status,
            notes:        o.notes
          });
          this.orderItemsArray.clear();
          o.orderItems.forEach(i => this.orderItemsArray.push(this.newItemGroup(i)));
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.error = 'Failed to load order.';
          this.loading = false;
          this.cdr.detectChanges();
        }
      });
    } else {
      this.addItem();
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      customerId:   [null, Validators.required],
      shopId:       [1],
      deliveryDate: [''],
      status:       ['Pending'],
      notes:        [''],
      orderItems:   this.fb.array([])
    });
  }

  get orderItemsArray(): FormArray {
    return this.form.get('orderItems') as FormArray;
  }

  newItemGroup(item?: any): FormGroup {
    return this.fb.group({
      garmentType:         [item?.garmentType ?? '', Validators.required],
      description:         [item?.description ?? ''],
      quantity:            [item?.quantity ?? 1, [Validators.required, Validators.min(1)]],
      unitPrice:           [item?.unitPrice ?? 0, [Validators.required, Validators.min(0)]],
      fabricDetails:       [item?.fabricDetails ?? ''],
      specialInstructions: [item?.specialInstructions ?? '']
    });
  }

  addItem(): void {
    this.orderItemsArray.push(this.newItemGroup());
  }

  removeItem(index: number): void {
    if (this.orderItemsArray.length > 1) this.orderItemsArray.removeAt(index);
  }

  getItemTotal(index: number): number {
    const item = this.orderItemsArray.at(index);
    return (item.get('quantity')?.value ?? 0) * (item.get('unitPrice')?.value ?? 0);
  }

  get grandTotal(): number {
    return this.orderItemsArray.controls.reduce((sum, _, i) => sum + this.getItemTotal(i), 0);
  }

  loadCustomers(): void {
    this.customerService.getCustomers().subscribe({
      next: data => { this.customers = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  submit(): void {
    if (this.form.invalid) return;
    this.saving = true;
    const payload = this.form.value;

    const call = this.isEdit && this.orderId
      ? this.orderService.update(this.orderId, payload)
      : this.orderService.create(payload);

    call.subscribe({
      next: () => this.router.navigate(['/orders']),
      error: () => {
        this.error = 'Save failed.';
        this.saving = false;
        this.cdr.detectChanges();
      }
    });
  }
}