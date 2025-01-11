
# SignalR with Angular Integration

This guide explains how to implement **SignalR** for real-time communication between an ASP.NET Core backend and an Angular frontend. The code covers all hub methods, including client and server communication.

---

## 🎯 Features
- Real-time messaging with WebSockets.
- Group-based communication.
- Active user tracking.
- Dynamic group management.

---

## 📋 Prerequisites
1. .NET Core SDK installed.
2. Node.js and npm installed.
3. Angular CLI installed.
4. Visual Studio Code or any preferred IDE.

---

## 🛠 Backend Implementation

### Step 1: Install SignalR
1. Add SignalR to your project:
   ```bash
   Install-Package Microsoft.AspNet.SignalR
   ```

### Step 2: SignalR Hub Configuration
**File: `Startup.cs`**
```csharp
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalRHub.Startup))]

namespace SignalRHub
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var signalRConfig = new Microsoft.AspNet.SignalR.HubConfiguration
            {
                EnableDetailedErrors = true,
            };
            app.MapSignalR("/signalr", signalRConfig);
        }
    }
}
```

### Step 3: SignalR Hub Implementation
Define the hub with the required methods for communication.

**File: `ChatHub.cs`**
```csharp
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    // Send a message to a specific group
    public void SendMessage(string message, string group)
    {
        Clients.Group(group).OnReceiveMessage(new
        {
            User = Context.ConnectionId,
            Message = message,
            Time = System.DateTime.UtcNow
        });
    }

    // Join a group
    public Task JoinGroup(string name, string groupName)
    {
        Clients.Group(groupName).OnNewUserJoined(new { User = name, Group = groupName });
        return Groups.Add(Context.ConnectionId, groupName);
    }

    // Exit a group
    public Task ExitGroup(string groupName)
    {
        Clients.Group(groupName).OnUserLeft(new { User = Context.ConnectionId, Group = groupName });
        return Groups.Remove(Context.ConnectionId, groupName);
    }

    // Get active users in a group
    public Task GetActiveUsers(string groupName)
    {
        return Clients.Caller.OnActiveUsers(new { Users = new[] { "User1", "User2" } });
    }

    // Get messages for a specific group
    public Task GetGroupMessages(string groupName)
    {
        return Clients.Caller.OnGroupMessages(new[]
        {
            new { User = "User1", Message = "Welcome to the group!", Time = System.DateTime.UtcNow }
        });
    }

    public override Task OnConnected()
    {
        Clients.Caller.OnConnected("Welcome to SignalR Chat!");
        return base.OnConnected();
    }

    public override Task OnDisconnected(bool stopCalled)
    {
        // Handle user disconnection
        return base.OnDisconnected(stopCalled);
    }
}
```

---

## 🖥 Frontend Implementation

### Step 1: Install SignalR Client
```bash
npm install @microsoft/signalr
```

### Step 2: Create SignalR Service
**File: `cchat-signalr-services.ts`**
```typescript
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

export interface ChatMessage {
  user: string;
  message: string;
  time: Date;
}

@Injectable({
  providedIn: 'root',
})
export class CChatSignalRService {
  private hubConnection!: signalR.HubConnection;
  private connectionUrl = 'https://localhost:5050/signalr';

  public messagesSubject = new BehaviorSubject<ChatMessage[]>([]);
  public messages$ = this.messagesSubject.asObservable();

  public groupsSubject = new BehaviorSubject<string[]>([]);
  public groups$ = this.groupsSubject.asObservable();

  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.connectionUrl)
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error starting connection:', err));

    this.setHubMethods();
  }

  public sendMessage(message: string, group: string): void {
    this.hubConnection.invoke('SendMessage', message, group).catch(err => console.error(err));
  }

  public joinGroup(name: string, group: string): void {
    this.hubConnection.invoke('JoinGroup', name, group).catch(err => console.error(err));
  }

  public exitGroup(group: string): void {
    this.hubConnection.invoke('ExitGroup', group).catch(err => console.error(err));
  }

  public getActiveUsers(group: string): void {
    this.hubConnection.invoke('GetActiveUsers', group).catch(err => console.error(err));
  }

  public getGroupMessages(group: string): void {
    this.hubConnection.invoke('GetGroupMessages', group).catch(err => console.error(err));
  }

  private setHubMethods(): void {
    this.hubConnection.on('OnReceiveMessage', (message: ChatMessage) => {
      const currentMessages = this.messagesSubject.getValue();
      this.messagesSubject.next([...currentMessages, message]);
    });

    this.hubConnection.on('OnNewUserJoined', (data: any) => {
      console.log(`${data.User} joined ${data.Group}`);
    });

    this.hubConnection.on('OnUserLeft', (data: any) => {
      console.log(`${data.User} left ${data.Group}`);
    });

    this.hubConnection.on('OnActiveUsers', (data: any) => {
      console.log('Active Users:', data.Users);
    });

    this.hubConnection.on('OnGroupMessages', (messages: ChatMessage[]) => {
      this.messagesSubject.next(messages);
    });
  }
}
```

---

### Step 3: Chat Component
**File: `cchat.component.ts`**
```typescript
import { Component } from '@angular/core';
import { CChatSignalRService, ChatMessage } from '@app/@core/signalR/cchat-signalr-services';

@Component({
  selector: 'app-cchat',
  templateUrl: './cchat.component.html',
  styleUrls: ['./cchat.component.scss'],
})
export class CChatComponent {
  message = '';
  group = '';
  messages$ = this.chatService.messages$;

  constructor(public chatService: CChatSignalRService) {}

  sendMessage(): void {
    this.chatService.sendMessage(this.message, this.group);
    this.message = '';
  }

  joinGroup(): void {
    this.chatService.joinGroup('User', this.group);
    this.group = '';
  }

  exitGroup(): void {
    this.chatService.exitGroup(this.group);
  }

  getGroupMessages(): void {
    this.chatService.getGroupMessages(this.group);
  }
}
```

**File: `cchat.component.html`**
```html
<div class="chat-container">
  <div class="messages">
    <div *ngFor="let message of messages$ | async">
      <strong>{{ message.user }}</strong>: {{ message.message }} ({{ message.time | date: 'short' }})
    </div>
  </div>

  <input [(ngModel)]="message" placeholder="Type a message..." />
  <button (click)="sendMessage()">Send</button>

  <input [(ngModel)]="group" placeholder="Group name..." />
  <button (click)="joinGroup()">Join</button>
  <button (click)="exitGroup()">Leave</button>
</div>
```

---

## 🛠 Testing
1. Start the backend:
   ```bash
   dotnet run
   ```
2. Start the Angular frontend:
   ```bash
   ng serve
   ```
3. Open multiple browser tabs to simulate real-time messaging.

---

## 🎉 Features Demo
- Send real-time messages between clients.
- Join or leave groups dynamically.
- Track active users in a group.

---

## 📚 Resources
- [SignalR Documentation](https://learn.microsoft.com/en-us/aspnet/signalr/overview)
- [Angular SignalR Client](https://www.npmjs.com/package/@microsoft/signalr)
