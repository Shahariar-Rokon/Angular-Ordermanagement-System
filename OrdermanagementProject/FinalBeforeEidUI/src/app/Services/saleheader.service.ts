import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SaleHeader } from '../Models/saleheader';
import { Observable } from 'rxjs';
import { AuthserviceService } from './authservice.service';

@Injectable({
  providedIn: 'root'
})
export class SaleheaderService {
  baseUrl: string = 'http://localhost:5038/api/'
  constructor(private http: HttpClient,private service:AuthserviceService) { }
  
  postSales(head: SaleHeader): Observable<SaleHeader> {
    return this.http.post<SaleHeader>(this.baseUrl + 'SaleHeaders', head,{ headers: this.service.getHeaders() });
  }
  getSales(): Observable<SaleHeader[]> {
    return this.http.get<SaleHeader[]>(this.baseUrl + "SaleHeaders",{ headers: this.service.getHeaders() });
  }
  putSales(id:number,head:SaleHeader):Observable<SaleHeader>{
    return this.http.put<SaleHeader>(this.baseUrl+'SaleHeaders/'+id,head,{ headers: this.service.getHeaders() })
  }
  deleteSales(id:number):Observable<SaleHeader>{
    return this.http.delete<SaleHeader>(this.baseUrl+'SaleHeaders/'+id,{ headers: this.service.getHeaders() })
  }
  getSalesById(id:number):Observable<SaleHeader>{
    return this.http.get<SaleHeader>(this.baseUrl+'SaleHeaders/'+id,{ headers: this.service.getHeaders() })
  }
}
