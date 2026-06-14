import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { OrderService } from '../../../services/order.service';
import { CustomerService } from '../../../services/customer.service';
import { MeasurementService } from '../../../services/measurements-service';
import { FabricInventoryService } from '../../../services/fabric-inventory.service';
import { Customer } from '../../../models/customer';
import { Measurement } from '../../../models/measurement';
import { FabricInventory } from '../../../models/fabricinventory';
import { ORDER_STATUSES } from '../../../models/order';

interface GarmentPreset { name: string; defaultPrice: number; }

const GARMENT_PRESETS: GarmentPreset[] = [
  { name: 'Shirt',       defaultPrice: 400 },
  { name: 'Pant',        defaultPrice: 450 },
  { name: 'Blouse',      defaultPrice: 350 },
  { name: 'Saree Fall',  defaultPrice: 150 },
  { name: 'Churidar',    defaultPrice: 500 },
  { name: 'Kurti',       defaultPrice: 450 },
  { name: 'Suit',        defaultPrice: 1200 },
  { name: 'Coat',        defaultPrice: 1500 },
  { name: 'Frock',       defaultPrice: 500 },
  { name: 'Lehenga',     defaultPrice: 1800 },
  { name: 'Salwar',      defaultPrice: 400 },
  { name: 'Jacket',      defaultPrice: 900 },
];

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
  customerMeasurements: Measurement[] = [];
  fabrics: FabricInventory[] = [];
  isEdit = false;
  orderId?: number;
  loading = false;
  saving = false;
  submitted = false;
  error = '';
  showMeasurements = false;
  statuses = ORDER_STATUSES;
  today = new Date().toISOString().split('T')[0];

  // Custom dropdown state
  customerDropOpen = false;
  statusDropOpen   = false;
  measDropOpen     = false;
  garmentDropOpen: boolean[] = [];
  fabricDropOpen: boolean[]  = [];
  filteredGarments: GarmentPreset[][] = [];

  selectedCustomerLabel = 'Select customer';
  selectedMeasLabel     = '— None —';

  measurementFields = [
    { key: 'chest',        label: 'Chest',    placeholder: '36' },
    { key: 'shoulder',     label: 'Shoulder', placeholder: '16' },
    { key: 'sleeveLength', label: 'Sleeve',   placeholder: '24' },
    { key: 'waist',        label: 'Waist',    placeholder: '32' },
    { key: 'hip',          label: 'Hip',      placeholder: '38' },
    { key: 'neck',         label: 'Neck',     placeholder: '15' },
    { key: 'inseamLength', label: 'Inseam',   placeholder: '30' },
    { key: 'height',       label: 'Height',   placeholder: '65' },
  ];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private orderService: OrderService,
    private customerService: CustomerService,
    private measurementService: MeasurementService,
    private fabricService: FabricInventoryService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadCustomers();
    this.loadFabrics();

    this.orderId = this.route.snapshot.params['id']
      ? +this.route.snapshot.params['id'] : undefined;
    this.isEdit = !!this.orderId && this.router.url.includes('edit');

    if (this.isEdit && this.orderId) {
      this.loading = true;
      this.orderService.getById(this.orderId).subscribe({
        next: o => {
          this.form.patchValue({
            customerId:    o.customerId,
            shopId:        o.shopId,
            deliveryDate:  o.deliveryDate ? o.deliveryDate.substring(0, 10) : '',
            status:        o.status,
            notes:         o.notes,
            measurementId: o.measurementId ?? null,
            discount:      0
          });
          this.orderItemsArray.clear();
          o.orderItems.forEach(i => {
            this.orderItemsArray.push(this.newItemGroup(i));
            this.garmentDropOpen.push(false);
            this.fabricDropOpen.push(false);
            this.filteredGarments.push([...GARMENT_PRESETS]);
          });
          const cust = this.customers.find(c => c.customerId === o.customerId);
          if (cust) this.selectedCustomerLabel =
            `${cust.firstName} ${cust.lastName} (${cust.customerCode})`;
          this.loadMeasurementsForCustomer(o.customerId);
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: () => { this.error = 'Failed to load order.'; this.loading = false; this.cdr.detectChanges(); }
      });
    } else {
      this.addItem();
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      customerId:    [null, Validators.required],
      shopId:        [1],
      deliveryDate:  [''],
      status:        ['Pending'],
      notes:         [''],
      measurementId: [null],
      discount:      [0],
      quickMeasurements: this.fb.group({
        chest: [null], shoulder: [null], sleeveLength: [null],
        waist: [null], hip: [null], neck: [null],
        inseamLength: [null], height: [null]
      }),
      orderItems: this.fb.array([])
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
      fabricId:            [item?.fabricId ?? null],
      specialInstructions: [item?.specialInstructions ?? '']
    });
  }

  addItem(): void {
    this.orderItemsArray.push(this.newItemGroup());
    this.garmentDropOpen.push(false);
    this.fabricDropOpen.push(false);
    this.filteredGarments.push([...GARMENT_PRESETS]);
  }

  removeItem(i: number): void {
    if (this.orderItemsArray.length > 1) {
      this.orderItemsArray.removeAt(i);
      this.garmentDropOpen.splice(i, 1);
      this.fabricDropOpen.splice(i, 1);
      this.filteredGarments.splice(i, 1);
    }
  }

  getItemTotal(i: number): number {
    const item = this.orderItemsArray.at(i);
    return (+(item.get('quantity')?.value ?? 0)) * (+(item.get('unitPrice')?.value ?? 0));
  }

  get grandTotal(): number {
    return this.orderItemsArray.controls.reduce((s, _, i) => s + this.getItemTotal(i), 0);
  }

  get finalTotal(): number {
    return Math.max(0, this.grandTotal - (+(this.form.get('discount')?.value ?? 0)));
  }

  calcItem(): void { this.cdr.detectChanges(); }
  calcDiscount(): void { this.cdr.detectChanges(); }

  // ── Custom dropdown methods ──

  get anyDropOpen(): boolean {
    return this.customerDropOpen || this.statusDropOpen || this.measDropOpen ||
      this.garmentDropOpen.some(x => x) || this.fabricDropOpen.some(x => x);
  }

  closeAllDrops(): void {
    this.customerDropOpen = false;
    this.statusDropOpen   = false;
    this.measDropOpen     = false;
    this.garmentDropOpen  = this.garmentDropOpen.map(() => false);
    this.fabricDropOpen   = this.fabricDropOpen.map(() => false);
    this.cdr.detectChanges();
  }

  selectCustomer(c: Customer): void {
    this.form.get('customerId')?.setValue(c.customerId);
    this.selectedCustomerLabel = `${c.firstName} ${c.lastName} (${c.customerCode})`;
    this.customerDropOpen = false;
    this.loadMeasurementsForCustomer(c.customerId!);
    this.cdr.detectChanges();
  }

  selectStatus(s: string): void {
    this.form.get('status')?.setValue(s);
    this.statusDropOpen = false;
  }

  selectMeas(m: Measurement | null): void {
    this.form.get('measurementId')?.setValue(m?.measurementId ?? null);
    this.selectedMeasLabel = m
      ? `${m.measurementCode} · C:${m.chest ?? '—'}" W:${m.waist ?? '—'}" H:${m.hip ?? '—'}"`
      : '— None —';
    this.measDropOpen = false;
    if (m) {
      this.showMeasurements = false;
      this.form.get('quickMeasurements')?.reset();
    }
    this.cdr.detectChanges();
  }

  toggleGarmentDrop(i: number): void {
    const was = this.garmentDropOpen[i];
    this.closeAllDrops();
    this.garmentDropOpen[i] = !was;
  }

  onGarmentInput(event: Event, i: number): void {
    const val = (event.target as HTMLInputElement).value;
    this.orderItemsArray.at(i).get('garmentType')?.setValue(val);
    this.filteredGarments[i] = GARMENT_PRESETS.filter(g =>
      g.name.toLowerCase().includes(val.toLowerCase()));
    this.cdr.detectChanges();
  }

  selectGarment(i: number, g: GarmentPreset): void {
    this.orderItemsArray.at(i).get('garmentType')?.setValue(g.name);
    this.orderItemsArray.at(i).get('unitPrice')?.setValue(g.defaultPrice);
    this.garmentDropOpen[i] = false;
    this.cdr.detectChanges();
  }

  closeGarmentDrop(i: number): void { this.garmentDropOpen[i] = false; }

  toggleFabricDrop(i: number): void {
    const was = this.fabricDropOpen[i];
    this.closeAllDrops();
    this.fabricDropOpen[i] = !was;
  }

  selectFabric(i: number, f: FabricInventory | null): void {
    this.orderItemsArray.at(i).get('fabricId')?.setValue(f?.fabricId ?? null);
    this.fabricDropOpen[i] = false;
    this.cdr.detectChanges();
  }

  getFabricLabel(i: number): string {
    const id = this.orderItemsArray.at(i).get('fabricId')?.value;
    if (!id) return '— No fabric —';
    const f = this.fabrics.find(x => x.fabricId === id);
    return f ? `${f.name} (${f.fabricType}) · ${f.quantityInMeters}m` : '— No fabric —';
  }

  // ── Data loading ──

  loadCustomers(): void {
    this.customerService.getCustomers().subscribe({
      next: data => { this.customers = data; this.cdr.detectChanges(); }
    });
  }

  loadFabrics(): void {
    this.fabricService.getAll().subscribe({
      next: data => { this.fabrics = data; this.cdr.detectChanges(); }
    });
  }

  loadMeasurementsForCustomer(customerId: number): void {
    this.measurementService.getMeasurementsByCustomer(customerId).subscribe({
      next: data => {
        this.customerMeasurements = data;
        // Restore label when editing an order that already has a measurement linked
        const savedId = this.form.get('measurementId')?.value;
        if (savedId) {
          const m = data.find(x => x.measurementId === savedId);
          if (m) {
            this.selectedMeasLabel =
              `${m.measurementCode} · C:${m.chest ?? '—'}" W:${m.waist ?? '—'}" H:${m.hip ?? '—'}"`;
          }
        }
        this.cdr.detectChanges();
      }
    });
  }

  submit(): void {
    this.submitted = true;
    this.error = '';

    if (this.form.invalid) {
      const msgs: string[] = [];
      if (!this.form.get('customerId')?.value) {
        msgs.push('Please select a customer');
      }
      const missingGarment = this.orderItemsArray.controls.some(
        item => !item.get('garmentType')?.value
      );
      if (missingGarment) msgs.push('Please select a garment type for each item');
      const invalidQty = this.orderItemsArray.controls.some(
        item => (+(item.get('quantity')?.value ?? 0)) < 1
      );
      if (invalidQty) msgs.push('Quantity must be at least 1 for each item');
      this.error = msgs.length ? msgs.join('. ') + '.' : 'Please fill in all required fields.';
      this.cdr.detectChanges();
      return;
    }

    const val = this.form.value;
    const qm = val.quickMeasurements;
    const hasQuickMeas = qm &&
      Object.values(qm).some(v => v !== null && v !== undefined && v !== '');

    if (!val.measurementId && !hasQuickMeas) {
      this.error = 'Measurement is required. Please select an existing measurement or fill in the quick measurements below.';
      this.showMeasurements = true;
      this.cdr.detectChanges();
      return;
    }

    this.saving = true;

    // If quick measurements were entered and no existing measurement is linked,
    // save as a new measurement record first, then attach the ID to the order.
    if (hasQuickMeas && !val.measurementId) {
      const newMeas: Measurement = {
        customerId:    val.customerId,
        shopId:        val.shopId,
        chest:         qm.chest        || undefined,
        shoulder:      qm.shoulder     || undefined,
        sleeveLength:  qm.sleeveLength || undefined,
        waist:         qm.waist        || undefined,
        hip:           qm.hip          || undefined,
        neck:          qm.neck         || undefined,
        inseamLength:  qm.inseamLength || undefined,
        height:        qm.height       || undefined,
      };
      this.measurementService.createMeasurement(newMeas).subscribe({
        next: (created) => this.placeOrder(val, created.measurementId ?? undefined),
        error: () => {
          this.error = 'Failed to save measurements. Please try again.';
          this.saving = false;
          this.cdr.detectChanges();
        }
      });
    } else {
      this.placeOrder(val, val.measurementId ?? undefined);
    }
  }

  private placeOrder(val: any, measurementId: number | undefined): void {
    const payload = {
      customerId:    val.customerId,
      shopId:        val.shopId,
      deliveryDate:  val.deliveryDate || null,
      status:        val.status,
      notes:         val.notes,
      measurementId,
      discount:      val.discount ?? 0,
      orderItems:    val.orderItems
    };

    const call = this.isEdit && this.orderId
      ? this.orderService.update(this.orderId, payload)
      : this.orderService.create(payload);

    call.subscribe({
      next: () => this.router.navigate(['/orders']),
      error: (err) => {
        if (err.status === 0) {
          this.error = 'Cannot connect to server. Check your connection.';
        } else if (err.status === 400) {
          this.error = err.error?.message || 'Invalid data submitted. Please check all fields.';
        } else {
          this.error = `Save failed (Error ${err.status}). Please try again.`;
        }
        this.saving = false;
        this.cdr.detectChanges();
      }
    });
  }
}