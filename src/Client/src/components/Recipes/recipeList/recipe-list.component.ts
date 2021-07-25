import { Component, EventEmitter, Input, Output } from '@angular/core';
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

  deleteMe: Recipe | undefined;

  @Output() editThisRecipe = new EventEmitter<Recipe>();

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

  deleteRecipe(recipe: Recipe): void{
    this.deleteMe = recipe;
  }

  deleteRecipeConfirm(id: string): void{
    this.recipeService.deleteRecipe(id).subscribe(
      res =>{
        this.clearDeleteRecipe();
        this.loadList();
      }
    )
  }

  clearDeleteRecipe(){
    this.deleteMe = undefined;
  }

  editRecipe(recipe: Recipe): void{
    this.editThisRecipe.emit(recipe);
  }

}
