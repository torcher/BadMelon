import { Component } from '@angular/core';
import { RecipeService } from 'src/services/recipe.service';
import { Recipe } from 'src/types/Recipe';


@Component({
  selector: 'recipe-list',
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.scss']
})
export class RecipeList {
  recipes: Recipe[] = [];

  constructor(private recipeService: RecipeService){
    recipeService.getAllRecipes().subscribe(data => this.recipes = <Recipe[]>data);
  }

}
