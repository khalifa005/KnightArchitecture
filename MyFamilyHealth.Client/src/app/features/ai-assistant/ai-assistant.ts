import { Component, signal, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { AvatarModule } from 'primeng/avatar';

@Component({
  standalone: true,
  selector: 'app-ai-assistant',
  imports: [CommonModule, FormsModule, TranslocoModule, ButtonModule, InputTextModule, AvatarModule],
  templateUrl: './ai-assistant.html',
  styleUrl: './ai-assistant.scss',
})
export class AiAssistant {
  userInput = signal('');
  newMessages = signal<{id: number, isAi: boolean, content: string, time: string}[]>([]);

  sendMessage() {
    const text = this.userInput().trim();
    if (!text) return;

    // Append User Message
    this.newMessages.update(m => [...m, {
      id: Date.now(),
      isAi: false,
      content: text,
      time: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
    }]);
    
    this.userInput.set('');

    // Mock AI Reply
    setTimeout(() => {
      this.newMessages.update(m => [...m, {
        id: Date.now() + 1,
        isAi: true,
        content: `I'm a local AI proxy. You just said: "${text}". I have successfully processed this mock context!`,
        time: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
      }]);
    }, 1000);
  }
}
