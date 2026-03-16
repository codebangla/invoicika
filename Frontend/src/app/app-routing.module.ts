import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/dashboard' },
  {
    path: 'dashboard',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/dashboard/dashboard.module').then(
        (m) => m.DashboardModule
      ),
  },
  {
    path: 'invoices',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/invoice/invoice.module').then((m) => m.InvoiceModule),
  },
  {
    path: 'customers',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/customer/customer.module').then((m) => m.CustomerModule),
  },
  {
    path: 'items',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/item/item.module').then((m) => m.ItemModule),
  },
  {
    path: 'groups',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/group/group.module').then((m) => m.GroupModule),
  },
  {
    path: 'vat',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/vat/vat.module').then((m) => m.VatModule),
  },
  {
    path: 'users',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/user/user.module').then((m) => m.UserModule),
  },
  {
    path: 'roles',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/roles/roles.module').then((m) => m.RolesModule),
  },
  {
    path: 'login',
    loadChildren: () =>
      import('./pages/login/login.module').then((m) => m.LoginModule),
  },
  {
    path: 'signup',
    loadChildren: () =>
      import('./pages/signup/signup.module').then((m) => m.SignupModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
