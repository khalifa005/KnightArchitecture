import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { TranslateService } from '@ngx-translate/core';
import { DefaultConfig, STYLE } from 'ngx-easy-table';
import { Subject, takeUntil } from 'rxjs';
import { UserListResponsePagedListApiResponse, UsersService, UserFilterRequest, CreateUserRequest } from 'src/open-api';

@Component({
  selector: 'app-user-manager',
  templateUrl: './user-manager.component.html',
  styleUrl: './user-manager.component.scss'
})
export class UserManagerComponent implements OnInit {

  constructor() {}

  ngOnInit(): void {

  }



}