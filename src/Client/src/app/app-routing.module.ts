import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomePage } from 'src/pages/home/home.page';
import { LoginPage} from 'src/pages/login/login.page';
import { LogoutPage } from 'src/pages/logout/logout.page';

const routes: Routes = [
  { path: '', component: HomePage },
  { path: 'login', component: LoginPage },
  { path: 'logout', component: LogoutPage}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
