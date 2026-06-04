import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FabricInventoryService } from '../../../services/fabric-inventory.service';

@Component({
  selector: 'app-fabric-form',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './fabric-form.html',
  styleUrls: ['./fabric-form.css']
})
export class FabricForm implements OnInit {
  form!: FormGroup;
  isEdit = false;
  fabricId?: number;
  loading = false;
  saving = false;
  error = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private fabricService: FabricInventoryService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.buildForm();

    this.fabricId = this.route.snapshot.params['id']
      ? +this.route.snapshot.params['id']
      : undefined;

    this.isEdit = !!this.fabricId && this.router.url.includes('edit');

    if (this.isEdit && this.fabricId) {
      this.loading = true;
      this.fabricService.getById(this.fabricId).subscribe({
        next: f => {
          this.form.patchValue(f);
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.error = 'Failed to load fabric.';
          this.loading = false;
          this.cdr.detectChanges();
        }
      });
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      name:                    ['', Validators.required],
      fabricType:              [''],
      color:                   [''],
      supplier:                [''],
      quantityInMeters:        [0, Validators.min(0)],
      quantityInItems:         [0, Validators.min(0)],
      lowStockThresholdMeters: [5, Validators.min(0)],
      lowStockThresholdItems:  [2, Validators.min(0)],
      pricePerMeter:           [0, Validators.min(0)],
      notes:                   ['']
    });
  }

  submit(): void {
    if (this.form.invalid) return;
    this.saving = true;
    const payload = this.form.value;

    const call = this.isEdit && this.fabricId
      ? this.fabricService.update(this.fabricId, payload)
      : this.fabricService.create(payload);

    call.subscribe({
      next: () => this.router.navigate(['/inventory']),
      error: () => {
        this.error = 'Save failed.';
        this.saving = false;
        this.cdr.detectChanges();
      }
    });
  }
}