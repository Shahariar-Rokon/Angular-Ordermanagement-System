import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DailyMenu, DailyMenuDTO, DailyMenuDTOAfter } from '../Models/daily-menu';
import { AuthserviceService } from './authservice.service';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  baseUrl: string = 'http://localhost:5038/api/';
  constructor(private http:HttpClient,private service:AuthserviceService) { }
  getDailyMenu():Observable<DailyMenu[]>{
    return this.http.get<DailyMenu[]>(this.baseUrl+'DailyMenus',{ headers: this.service.getHeaders() });
  }
  getDailyMenuById(id:number):Observable<DailyMenu>{
    return this.http.get<DailyMenu>(this.baseUrl+'DailyMenus/'+id,{ headers: this.service.getHeaders() });
  }
  postDailyMenu(dailyMenuDTO:DailyMenuDTO):Observable<DailyMenuDTO>{
    return this.http.post<DailyMenuDTO>(this.baseUrl+'DailyMenus',dailyMenuDTO,{ headers: this.service.getHeaders() });
  }
  patchDailyMenu(id:number,dailyAfter:DailyMenuDTOAfter):Observable<DailyMenuDTOAfter>{
    return this.http.patch<DailyMenuDTOAfter>(this.baseUrl+'DailyMenus/'+id,dailyAfter,{ headers: this.service.getHeaders() });
  }
  putDailyMenu(id:number,dm:DailyMenu):Observable<DailyMenu>{
    return this.http.put<DailyMenu>(this.baseUrl+'DailyMenus/'+id,dm,{ headers: this.service.getHeaders()});
  }
  DeleteDailyMenu(id:number):Observable<DailyMenu>{
    return this.http.delete<DailyMenu>(this.baseUrl+'DailyMenus/'+id,{ headers: this.service.getHeaders()})
  }
}
