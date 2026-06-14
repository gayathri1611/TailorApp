import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { MeasurementService } from '../../../services/measurements-service';
import { CustomerService } from '../../../services/customer.service';
import { NameValueService } from '../../../services/namevalue.service';
import { Customer } from '../../../models/customer';
import { NameValue } from '../../../models/namevalue';
import { MeasurementLimitsService, LimitsMap } from '../../../services/measurementlimits.service';

@Component({
  selector: 'app-measurement-form',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './measurements-form.html',
  styleUrls: ['./measurements-form.css']
})
export class MeasurementForm implements OnInit, OnDestroy {

  form!: FormGroup;
  customers: Customer[] = [];
  units: NameValue[]    = [];
  isEdit      = false;
  measurementId?: number;
  loading     = false;
  saving      = false;
  submitted   = false;
  error       = '';
  limits: LimitsMap = {};

  private errorTimer: ReturnType<typeof setTimeout> | null = null;

  ngOnDestroy(): void {
    if (this.errorTimer) clearTimeout(this.errorTimer);
  }

  showError(msg: string, duration = 6000): void {
    this.error = msg;
    if (this.errorTimer) clearTimeout(this.errorTimer);
    this.errorTimer = setTimeout(() => {
      this.error = '';
      this.cdr.detectChanges();
    }, duration);
    this.cdr.detectChanges();
  }

  dismissError(): void {
    this.error = '';
    if (this.errorTimer) clearTimeout(this.errorTimer);
  }

  upperFields = [
    { key: 'chest',        label: 'Chest',        placeholder: '36' },
    { key: 'shoulder',     label: 'Shoulder',     placeholder: '16' },
    { key: 'sleeveLength', label: 'Sleeve Length', placeholder: '24' },
    { key: 'armHole',      label: 'Arm Hole',      placeholder: '18' },
    { key: 'neck',         label: 'Neck',          placeholder: '15' },
    { key: 'waist',        label: 'Waist',         placeholder: '32' },
  ];

  lowerFields = [
    { key: 'hip',          label: 'Hip',           placeholder: '38' },
    { key: 'thigh',        label: 'Thigh',         placeholder: '24' },
    { key: 'knee',         label: 'Knee',          placeholder: '18' },
    { key: 'inseamLength', label: 'Inseam Length', placeholder: '30' },
    { key: 'outseamLength',label: 'Outseam Length',placeholder: '40' },
    { key: 'height',       label: 'Height',        placeholder: '65' },
  ];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private measurementService: MeasurementService,
    private customerService: CustomerService,
    private nameValueService: NameValueService,
    private cdr: ChangeDetectorRef,
    private limitsService: MeasurementLimitsService
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadCustomers();
    this.loadUnits();
    this.loadLimits();

    // ── Auto-select customer from order screen query param ──
    const customerIdParam = this.route.snapshot.queryParams['customerId'];
    if (customerIdParam) {
      this.form.get('customerId')?.setValue(+customerIdParam);
    }

    const id = this.route.snapshot.params['id'];
    this.isEdit        = !!id && this.router.url.includes('edit');
    this.measurementId = id ? +id : undefined;

    if (this.isEdit && this.measurementId) {
      this.loading = true;
      this.measurementService.getMeasurement(this.measurementId).subscribe({
        next: m => {
          this.form.patchValue(m);
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.showError('Failed to load measurement.');
          this.loading = false;
          this.cdr.detectChanges();
        }
      });
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      customerId:    [null, Validators.required],
      unit:          ['inch'],
      shopId:        [1],
      chest:         [null, [this.limitValidator('chest')]],
      shoulder:      [null, [this.limitValidator('shoulder')]],
      sleeveLength:  [null, [this.limitValidator('sleevelength')]],
      armHole:       [null, [this.limitValidator('armhole')]],
      neck:          [null, [this.limitValidator('neck')]],
      waist:         [null, [this.limitValidator('waist')]],
      hip:           [null, [this.limitValidator('hip')]],
      thigh:         [null, [this.limitValidator('thigh')]],
      knee:          [null, [this.limitValidator('knee')]],
      inseamLength:  [null, [this.limitValidator('inseamlength')]],
      outseamLength: [null, [this.limitValidator('outseamlength')]],
      height:        [null, [this.limitValidator('height')]],
      notes:         ['']
    });
  }

  get selectedUnit(): string {
    return this.form.get('unit')?.value ?? 'inch';
  }

  loadCustomers(): void {
    this.customerService.getCustomers().subscribe({
      next: data => { this.customers = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  loadUnits(): void {
    this.nameValueService.getByCategory('Unit').subscribe({
      next: data => {
        this.units = data;
        if (data.length > 0 && !this.form.get('unit')?.value) {
          this.form.get('unit')?.setValue(data[0].value);
        }
        this.cdr.detectChanges();
      },
      error: () => {
        // Fallback if API fails
        this.units = [
          { nameValueId: 1, category: 'Unit', value: 'inch', label: 'Inch (")',  sortOrder: 1 },
          { nameValueId: 2, category: 'Unit', value: 'cm',   label: 'Centimeter (cm)', sortOrder: 2 }
        ];
      }
    });
  }

  submit(): void {
    this.submitted = true;
    this.form.markAllAsTouched();

    if (this.form.invalid) {
      const msgs: string[] = [];
      if (this.form.get('customerId')?.invalid) {
        msgs.push('Please select a customer');
      }
      const allFields = [...this.upperFields, ...this.lowerFields];
      allFields.forEach(f => {
        const ctrl = this.form.get(f.key);
        if (ctrl?.errors?.['belowMin']) {
          msgs.push(`${f.label} must be at least ${ctrl.errors['belowMin'].min} ${this.selectedUnit}`);
        } else if (ctrl?.errors?.['aboveMax']) {
          msgs.push(`${f.label} must not exceed ${ctrl.errors['aboveMax'].max} ${this.selectedUnit}`);
        } else if (ctrl?.errors?.['negative']) {
          msgs.push(`${f.label} cannot be negative`);
        }
      });
      const msg = msgs.length ? msgs.join('. ') + '.' : 'Please fix the errors before saving.';
      this.showError(msg);
      return;
    }

    this.saving = true;

    const call = this.isEdit && this.measurementId
      ? this.measurementService.updateMeasurement(this.measurementId, this.form.value)
      : this.measurementService.createMeasurement(this.form.value);

    call.subscribe({
      next: () => this.router.navigate(['/measurements']),
      error: (err) => {
        if (err.status === 0) {
          this.showError('Cannot connect to server. Check your connection.');
        } else if (err.status === 400) {
          this.showError('Invalid data. Please check all fields.');
        } else {
          this.showError(`Save failed (Error ${err.status}). Please try again.`);
        }
        this.saving = false;
      }
    });
  }

loadLimits(): void {
  this.limitsService.getLimits().subscribe({
    next: data => {
      this.limits = data;
      this.form.updateValueAndValidity(); // ← re-run all validators with new limits
      this.cdr.detectChanges();
    },
    error: () => {}
  });
}

limitValidator(fieldKey: string): ValidatorFn {
  return (control: AbstractControl) => {
    const val = control.value;
    if (val == null || val === '') return null;

    // Always read from this.limits at validation time — not at build time
    const limit = this.limits[fieldKey.toLowerCase()];
    const min = limit?.min ?? 0;
    const max = limit?.max ?? 9999;

    if (val < 0)   return { negative: true };
    if (val < min) return { belowMin: { min } };
    if (val > max) return { aboveMax: { max } };
    return null;
  };
}
}