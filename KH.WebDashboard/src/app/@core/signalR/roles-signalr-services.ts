import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class RolesSignalRService {
  private hubConnection!: signalR.HubConnection;

  public startConnection(token: string): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5050/signalrhub', {
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
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('Error while starting SignalR connection: ', err));
  }

  //RoleAdded is the method we will call from our api or server side to notify the client side
  public addRoleListener(onRoleAdded: (role: any) => void): void {
    this.hubConnection.on('RoleAdded', (role) => {
      onRoleAdded(role);
    });
  }

  //this can call server hub metod to update the role
  async updateFoodItem(roleId: number, state: any) {
    await this.hubConnection.invoke('UpdateRoleItem', roleId, state);
  }

}

