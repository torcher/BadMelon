import { Component } from '@angular/core';
import { RecipeService } from 'src/services/recipe.service';
import { Recipe } from 'src/types/Recipe';


@Component({
  selector: 'add-recipe',
  templateUrl: './add-recipe.component.html',
  styleUrls: ['./add-recipe.component.scss']
})
export class AddRecipe {
  recipe: Recipe;
  isError: boolean = false;
  errorMessage: string = "";

  constructor(private recipeService: RecipeService){
    this.recipe = { name: "", steps:[{ text: "Step 1"}], ingredients: [{ typeId: "96acc82f-912b-4c8a-9efb-7f384112780c", weight: 1.0 }]};
  }

  submit(): void{
    this.recipeService.createRecipe(this.recipe).subscribe(
      res =>{

      },
      err =>{
        this.errorMessage = "Cannot make this recipe";
        this.isError = true;
      }
    )
  }

}
