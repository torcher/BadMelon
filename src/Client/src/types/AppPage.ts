import { Component, HostBinding } from "@angular/core";

@Component({
  selector: 'app-base',
  template: `
      <div>
          base works!!
      </div>
  `,
  host: { 'class': 'page-grid'}
})
export class AppPage {
  @HostBinding('class.nav-open') navOpen: boolean = false; 

  toggleNav(openOrClosed: boolean): void{
    this.navOpen = openOrClosed;
  }
}