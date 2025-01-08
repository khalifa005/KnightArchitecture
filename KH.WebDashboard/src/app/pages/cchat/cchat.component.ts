import { Component, OnInit } from '@angular/core';
import { chatMesage, CChatSignalRService } from '@app/@core/signalR/cchat-signalr-services';

@Component({
  selector: 'app-cchat',
  templateUrl: './cchat.component.html',
  styleUrl: './cchat.component.scss'
})
export class CChatComponent  {
  message = '';
  group = '';

  messages$ = this.chatService.messages$;
  groups$ = this.chatService.groups$;
  unseenMessages$ = this.chatService.unseenMessages$;

  constructor(public chatService: CChatSignalRService) {}
  // ngOnInit(): void {
  //   this.switchGroup(this.chatService.user.group)
  // }

  sendMessage() {
    this.chatService.sendMessage(this.message);
    this.message = '';
  }

  joinGroup() {
    this.chatService.joinGroup(this.group);
    this.group = '';
  }

  switchGroup(group: string) {
    // if (this.chatService.user.group === group) return;
  
    this.chatService.switchGroup(group);
    this.chatService.getActiveUsers(group); // Fetch active users for the new group
  }

  exitGroup(group: string) {
    this.chatService.exitGroup(group);
    this.chatService.getActiveUsers(group); // Fetch active users for the new group

  }

  clearMessages() {
    this.chatService.clearMessages();
  }
}