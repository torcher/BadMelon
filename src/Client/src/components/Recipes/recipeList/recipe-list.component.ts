import { Component, Input } from '@angular/core';
import { RecipeService } from 'src/services/recipe.service';
import { Recipe } from 'src/types/Recipe';


@Component({
  selector: 'recipe-list',
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.scss']
})
export class RecipeList {
  recipes: Recipe[] = [];
  showSpinner: boolean = true;
  private _loadRecipes: boolean = false;

  @Input() 
  get loadRecipes(): boolean{ return this._loadRecipes; }
  set loadRecipes(value: boolean)
  { 
    this._loadRecipes = value; 
    this.loadList(); 
  }

  constructor(private recipeService: RecipeService){
    this.loadList();
  }

  private loadList(): void{
    this.showSpinner = true;
    this.recipeService.getAllRecipes().subscribe(data =>
      { 
        this.recipes = <Recipe[]>data;
        this.showSpinner = false;
      });
  }

}
