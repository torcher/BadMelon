import { Component } from '@angular/core';
import { Recipe } from 'src/types/Recipe';


@Component({
  selector: 'home-page',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss']
})
export class HomePage {
  title: string = 'Home';
  recipeListLoaded: boolean = false;
  showAddRecipe: boolean = false;
  editRecipe: Recipe | undefined;

  toggleRecipeButton(): void{
    this.showAddRecipe = !this.showAddRecipe;
  }

  recipeSaved(): void{
    this.recipeListLoaded = !this.recipeListLoaded;
    this.toggleRecipeButton();
  }

  setEditRecipe(recipe: Recipe) : void{
    this.editRecipe = recipe;
    this.showAddRecipe = true;
  }

}
