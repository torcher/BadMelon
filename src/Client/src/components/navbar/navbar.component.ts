import { Component, EventEmitter, HostBinding, Output } from "@angular/core";
import { Router } from "@angular/router";

@Component({
  selector: 'nav',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class Navbar {

  @Output() onNavStateChange = new EventEmitter<boolean>();

  @HostBinding('class.opened') navOpened: boolean = false;
  togglerText: string = ">>";

  constructor(private router: Router) {
  }

  public isActive(route: string): boolean{
    return this.router.url === route
  }

  public toggleOpenNav(): void{
    this.navOpened = !this.navOpened;
    if(this.navOpened)
      this.togglerText = "<<"
    else
      this.togglerText = ">>"

    this.onNavStateChange.emit(this.navOpened)
  }

}