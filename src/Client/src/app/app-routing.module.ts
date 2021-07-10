import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomePage } from 'src/pages/home/home.page';
import { LoginPage} from 'src/pages/login/login.page';
import { LogoutPage } from 'src/pages/logout/logout.page';
import { RecipePage } from 'src/pages/recipe/recipe.page';

const routes: Routes = [
  { path: '', component: HomePage },
  { path: 'login', component: LoginPage },
  { path: 'logout', component: LogoutPage},
  { path: 'recipe/:id', component: RecipePage}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
