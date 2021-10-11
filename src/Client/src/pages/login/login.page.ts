import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/services/auth.service';
import { AppPage } from 'src/types/AppPage';

@Component({
  selector: 'login-page',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
  host: { 'class': 'single-form'}
})
export class LoginPage extends AppPage {
  username: string = "";
  password: string = "";
  errorMessage: string = "";

  constructor(private router: Router, private auth: AuthService){ super() }

  submit(): void{
    console.log("login")
    this.auth.login(this.username,this.password, "/")
      .subscribe(
      res =>{
      },
      err =>{
         if(err.status === 400)
          this.errorMessage = "Uh oh!"
        else if(err.status === 0)
          this.errorMessage = "The server is not communicating. Please contact an admin.";
      });
  }
}
