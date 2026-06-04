import { Routes } from '@angular/router';
import { authGuard } from './auth-guard';

import { Login } from './pages/login/login/login';
import { Register } from './pages/register/register/register';
import { CustomerList } from './pages/customers/customer-list/customer-list';
import { CustomerForm } from './pages/customers/customer-form/customer-form';
import { MeasurementList } from './pages/measurements/measurements-list/measurements-list';
import { MeasurementForm } from './pages/measurements/measurements-form/measurements-form';
import { FabricList } from './pages/fabrics/fabric-list/fabric-list';
import { FabricForm } from './pages/fabrics/fabric-form/fabric-form';
import { OrderList } from './pages/orders/order-list/order-list';
import { OrderForm } from './pages/orders/order-form/order-form';

export const routes: Routes = [
  { path: 'login', component: Login },

  { path: 'customers',          component: CustomerList,    canActivate: [authGuard] },
  { path: 'customers/new',      component: CustomerForm,    canActivate: [authGuard] },
  { path: 'customers/:id/edit', component: CustomerForm,    canActivate: [authGuard] },

  { path: 'measurements',          component: MeasurementList, canActivate: [authGuard] },
  { path: 'measurements/new',      component: MeasurementForm, canActivate: [authGuard] },
  { path: 'measurements/:id/edit', component: MeasurementForm, canActivate: [authGuard] },

  { path: 'inventory',          component: FabricList, canActivate: [authGuard] },
  { path: 'inventory/new',      component: FabricForm, canActivate: [authGuard] },
  { path: 'inventory/:id/edit', component: FabricForm, canActivate: [authGuard] },

  { path: 'orders',          component: OrderList, canActivate: [authGuard] },
  { path: 'orders/new',      component: OrderForm, canActivate: [authGuard] },
  { path: 'orders/:id/edit', component: OrderForm, canActivate: [authGuard] },

  { path: 'users/new', component: Register, canActivate: [authGuard] },

  { path: '', redirectTo: 'customers', pathMatch: 'full' }
];