import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'logout-page',
  templateUrl: './logout.page.html',
  styleUrls: ['./logout.page.scss']
})
export class LogoutPage {
  constructor(private auth: AuthService){ 
    auth.logout();
   }
}
