import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-paginator',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './paginator.html',
  styleUrls: ['./paginator.css']
})
export class Paginator implements OnChanges {
  @Input() total = 0;
  @Input() pageSize = 10;
  @Input() page = 1;
  @Output() pageChange = new EventEmitter<number>();

  totalPages = 0;
  pages: (number | null)[] = [];

  ngOnChanges(): void {
    this.totalPages = Math.ceil(this.total / this.pageSize) || 1;
    this.pages = this.buildPages();
  }

  get startItem(): number { return Math.min((this.page - 1) * this.pageSize + 1, this.total); }
  get endItem(): number   { return Math.min(this.page * this.pageSize, this.total); }

  go(p: number): void {
    if (p < 1 || p > this.totalPages || p === this.page) return;
    this.pageChange.emit(p);
  }

  private buildPages(): (number | null)[] {
    const total = this.totalPages;
    const p = this.page;
    if (total <= 7) {
      return Array.from({ length: total }, (_, i) => i + 1);
    }
    const result: (number | null)[] = [1];
    if (p > 3) result.push(null);
    const start = Math.max(2, p - 1);
    const end   = Math.min(total - 1, p + 1);
    for (let i = start; i <= end; i++) result.push(i);
    if (p < total - 2) result.push(null);
    result.push(total);
    return result;
  }
}
