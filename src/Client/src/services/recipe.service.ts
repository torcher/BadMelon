import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { Recipe } from 'src/types/Recipe';
import { Observable } from 'rxjs';
import { HttpResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class RecipeService{
  constructor(private http: HttpService){  }

  getAllRecipes(): Observable<Recipe[]>{
    return this.http.get<Recipe[]>("recipe");
  }

  createRecipe(recipe: Recipe): Observable<HttpResponse<Recipe>>{
    return this.http.post<Recipe>("recipe", recipe);
  }

  getRecipe(recipeId: string): Observable<Recipe>{
    return this.http.get<Recipe>("recipe/" + recipeId);
  }
}