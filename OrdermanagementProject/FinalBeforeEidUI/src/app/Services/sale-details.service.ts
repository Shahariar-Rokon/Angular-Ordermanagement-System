import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SaleDetails } from '../Models/sale-details';
import { Observable } from 'rxjs';
import { AuthserviceService } from './authservice.service';

@Injectable({
  providedIn: 'root'
})
export class SaleDetailsService {
  baseUrl: string = 'http://localhost:5038/api/';
  constructor(private http:HttpClient,private service:AuthserviceService) { }
  putSaleDetials(id:number,details:SaleDetails):Observable<SaleDetails>{
    return this.http.put<SaleDetails>(this.baseUrl+'SaleDetails/'+id,details,{ headers: this.service.getHeaders()})
  }
  deleteSaleDetails(id:number):Observable<SaleDetails>{
    return this.http.delete<SaleDetails>(this.baseUrl+'SaleDetails/'+id,{ headers: this.service.getHeaders()})
  }
}
