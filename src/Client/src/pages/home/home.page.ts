import { Component } from '@angular/core';


@Component({
  selector: 'home-page',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss']
})
export class HomePage {
  title: string = 'Home';
  showAddRecipe: boolean = false;

  addRecipeButton(): void{
    this.showAddRecipe = true;
  }

}
