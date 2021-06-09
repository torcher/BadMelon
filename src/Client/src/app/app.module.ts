import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';  
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptor } from 'src/interceptors/error.interceptor';
import { AuthService } from 'src/services/auth.service';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HomePage } from 'src/pages/home/home.page';
import { LoginPage } from 'src/pages/login/login.page';

import { RecipeList } from 'src/components/Recipes/recipeList/recipe-list.component';
import { AddRecipe } from 'src/components/Recipes/addRecipe/add-recipe.component';

@NgModule({
  declarations: [
    AppComponent,
    HomePage,
    LoginPage,
    RecipeList,
    AddRecipe
  ],
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    },
    AuthService
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule { }
