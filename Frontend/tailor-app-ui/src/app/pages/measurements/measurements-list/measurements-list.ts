import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MeasurementService } from '../../../services/measurements-service';
import { Measurement } from '../../../models/measurement';

@Component({
  selector: 'app-measurement-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './measurements-list.html',
  styleUrls: ['./measurements-list.css']
})
export class MeasurementList implements OnInit {
  measurements: Measurement[] = [];
  filtered: Measurement[] = [];
  searchTerm = '';
  loading = false;
  error = '';

  constructor(
    private measurementService: MeasurementService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.measurementService.getMeasurements().subscribe({
      next: data => {
        this.measurements = data;
        this.filtered = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Failed to load measurements.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  search(): void {
    const term = this.searchTerm.toLowerCase();
    this.filtered = this.measurements.filter(m =>
      m.customerName?.toLowerCase().includes(term) ||
      m.measurementCode?.toLowerCase().includes(term)
    );
  }

  delete(id: number): void {
    if (!confirm('Delete this measurement?')) return;
    this.measurementService.deleteMeasurement(id).subscribe({
      next: () => this.load(),
      error: () => (this.error = 'Delete failed.')
    });
  }
}