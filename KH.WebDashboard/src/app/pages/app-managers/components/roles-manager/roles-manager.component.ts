import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { AuthService } from '@app/@auth/services/custom-auth-service';
import { RolesSignalRService } from '@app/@core/signalR/roles-signalr-services';

@Component({
  selector: 'app-roles-manager',
  templateUrl: './roles-manager.component.html',
  styleUrl: './roles-manager.component.scss'
})
export class RolesManagerComponent  implements OnInit {
  roles: any[] = []; // Array to store roles
  loadingRoles:boolean = false;

  constructor(
    private rolesSignalRService: RolesSignalRService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // const token = this.authService.getAccessToken(); // Get the JWT token
    // this.rolesSignalRService.startConnection(token);
    // this.rolesSignalRService.addRoleListener((role) => {
    //   this.loadingRoles = true;
    //   this.roles.push(role); // Add the new role to the roles array
    //   this.loadingRoles = false;
    
    // });
  }
}