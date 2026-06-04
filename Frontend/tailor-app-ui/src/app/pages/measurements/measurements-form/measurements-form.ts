import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { MeasurementService } from '../../../services/measurements-service';
import { CustomerService } from '../../../services/customer.service';
import { Customer } from '../../../models/customer';

@Component({
  selector: 'app-measurement-form',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './measurements-form.html',
  styleUrls: ['./measurements-form.css']
})
export class MeasurementForm implements OnInit {
  form!: FormGroup;
  customers: Customer[] = [];
  isEdit = false;
  measurementId?: number;
  loading = false;
  saving = false;
  error = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private measurementService: MeasurementService,
    private customerService: CustomerService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadCustomers();

    this.measurementId = this.route.snapshot.params['id']
      ? +this.route.snapshot.params['id']
      : undefined;

    this.isEdit = !!this.measurementId && this.router.url.includes('edit');

    if (this.isEdit && this.measurementId) {
      this.loading = true;
      this.measurementService.getMeasurement(this.measurementId).subscribe({
        next: m => {
          this.form.patchValue(m);
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.error = 'Failed to load measurement.';
          this.loading = false;
          this.cdr.detectChanges();
        }
      });
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      customerId:    [null],
      shopId:        [1],
      chest:         [null],
      shoulder:      [null],
      sleeveLength:  [null],
      armHole:       [null],
      neck:          [null],
      waist:         [null],
      hip:           [null],
      thigh:         [null],
      knee:          [null],
      inseamLength:  [null],
      outseamLength: [null],
      height:        [null],
      notes:         ['']
    });
  }

  loadCustomers(): void {
    this.customerService.getCustomers().subscribe({
      next: (data) => {
        this.customers = data;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  submit(): void {
    if (this.form.invalid) return;
    this.saving = true;

    const payload = this.form.value;

    if (this.isEdit && this.measurementId) {
      this.measurementService.updateMeasurement(this.measurementId, payload).subscribe({
        next: () => this.router.navigate(['/measurements']),
        error: () => {
          this.error = 'Save failed.';
          this.saving = false;
          this.cdr.detectChanges();
        }
      });
    } else {
      this.measurementService.createMeasurement(payload).subscribe({
        next: () => this.router.navigate(['/measurements']),
        error: () => {
          this.error = 'Save failed.';
          this.saving = false;
          this.cdr.detectChanges();
        }
      });
    }
  }
}