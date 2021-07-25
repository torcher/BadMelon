import { Component, Output, EventEmitter, Input } from '@angular/core';
import { RecipeService } from 'src/services/recipe.service';
import { Recipe } from 'src/types/Recipe';
import { IngredientType } from 'src/types/IngredientType';
import { IngredientTypeService } from 'src/services/ingredientType.service';
import { FormControl, FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { Ingredient } from 'src/types/Ingedient';
import { Step } from 'src/types/Step';


@Component({
  selector: 'save-recipe',
  templateUrl: './save-recipe.component.html',
  styleUrls: ['./save-recipe.component.scss']
})
export class SaveRecipe {
  isError: boolean = false;
  errorMessage: string = "";
  ingredientTypes: IngredientType[] = [];
  recipeForm: FormGroup;
  _editRecipe: Recipe  | undefined;
  
  @Output() closeMe = new EventEmitter();
  @Output() recipeSaved = new EventEmitter();

  @Input()  
  get editRecipe(): Recipe | undefined{
    return this._editRecipe;
  }
  set editRecipe(value: Recipe | undefined){
    this._editRecipe = value;
    this.setupFormFromRecipe();
  }

  get name(){ return this.recipeForm.get('name') as FormControl; }
  get ingredients(){ return this.recipeForm.get('ingredients') as FormArray; }
  get steps(){ return this.recipeForm.get('steps') as FormArray; }

  constructor(private recipeService: RecipeService, private ingredientTypeService: IngredientTypeService, private fb: FormBuilder){
      this.recipeForm = fb.group({
        name: ['', [Validators.required, Validators.maxLength(250)]],
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

  setupFormFromRecipe(){
    if(this._editRecipe !== undefined){
      this.recipeForm.patchValue({
        name: this.editRecipe?.name
      })
      this.removeAllIngredients();
      this._editRecipe.ingredients.forEach(i => {
        this.addIngredient(i);
      });
      this.removeAllSteps();
      this._editRecipe.steps.forEach(s =>{
        this.addStep(s);
      })
    }
  }

  addIngredient(ingredient?: Ingredient): void{
    this.ingredients.push(this.fb.group({
      typeId: [ingredient?.typeID ?? '', Validators.required],
      weight: [ingredient?.weight ?? '', Validators.required]
    }))
  }

  removeIngredient(index: number): void{
    this.ingredients.removeAt(index);
  }

  removeAllIngredients(): void{
    while(this.ingredients.length > 0)
      this.ingredients.removeAt(0);
  }

  addStep(step?: Step): void{
    this.steps.push(this.fb.group({
      text: [step?.text ?? '', [Validators.required, Validators.maxLength(1000)]],
      cookTime: [step?.cookTime ?? ''],
      prepTime: [step?.prepTime ?? '']
    }))
  }

  removeStep(index: number): void{
    this.steps.removeAt(index);
  }

  removeAllSteps(): void{
    while(this.steps.length>0)
      this.steps.removeAt(0);
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
      typeID: i.value.typeId,
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

    const recipeFromForm = this.getRecipeFromForm();
    if(this.editRecipe !== undefined){
      recipeFromForm.id = this.editRecipe.id;
      this.recipeService.updateRecipe(recipeFromForm).subscribe(
        res =>{
          this.recipeSaved.emit();
        },
        err =>{
          this.errorMessage = "Cannot make this recipe";
          this.isError = true;
        }
      )
    }
    else{
      this.recipeService.createRecipe(recipeFromForm).subscribe(
        res =>{
          this.recipeSaved.emit();
        },
        err =>{
          this.errorMessage = "Cannot make this recipe";
          this.isError = true;
        }
      )
    }
  }

  close(): void{
    this.closeMe.emit();
  }

}
