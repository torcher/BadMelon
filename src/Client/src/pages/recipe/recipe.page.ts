import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Recipe } from 'src/types/Recipe';
import { RecipeService } from 'src/services/recipe.service';

@Component({
  selector: 'recipe-page',
  templateUrl: './recipe.page.html',
  styleUrls: ['./recipe.page.scss']
})
export class RecipePage {
  recipeId: string = "";
  recipe: Recipe = { name: '', ingredients: [], steps: []};
  recipeLoaded: boolean = false;

  constructor(private route: ActivatedRoute, private recipeService: RecipeService) {
  }

  ngOnInit() {
    this.recipeId = this.route.snapshot.paramMap.get('id') ?? "";
    this.recipeService.getRecipe(this.recipeId).subscribe(data => {
      this.recipe = <Recipe>data;
      this.recipeLoaded = true;
    });
  }

}
