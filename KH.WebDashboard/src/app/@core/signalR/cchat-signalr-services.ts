import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '@app/@auth/services/custom-auth-service';
import { environment } from '@environments/environment';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, from, tap } from 'rxjs';

export interface chatMesage {
  user: ChatUser;
  message: string;
  time: Date;
  isCurrentUser: boolean;
}

export interface ChatUser {
  name: string;
  group: string;
}

@Injectable({
  providedIn: 'root',
})

export class CChatSignalRService {
  private hubConnection!: signalR.HubConnection;
  private connectionUrl = 'https://localhost:5050/signalrChatHub';
  public connectionId = '';
  private apiUrl =  `${environment.apiBaseUrl}/chat`;
  user: ChatUser = { name: `Anonymous${Date.now()}`, group: 'DefaultGroup' };
  signalrConnectionStatus = 'Disconnected';

  private messagesSubject = new BehaviorSubject<chatMesage[]>([]);
  public messages$ = this.messagesSubject.asObservable();

  private groupsSubject = new BehaviorSubject<string[]>([]);
  public groups$ = this.groupsSubject.asObservable();

  private unseenMessages: { [group: string]: number } = {};
  private unseenMessagesSubject = new BehaviorSubject<{ [group: string]: number }>({});
  public unseenMessages$ = this.unseenMessagesSubject.asObservable();

public currentGroup = '';
private activeUsersSubject = new BehaviorSubject<ChatUser[]>([]);
public activeUsers$ = this.activeUsersSubject.asObservable();

public getActiveUsers(group: string) {
  if (!group) return;

  this.hubConnection.invoke('GetActiveUsers', group)
    .then((users: ChatUser[]) => {
      this.activeUsersSubject.next(users);
    })
    .catch(err => console.error('Error retrieving active users:', err));
}


  constructor(
    private authService: AuthService,
    private http: HttpClient) {

    this.user.name = this.authService.getUser().fullName;
    const token = this.authService.getAccessToken(); // Get the JWT token
    this.startConnection(token);
    this.setConnectionHandlers();
   }

  
  public startConnection(token: string): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.connectionUrl, {
        withCredentials: token != null,
        accessTokenFactory: () => {
            return token ?? '';
        },
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,

      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connection started');
        this.signalrConnectionStatus = 'Connected';
        this.joinGroup(this.user.group);
        this.switchGroup(this.user.group);
        this.getActiveUsers(this.user.group);
         // Fetch the connection ID explicitly
      
      
      })
      .catch(err => {
        console.error('Error while starting SignalR connection:', err);
        this.signalrConnectionStatus = 'Disconnected';
      });

  }

    // A. We can send messages directly to the HUB. calling the hub directly B. We can send Http requests to 
  // the API Controller and let the controller call the hub. calling the hub thru the API controller 
  // Either option is fine and this depends on your requirements. If you need to execute additional business rules
  //  besides sending the message,
  //  it is a good idea to call the API controller and execute your business rules there and not in the HUB. Just to
  //  separate concerns.
  public sendMessageToApi(message: string) {
    return this.http.post(this.apiUrl, message)
      .pipe(tap(_ => console.log("message sucessfully sent to api controller")));
  }

  public sendMessage(message: string) {
    if (!this.currentGroup || !message) return;
    this.hubConnection.invoke('SendMessage', message, this.currentGroup, this.user.name);
  }

  public joinGroup(group: string) {
    if (!group) return;

    const currentGroups = this.groupsSubject.getValue();
 // Check if the user is already in the group
 if (currentGroups.includes(group)) {
  this.switchGroup(group); // Automatically switch to the group
  return;
}

    this.hubConnection.invoke('JoinGroup', this.user.name, group).then(() => {
      const currentGroups = this.groupsSubject.getValue();
      if (!currentGroups.includes(group)) {
        this.groupsSubject.next([...currentGroups, group]);
        this.switchGroup(group); // Automatically switch to the group after joining

      }
    });
  }

  public switchGroup(group: string) {
    if (this.currentGroup === group) return;

    this.currentGroup = group;
    this.user.group = group;
    this.unseenMessages[group] = 0;
    this.unseenMessagesSubject.next({ ...this.unseenMessages });

    this.hubConnection
      .invoke('GetGroupMessages', group)
      .then((messages: chatMesage[]) => {
        messages.forEach(msg => {
          msg.isCurrentUser = msg.user.name === this.user.name;
        });
        this.messagesSubject.next(messages);
      });
  }

  public exitGroup(group: string) {
    this.hubConnection.invoke('ExitGroup', group).then(() => {
      const updatedGroups = this.groupsSubject
        .getValue()
        .filter(g => g !== group);
      this.groupsSubject.next(updatedGroups);

      if (this.currentGroup === group) {
        this.currentGroup = '';
        this.user.group = '';
        this.messagesSubject.next([]);
      }
    });
  }

  clearMessages() {
    // this.messages = [];
    this.messagesSubject.next([]); // Clear messages by emitting an empty array

  }

  private setConnectionHandlers() {
    this.hubConnection.on('OnReceiveMessage', (message: chatMesage) => {
      message.isCurrentUser = message.user.name === this.user.name;
     
     
      if (message.user.group === this.currentGroup) {
        const currentMessages = this.messagesSubject.getValue();
        this.messagesSubject.next([message, ...currentMessages]); // Add new message at the beginning

      } else {
        this.unseenMessages[message.user.group] =
          (this.unseenMessages[message.user.group] || 0) + 1;
        this.unseenMessagesSubject.next({ ...this.unseenMessages });
      }

    });
    
    this.hubConnection.on('OnNewUserJoined', (message: chatMesage) => {
      message.isCurrentUser = message.user.name === this.user.name;
     
     
      if (message.user.group === this.currentGroup) {
        const currentMessages = this.messagesSubject.getValue();
        this.messagesSubject.next([message, ...currentMessages]); // Add new message at the beginning

      } else {
        this.unseenMessages[message.user.group] =
          (this.unseenMessages[message.user.group] || 0) + 1;
        this.unseenMessagesSubject.next({ ...this.unseenMessages });
      }

      this.getActiveUsers(message.user.group);
    });

    this.hubConnection.onreconnecting(() => { 
      this.signalrConnectionStatus = 'Reconnecting';
      console.log('Reconnecting to SignalR...');
    });

    this.hubConnection.onreconnected(() => {
      this.signalrConnectionStatus = 'Connected';
      console.log('Reconnected to SignalR.');
      this.joinGroup(this.user.group);
    });

    // this.hubConnection.onclose(() => {
    //   this.signalrConnectionStatus = 'Disconnected';
    //   console.log('SignalR connection closed.');
    //   const token = this.authService.getAccessToken(); // Get the JWT token
    
    //   setTimeout(() => this.startConnection(token), 2000); // Reconnect after 2 seconds
    // });
  }
}

