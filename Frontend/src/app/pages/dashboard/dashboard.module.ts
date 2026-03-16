import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {DashboardRoutingModule} from './dashboard-routing.module';
import {DashboardComponent} from './dashboard.component';
import {NgZorroModule} from '@modules';


@NgModule({
  declarations: [
    DashboardComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    NgZorroModule
  ]
})
export class DashboardModule { }
