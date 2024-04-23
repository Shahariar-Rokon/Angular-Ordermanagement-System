import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Recipe, RecipeDTO } from '../Models/recipe';
import { AuthserviceService } from './authservice.service';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {

  constructor(private http:HttpClient,private service:AuthserviceService) { }
  baseUrl: string = 'http://localhost:5038/api/';

  getRecipes():Observable<Recipe[]>{
    return this.http.get<Recipe[]>(this.baseUrl+'Recipes',{ headers: this.service.getHeaders()});
  };

  getRecipeById(id:number):Observable<Recipe>{
    return this.http.get<Recipe>(this.baseUrl+'Recipes/'+id,{ headers: this.service.getHeaders()});
  };

  postRecipe(recipe:RecipeDTO):Observable<RecipeDTO>{
    return this.http.post<RecipeDTO>(this.baseUrl+'Recipes',recipe,{ headers: this.service.getHeaders()});
  };

  updateRecipe(id: number, recipe: RecipeDTO): Observable<Recipe>{
    return this.http.put<Recipe>(this.baseUrl+'Recipes/'+id, recipe,{ headers: this.service.getHeaders() })
  }

  deleteRecipe(recipeId: number): Observable<Recipe>{
    return this.http.delete<Recipe>(`${this.baseUrl + 'Recipes'}/${recipeId}`,{ headers: this.service.getHeaders() });
  }
}
