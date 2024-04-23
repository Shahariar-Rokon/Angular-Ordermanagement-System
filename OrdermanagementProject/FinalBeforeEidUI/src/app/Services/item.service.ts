import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Item } from '../Models/item';
import { AuthserviceService } from './authservice.service';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  private apiUrl = "http://localhost:5038/api/items";

  constructor(private http: HttpClient,private service:AuthserviceService) { }

  getItems(): Observable<Item[]>{
    return this.http.get<Item[]>(this.apiUrl,{ headers: this.service.getHeaders() });
  }

  getItemById(itemId: number): Observable<Item>{
    return this.http.get<Item>(`${this.apiUrl}/${itemId}`,{ headers: this.service.getHeaders() });
  }

  postItem(item: Item): Observable<Item>{
    return this.http.post<Item>(this.apiUrl, item,{ headers: this.service.getHeaders() });
  }
  
  updateItem(id: number, item: Item): Observable<Item>{
    return this.http.put<Item>(`${this.apiUrl}/${item.itemId}`, item,{ headers: this.service.getHeaders() })
  }

  deleteItem(itemId: number): Observable<Item>{
    return this.http.delete<Item>(`${this.apiUrl}/${itemId}`,{ headers: this.service.getHeaders() });
  }
}
