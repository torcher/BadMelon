import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { Observable } from 'rxjs';
import { IngredientType } from 'src/types/IngredientType';

@Injectable({
  providedIn: 'root',
})
export class IngredientTypeService{
  constructor(private http: HttpService){  }

  getAllIngredientTypes(): Observable<IngredientType[]>{
    return this.http.get<IngredientType[]>("ingredientType");
  }
}