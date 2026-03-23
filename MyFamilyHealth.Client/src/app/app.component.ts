import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TestPageComponent } from './features/test-page/test-page.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, TestPageComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'MyFamilyHealth.Client';
}
