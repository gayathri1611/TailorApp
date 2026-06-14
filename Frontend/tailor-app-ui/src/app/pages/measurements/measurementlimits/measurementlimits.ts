import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { MeasurementLimitsService } from '../../../services/measurementlimits.service';

const DEFAULTS: Record<string, {min: number; max: number}> = {
  chest:         { min: 20,  max: 200 },
  shoulder:      { min: 10,  max: 100 },
  sleevelength:  { min: 10,  max: 150 },
  armhole:       { min: 10,  max: 100 },
  neck:          { min: 10,  max: 80  },
  waist:         { min: 20,  max: 200 },
  hip:           { min: 20,  max: 200 },
  thigh:         { min: 10,  max: 150 },
  knee:          { min: 10,  max: 120 },
  inseamlength:  { min: 10,  max: 200 },
  outseamlength: { min: 10,  max: 250 },
  height:        { min: 30,  max: 300 }
};

@Component({
  selector: 'app-measurement-limits',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './measurementlimits.html',
  styleUrls: ['./measurementlimits.css']
})
export class MeasurementLimits implements OnInit {
  form!: FormGroup;
  loading = false;
  saving  = false;
  error   = '';
  success = '';

  upperFields = [
    { key: 'chest',        label: 'Chest' },
    { key: 'shoulder',     label: 'Shoulder' },
    { key: 'sleevelength', label: 'Sleeve Length' },
    { key: 'armhole',      label: 'Arm Hole' },
    { key: 'neck',         label: 'Neck' },
    { key: 'waist',        label: 'Waist' },
  ];

  lowerFields = [
    { key: 'hip',          label: 'Hip' },
    { key: 'thigh',        label: 'Thigh' },
    { key: 'knee',         label: 'Knee' },
    { key: 'inseamlength', label: 'Inseam Length' },
    { key: 'outseamlength',label: 'Outseam Length' },
    { key: 'height',       label: 'Height' },
  ];

  get allFields() { return [...this.upperFields, ...this.lowerFields]; }

  constructor(
    private fb: FormBuilder,
    private limitsService: MeasurementLimitsService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadLimits();
  }

  buildForm(): void {
    const group: Record<string, FormGroup> = {};
    for (const f of this.allFields) {
      const d = DEFAULTS[f.key];
      group[f.key] = this.fb.group({
        min: [d.min, [Validators.required, Validators.min(0)]],
        max: [d.max, [Validators.required, Validators.min(0)]]
      }, { validators: this.maxGreaterThanMin });
    }
    this.form = this.fb.group(group);
  }

  maxGreaterThanMin(group: AbstractControl) {
    const min = group.get('min')?.value;
    const max = group.get('max')?.value;
    if (min != null && max != null && max <= min)
      return { maxNotGreater: true };
    return null;
  }

  loadLimits(): void {
    this.loading = true;
    this.limitsService.getLimits().subscribe({
      next: data => {
        for (const f of this.allFields) {
          const limit = data[f.key];
          if (limit) {
            this.form.get(f.key)?.patchValue({ min: limit.min, max: limit.max });
          }
        }
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  hasError(key: string): boolean {
    const g = this.form.get(key);
    return !!(g?.invalid && g?.touched) || !!(g?.errors?.['maxNotGreater']);
  }

  getError(key: string): string {
    const g = this.form.get(key);
    if (g?.errors?.['maxNotGreater']) return 'Max must be greater than min.';
    if (g?.get('min')?.errors?.['min']) return 'Min cannot be negative.';
    return 'Invalid value.';
  }

  saveAll(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.error = 'Fix errors before saving.';
      return;
    }
    this.saving  = true;
    this.error   = '';
    this.success = '';

    const saves = this.allFields.map(f => {
      const val = this.form.get(f.key)?.value;
      return this.limitsService.setLimit(f.key, val.min, val.max).toPromise();
    });

    Promise.all(saves)
      .then(() => {
        this.success = 'All limits saved successfully.';
        this.saving  = false;
        this.cdr.detectChanges();
      })
      .catch(() => {
        this.error  = 'Failed to save some limits.';
        this.saving = false;
        this.cdr.detectChanges();
      });
  }

  resetDefaults(): void {
    for (const f of this.allFields) {
      const d = DEFAULTS[f.key];
      this.form.get(f.key)?.patchValue({ min: d.min, max: d.max });
    }
    this.success = 'Defaults restored. Click Save to apply.';
    this.cdr.detectChanges();
  }
}