import { Component } from '@angular/core';
import { NbIconConfig } from '@nebular/theme';

@Component({
  selector: 'app-pipes',
  templateUrl: './pipes.component.html',
  styleUrl: './pipes.component.scss'
})
export class PipesComponent {
  htmlContent: string = '<h3> test h3 html</h3>';


  bellIconConfig: NbIconConfig = { icon: 'bell-outline', pack: 'eva' };
  
}
