import { Component, Input } from '@angular/core';

export enum InvoiceStatus {
  Draft = 0,
  Pending = 1,
  Paid = 2
}

const STATUS_CONFIG = {
  [InvoiceStatus.Draft]: { label: 'Draft', class: 'ant-tag-yellow' },
  [InvoiceStatus.Pending]: { label: 'Pending', class: 'ant-tag-green' },
  [InvoiceStatus.Paid]: { label: 'Paid', class: 'ant-tag-blue' },
};

@Component({
  selector: 'invoice-status',
  templateUrl: 'invoice-status.component.html',
  styleUrls: ['invoice-status.component.scss']
})
export class InvoiceStatusComponent {
  #status: InvoiceStatus = InvoiceStatus.Draft;

  antTagClass = STATUS_CONFIG[this.#status].class;
  label = STATUS_CONFIG[this.#status].label;

  @Input()
  set status(value: InvoiceStatus | number) {
    this.#status = value as InvoiceStatus;
    this.updateState();
  }

  get status(): InvoiceStatus {
    return this.#status;
  }

  private updateState(): void {
    const config = STATUS_CONFIG[this.#status] || STATUS_CONFIG[InvoiceStatus.Draft];
    this.antTagClass = config.class;
    this.label = config.label;
  }
}
