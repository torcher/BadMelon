import { Component, Input } from '@angular/core';
import { RecipeService } from 'src/services/recipe.service';
import { Recipe } from 'src/types/Recipe';


@Component({
  selector: 'recipe-details',
  templateUrl: './recipe-details.component.html',
  styleUrls: ['./recipe-details.component.scss']
})
export class RecipeDetails {
  @Input() recipe: Recipe = { name: '', ingredients: [], steps: []};
}
