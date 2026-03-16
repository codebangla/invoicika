import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ItemGroupService } from 'src/app/services/item-group.service';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Router } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.less']
})
export class GroupListComponent implements OnInit {
  total = 0;
  listOfGroups: any[] = [];
  loading = true;
  pageSize = 10;
  pageIndex = 1;
  searchText: string | null = null;
  constructor(
    private itemGroupService: ItemGroupService,
    private location: Location,
    private message: NzMessageService,
    private router: Router,
    private modal: NzModalService
  ) {}

  ngOnInit(): void {
    this.loadDataFromServer(this.pageIndex, this.pageSize, null, null, [], null);
  }

  loadDataFromServer(
    pageIndex: number,
    pageSize: number,
    sortField: string | null,
    sortOrder: string | null,
    filters: Array<{ key: string; value: string[] }>,
    searchTerm: string | null
  ): void {
    this.loading = true;
    if (this.searchText) {
      filters.push({ key: 'searchTerm', value: [this.searchText] });
    }
    this.itemGroupService.getItemGroups(pageIndex, pageSize, sortField, sortOrder, filters, searchTerm).subscribe(response => {
      this.loading = false;
      this.total = response.totalCount;
      this.listOfGroups = response.items;
      this.searchText = '';
    }, error => {
      this.loading = false;
      this.message.error('Error loading item groups');
    });
  }

  onQueryParamsChange(params: NzTableQueryParams): void {
    const { pageSize, pageIndex, sort, filter } = params;
    const currentSort = sort.find(item => item.value !== null);
    const sortField = currentSort?.key || null;
    const sortOrder = currentSort?.value || null;
    this.loadDataFromServer(pageIndex, pageSize, sortField, sortOrder, filter, this.searchText);
  }
  onSearch(): void {
    this.loadDataFromServer(this.pageIndex, this.pageSize, null, null, [], this.searchText);
  }

  deleteGroup(group: any): void {
    this.itemGroupService.deleteItemGroup(group.itemGroupId).subscribe(() => {
      this.listOfGroups = this.listOfGroups.filter(i => i.itemGroupId !== group.itemGroupId);
      this.message.success('Item group deleted successfully');
      this.loadDataFromServer(this.pageIndex, this.pageSize, null, null, [], null);
      this.router.navigate(['/groups/all']);
    }, error => {
      this.message.error('Error deleting item group');
    });
  }
  showDeleteConfirm(group: any): void {
    this.modal.confirm({
      nzTitle: 'Are you sure you want to delete this item group?',
      nzContent: '<b style="color: red;">This action cannot be undone.</b>',
      nzOkText: 'Yes',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzOnOk: () => this.deleteGroup(group),
      nzCancelText: 'No',
      nzOnCancel: () => console.log('Cancel')
    });
  }

  onBack(): void {
    this.location.back();
  }
}
