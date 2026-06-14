import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NameValueService } from '../../../services/namevalue.service';
import { NameValue } from '../../../models/namevalue';
@Component({
  selector: 'app-namevalue-manager',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './namevaluemanager.html',
  styleUrls: ['./namevaluemanager.css']
})
export class NameValueManager implements OnInit {

  all: NameValue[]      = [];
  filtered: NameValue[] = [];
  categories: string[]  = [];

  activeCategory = '';
  searchTerm     = '';
  loading        = false;
  saving         = false;
  error          = '';
  success        = '';

  adding    = false;
  editingId: number | null = null;

  newItem  = { category: '', value: '', label: '', sortOrder: 0 };
  editItem = { category: '', value: '', label: '', sortOrder: 0 };

  newErrors:  { category?: string; value?: string } = {};
  editErrors: { value?: string } = {};

  constructor(
    private nameValueService: NameValueService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading = true;
    this.nameValueService.getAll().subscribe({
      next: (data: NameValue[]) => {
        this.all        = data;
        this.categories = [...new Set(data.map(d => d.category))].sort();
        this.applyFilter();
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error   = 'Failed to load records.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  setCategory(cat: string): void {
    this.activeCategory = cat;
    this.applyFilter();
  }

  applyFilter(): void {
    const term = this.searchTerm.toLowerCase();
    this.filtered = this.all.filter(i => {
      const matchCat  = !this.activeCategory || i.category === this.activeCategory;
      const matchTerm = !term ||
        i.value.toLowerCase().includes(term) ||
        (i.label ?? '').toLowerCase().includes(term) ||
        i.category.toLowerCase().includes(term);
      return matchCat && matchTerm;
    });
    this.cdr.detectChanges();
  }

  // ── Duplicate check (case-insensitive) ──
  isDuplicate(category: string, value: string, excludeId?: number): boolean {
    return this.all.some(i =>
      i.category.trim().toLowerCase() === category.trim().toLowerCase() &&
      i.value.trim().toLowerCase()    === value.trim().toLowerCase() &&
      i.nameValueId !== excludeId
    );
  }

  // ── Add ──
  startAdd(): void {
    this.adding    = true;
    this.newErrors = {};
    this.newItem   = { category: this.activeCategory, value: '', label: '', sortOrder: 0 };
    this.error     = '';
    this.success   = '';
  }

  cancelAdd(): void {
    this.adding    = false;
    this.newErrors = {};
  }

  validateNew(): boolean {
    this.newErrors = {};
    if (!this.newItem.category.trim())
      this.newErrors.category = 'Category is required.';
    if (!this.newItem.value.trim())
      this.newErrors.value = 'Value is required.';
    else if (this.isDuplicate(this.newItem.category, this.newItem.value))
      this.newErrors.value = `"${this.newItem.value}" already exists in "${this.newItem.category}".`;
    return Object.keys(this.newErrors).length === 0;
  }

  saveNew(): void {
    if (!this.validateNew()) { this.cdr.detectChanges(); return; }
    this.saving = true;
    this.nameValueService.create(this.newItem).subscribe({
      next: () => {
        this.saving    = false;
        this.adding    = false;
        this.newErrors = {};
        this.success   = `"${this.newItem.value}" added to "${this.newItem.category}".`;
        this.load();
      },
      error: (err) => {
        this.error  = err?.error ?? 'Failed to add record.';
        this.saving = false;
        this.cdr.detectChanges();
      }
    });
  }

  // ── Edit ──
  startEdit(item: NameValue): void {
    this.editingId  = item.nameValueId;
    this.editErrors = {};
    this.editItem   = { category: item.category, value: item.value, label: item.label ?? '', sortOrder: item.sortOrder };
    this.error      = '';
    this.success    = '';
  }

  cancelEdit(): void {
    this.editingId  = null;
    this.editErrors = {};
  }

  validateEdit(id: number): boolean {
    this.editErrors = {};
    if (!this.editItem.value.trim())
      this.editErrors.value = 'Value is required.';
    else if (this.isDuplicate(this.editItem.category, this.editItem.value, id))
      this.editErrors.value = `"${this.editItem.value}" already exists in "${this.editItem.category}".`;
    return Object.keys(this.editErrors).length === 0;
  }

  saveEdit(id: number): void {
    if (!this.validateEdit(id)) { this.cdr.detectChanges(); return; }
    this.saving = true;
    this.nameValueService.update(id, this.editItem).subscribe({
      next: () => {
        this.saving     = false;
        this.editingId  = null;
        this.editErrors = {};
        this.success    = 'Record updated.';
        this.load();
      },
      error: (err) => {
        this.error  = err?.error ?? 'Failed to update record.';
        this.saving = false;
        this.cdr.detectChanges();
      }
    });
  }

  // ── Delete ──
  delete(id: number, value: string): void {
    if (!confirm(`Delete "${value}"? This may affect existing dropdowns.`)) return;
    this.nameValueService.delete(id).subscribe({
      next: () => { this.success = `"${value}" deleted.`; this.load(); },
      error: () => { this.error = 'Delete failed.'; this.cdr.detectChanges(); }
    });
  }
}