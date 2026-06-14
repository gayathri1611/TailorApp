import { Routes } from '@angular/router';
import { authGuard } from './auth-guard';
import { MeasurementLimits } from './pages/measurements/measurementlimits/measurementlimits';
import { adminGuard } from './admin-guard';
import { Settings } from './pages/settings/settings';
import { NameValueManager } from './pages/settings/namevaluemanager/namevaluemanager';

export const routes: Routes = [
  { path: 'login',    loadComponent: () => import('./pages/login/login').then(m => m.Login) },
  { path: 'register', loadComponent: () => import('./pages/register/register').then(m => m.Register) },
  { path: 'auth/callback', loadComponent: () => import('./pages/auth/auth-callback').then(m => m.AuthCallback) },

  { path: 'dashboard', loadComponent: () => import('./pages/dashboard/dashboard/dashboard').then(m => m.Dashboard), canActivate: [authGuard] },

  { path: 'customers',          loadComponent: () => import('./pages/customers/customer-list/customer-list').then(m => m.CustomerList), canActivate: [authGuard] },
  { path: 'customers/new',      loadComponent: () => import('./pages/customers/customer-form/customer-form').then(m => m.CustomerForm), canActivate: [authGuard] },
  { path: 'customers/:id/edit', loadComponent: () => import('./pages/customers/customer-form/customer-form').then(m => m.CustomerForm), canActivate: [authGuard] },

  { path: 'measurements',          loadComponent: () => import('./pages/measurements/measurements-list/measurements-list').then(m => m.MeasurementList), canActivate: [authGuard] },
  { path: 'measurements/new',      loadComponent: () => import('./pages/measurements/measurements-form/measurements-form').then(m => m.MeasurementForm), canActivate: [authGuard] },
  { path: 'measurements/:id/edit', loadComponent: () => import('./pages/measurements/measurements-form/measurements-form').then(m => m.MeasurementForm), canActivate: [authGuard] },

  { path: 'inventory',          loadComponent: () => import('./pages/fabrics/fabric-list/fabric-list').then(m => m.FabricList), canActivate: [authGuard] },
  { path: 'inventory/new',      loadComponent: () => import('./pages/fabrics/fabric-form/fabric-form').then(m => m.FabricForm), canActivate: [authGuard] },
  { path: 'inventory/:id/edit', loadComponent: () => import('./pages/fabrics/fabric-form/fabric-form').then(m => m.FabricForm), canActivate: [authGuard] },

  { path: 'orders',          loadComponent: () => import('./pages/orders/order-list/order-list').then(m => m.OrderList), canActivate: [authGuard] },
  { path: 'orders/new',      loadComponent: () => import('./pages/orders/order-form/order-form').then(m => m.OrderForm), canActivate: [authGuard] },
  { path: 'orders/:id/edit', loadComponent: () => import('./pages/orders/order-form/order-form').then(m => m.OrderForm), canActivate: [authGuard] },

  { path: 'users',          loadComponent: () => import('./pages/user-list/user-list').then(m => m.UserList), canActivate: [authGuard] },
  { path: 'users/new',      loadComponent: () => import('./pages/user-form/user-form').then(m => m.UserForm), canActivate: [authGuard] },
  { path: 'users/:id/edit', loadComponent: () => import('./pages/user-form/user-form').then(m => m.UserForm), canActivate: [authGuard] },

  // Add to app.routes.ts:
{ path: 'settings/measurement-limits', component: MeasurementLimits, canActivate: [authGuard] },
{ path: 'settings',                           component: Settings,          canActivate: [adminGuard] },
{ path: 'settings/namevalues',                component: NameValueManager,  canActivate: [adminGuard] },
{ path: 'settings/measurement-limits',        component: MeasurementLimits, canActivate: [adminGuard] },
  
{ path: '', redirectTo: 'dashboard', pathMatch: 'full' },
];
