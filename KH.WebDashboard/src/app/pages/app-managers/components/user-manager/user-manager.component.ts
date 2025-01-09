import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { UserListResponsePagedListApiResponse, UsersService, UserFilterRequest } from 'src/open-api';

@Component({
  selector: 'app-user-manager',
  templateUrl: './user-manager.component.html',
  styleUrl: './user-manager.component.scss'
})
export class UserManagerComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  pagedUserList: UserListResponsePagedListApiResponse | null = null;

  constructor(private usersService: UsersService) {}

  ngOnInit(): void {
    this.listPagedUsers(); // Fetch paginated users
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  // Fetch paginated users
  listPagedUsers(): void {
    const userFilterRequest: UserFilterRequest = {
      pageIndex: 1, // Example: First page
      pageSize: 10, // Example: 10 items per page
      sort: 'name', // Example: Sort by user name
      search: '', // Example: No search term
      isDeleted: false // Example: Filter out deleted users
    };

    this.usersService
      .apiV1UsersListPost(userFilterRequest) // Pass the filter object
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          this.pagedUserList = response;
          console.log('Paged Users List:', response);
        },
        error: (error) => {
          console.error('Error fetching paged user list:', error);
        }
      });
  }
}