import { Component, Output, EventEmitter } from '@angular/core';
import { RecipeService } from 'src/services/recipe.service';
import { Recipe } from 'src/types/Recipe';
import { IngredientType } from 'src/types/IngredientType';
import { IngredientTypeService } from 'src/services/ingredientType.service';
import { FormControl, FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { Ingredient } from 'src/types/Ingedient';
import { Step } from 'src/types/Step';


@Component({
  selector: 'add-recipe',
  templateUrl: './add-recipe.component.html',
  styleUrls: ['./add-recipe.component.scss']
})
export class AddRecipe {
  @Output() closeMe = new EventEmitter();
  @Output() recipeSaved = new EventEmitter();

  isError: boolean = false;
  errorMessage: string = "";
  ingredientTypes: IngredientType[] = [];
  recipeForm: FormGroup;

  get name(){ return this.recipeForm.get('name') as FormControl; }
  get ingredients(){ return this.recipeForm.get('ingredients') as FormArray; }
  get steps(){ return this.recipeForm.get('steps') as FormArray; }

  constructor(private recipeService: RecipeService, private ingredientTypeService: IngredientTypeService, private fb: FormBuilder){
    this.recipeForm = fb.group({
      name: ['', Validators.required, Validators.maxLength(250)],
      ingredients: fb.array([], Validators.required),
      steps: fb.array([], Validators.required)
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
    this.ingredients.push(this.fb.group({
      typeId: ['', Validators.required],
      weight: ['', Validators.required]
    }))
  }

  addStep(): void{
    this.steps.push(this.fb.group({
      text: ['', Validators.required, Validators.maxLength(1000)],
      cookTime: [''],
      prepTime: ['']
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
    let i = 1;
    const steps: Step[] = this.steps.controls.map(s => ({
      text: s.value.text,
      order: i++,
      cookTime: s.value.cookTime,
      prepTime: s.value.prepTime
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
        this.recipeSaved.emit();
      },
      err =>{
        this.errorMessage = "Cannot make this recipe";
        this.isError = true;
      }
    )
  }

  close(): void{
    this.closeMe.emit();
  }

}
