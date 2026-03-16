import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ItemGroupService {
  private apiUrl = environment.apiUrl + 'itemgroups';

  constructor(private http: HttpClient) {}

  getItemGroups(
    pageNumber: number,
    pageSize: number,
    sortField: string | null,
    sortOrder: string | null,
    filters: Array<{ key: string; value: string[] }>,
    searchTerm: string | null
  ): Observable<any> {
    let params = new HttpParams()
      .set('pageNumber', `${pageNumber}`)
      .set('pageSize', `${pageSize}`)
      .set('sortField', sortField || '')
      .set('sortOrder', sortOrder || '');
      if (searchTerm) {
        params = params.set('searchTerm', searchTerm);
      }

    filters.forEach((filter) => {
      filter.value.forEach((value) => {
        params = params.append(filter.key, value);
      });
    });

    return this.http
      .get<any>(this.apiUrl, { params })
      .pipe(catchError(() => of([])));
  }

  createItemGroup(itemGroup: any): Observable<void> {
    return this.http.post<void>(this.apiUrl, itemGroup);
  }

  updateItemGroup(itemGroupId: string, itemGroup: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${itemGroupId}`, itemGroup);
  }

  getItemGroupById(itemGroupId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${itemGroupId}`);
  }

  deleteItemGroup(itemGroupId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${itemGroupId}`);
  }
}
