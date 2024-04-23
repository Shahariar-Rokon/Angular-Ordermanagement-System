import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MenuDate, Requisition } from '../Models/requisition';
import { AuthserviceService } from './authservice.service';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  constructor(private http:HttpClient,private service:AuthserviceService) { }
  baseUrl:string='http://localhost:5038/api/'
  getAllRequisitions():Observable<Requisition[]>{
    return this.http.get<Requisition[]>(this.baseUrl+'Requisitions',{ headers: this.service.getHeaders() })
  }
  postRequisition(menudate:MenuDate):Observable<MenuDate>{
return this.http.post<MenuDate>(this.baseUrl+'Requisitions',menudate,{ headers: this.service.getHeaders() })
  }
}
