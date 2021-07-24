import { Component } from '@angular/core';


@Component({
  selector: 'home-page',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss']
})
export class HomePage {
  title: string = 'Home';
  recipeListLoaded: boolean = false;
  showAddRecipe: boolean = false;

  toggleRecipeButton(): void{
    this.showAddRecipe = !this.showAddRecipe;
  }

  recipeSaved(): void{
    this.recipeListLoaded = !this.recipeListLoaded;
    this.toggleRecipeButton();
  }

}
