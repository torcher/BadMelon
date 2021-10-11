import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Recipe } from 'src/types/Recipe';
import { RecipeService } from 'src/services/recipe.service';
import { AppPage } from 'src/types/AppPage';

@Component({
  selector: 'recipe-page',
  templateUrl: './recipe.page.html',
  styleUrls: ['./recipe.page.scss'],
  host: { 'class': 'app-layout'}
})
export class RecipePage extends AppPage {
  recipeId: string = "";
  recipe: Recipe = { name: '', ingredients: [], steps: []};
  recipeLoaded: boolean = false;

  constructor(private route: ActivatedRoute, private recipeService: RecipeService) {
    super()
  }

  ngOnInit() {
    this.recipeId = this.route.snapshot.paramMap.get('id') ?? "";
    this.recipeService.getRecipe(this.recipeId).subscribe(data => {
      this.recipe = <Recipe>data;
      this.recipeLoaded = true;
    });
  }

}
