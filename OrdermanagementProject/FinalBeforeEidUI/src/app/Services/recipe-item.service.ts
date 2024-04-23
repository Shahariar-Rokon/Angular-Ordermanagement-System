import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthserviceService } from './authservice.service';
import { RecipeItems } from '../Models/recipe-items';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecipeItemService {

  baseUrl: string = 'http://localhost:5038/api/';
  constructor(private http:HttpClient,private service:AuthserviceService) { }

  getRecipeItems():Observable<RecipeItems[]>{
    return this.http.get<RecipeItems[]>(this.baseUrl+'RecipeItems',{ headers: this.service.getHeaders() });
  };

  
}
