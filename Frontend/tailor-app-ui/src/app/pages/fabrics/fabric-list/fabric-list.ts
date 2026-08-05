import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { FabricInventoryService } from '../../../services/fabric-inventory.service';
import { FabricInventory } from '../../../models/fabricinventory';
import { Paginator } from '../../../shared/paginator/paginator';
@Component({
  selector: 'app-fabric-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, Paginator],
  templateUrl: './fabric-list.html',
  styleUrls: ['./fabric-list.css']
})
export class FabricList implements OnInit {
  fabrics: FabricInventory[] = [];
  filtered: FabricInventory[] = [];
  searchTerm = '';
  loading = false;
  error = '';

  currentPage = 1;
  pageSize = 10;

  get paged(): FabricInventory[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filtered.slice(start, start + this.pageSize);
  }

  constructor(
    private fabricService: FabricInventoryService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.fabricService.getAll().subscribe({
      next: data => {
        this.fabrics = data;
        this.filtered = data;
        this.currentPage = 1;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Failed to load inventory.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  search(): void {
    const term = this.searchTerm.toLowerCase();
    this.filtered = this.fabrics.filter(f =>
      f.name.toLowerCase().includes(term) ||
      f.fabricCode?.toLowerCase().includes(term) ||
      f.fabricType?.toLowerCase().includes(term) ||
      f.color?.toLowerCase().includes(term)
    );
    this.currentPage = 1;
  }

  delete(id: number): void {
    if (!confirm('Delete this fabric?')) return;
    this.fabricService.delete(id).subscribe({
      next: () => this.load(),
      error: () => (this.error = 'Delete failed.')
    });
  }

  get lowStockCount(): number {
    return this.fabrics.filter(f => f.isLowStockMeters || f.isLowStockItems).length;
  }
}