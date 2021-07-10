import { Component } from '@angular/core';
import { RecipeService } from 'src/services/recipe.service';
import { Recipe } from 'src/types/Recipe';
import { IngredientType } from 'src/types/IngredientType';
import { IngredientTypeService } from 'src/services/ingredientType.service';
import { FormControl, FormGroup, FormArray } from '@angular/forms';
import { Ingredient } from 'src/types/Ingedient';
import { Step } from 'src/types/Step';


@Component({
  selector: 'add-recipe',
  templateUrl: './add-recipe.component.html',
  styleUrls: ['./add-recipe.component.scss']
})
export class AddRecipe {
  recipe: Recipe;
  isError: boolean = false;
  errorMessage: string = "";
  ingredientTypes: IngredientType[] = [];
  recipeForm: FormGroup;

  get ingredients(){ return this.recipeForm.get('ingredients') as FormArray; }
  get steps(){ return this.recipeForm.get('steps') as FormArray; }

  constructor(private recipeService: RecipeService, private ingredientTypeService: IngredientTypeService){
    this.recipe = { name: "", steps:[{ text: "Step 1"}], ingredients: [{ typeId: "96acc82f-912b-4c8a-9efb-7f384112780c", type: "Panko", weight: 1.0 }]};
    this.recipeForm = new FormGroup({
      name: new FormControl(),
      ingredients: new FormArray([]),
      steps: new FormArray([])
    })
  }

  ngOnInit(): void{
    this.ingredientTypeService.getAllIngredientTypes().subscribe(
      res =>{
        this.ingredientTypes = res;
      },
      err =>{
        this.errorMessage = "There is a problem with the server. Contact support.";
      }
    )
  }

  addIngredient(): void{
    this.ingredients.push(new FormGroup({
      typeId: new FormControl(),
      weight: new FormControl()
    }))
  }

  addStep(): void{
    this.steps.push(new FormGroup({
      text: new FormControl()
    }))
  }

  getRecipeFromForm(): Recipe{
    return {
      name: this.recipeForm.value.name,
      ingredients: this.getIngredientsFromForm(),
      steps: this.getStepsFromForm()
    };

  }

  getIngredientsFromForm(): Ingredient[]{
    const ingredients: Ingredient[] = this.ingredients.controls.map(i => ({
      typeId: i.value.typeId,
      weight: i.value.weight,
      type: ''
    }));
    return ingredients;
  }

  getStepsFromForm(): Step[]{
    const steps: Step[] = this.steps.controls.map(s => ({
      text: s.value.text
    }));
    return steps;
  }

  submit(): void{
    if(this.recipeForm.invalid){
      this.errorMessage = "Invalid Recipe";
      return;
    }

    this.recipeService.createRecipe(this.getRecipeFromForm()).subscribe(
      res =>{

      },
      err =>{
        this.errorMessage = "Cannot make this recipe";
        this.isError = true;
      }
    )
  }

}
