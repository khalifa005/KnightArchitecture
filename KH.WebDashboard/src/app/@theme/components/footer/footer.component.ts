import { Component } from '@angular/core';

@Component({
  selector: 'ngx-footer',
  styleUrls: ['./footer.component.scss'],
  template: `
    <!-- <span class="created-by">
      Created with <span style="color: #ff3d71">  ♥  </span> by <b><a href="https://progel.net" target="_blank">Progel.net</a></b> {{year}} powered by <a href="https://akveo.github.io/nebular/" target="_blank"> Nebular Akveo Theme</a>
    </span> -->
  `,
})
export class FooterComponent {

  // year: number = 1927
  // constructor() {
  //   const d = new Date();
  //   this.year = d.getFullYear();
  // }
}
