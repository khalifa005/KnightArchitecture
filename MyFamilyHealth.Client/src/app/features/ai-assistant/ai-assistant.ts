import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { Component } from '@angular/core';

@Component({
  selector: 'app-ai-assistant',
  imports: [ButtonModule, InputTextModule],
  templateUrl: './ai-assistant.html',
  styleUrl: './ai-assistant.scss',
})
export class AiAssistant {}
