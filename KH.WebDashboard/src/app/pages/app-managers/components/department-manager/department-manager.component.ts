import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { DepartmentResponseApiResponse, DepartmentListResponsePagedListApiResponse, DepartmentsService, DepartmentFilterRequest } from 'src/open-api';

@Component({
  selector: 'app-department-manager',
  templateUrl: './department-manager.component.html',
  styleUrl: './department-manager.component.scss'
})
export class DepartmentManagerComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();

  departmentDetails: DepartmentResponseApiResponse | null = null;
  pagedDepartmentList: DepartmentListResponsePagedListApiResponse | null = null;

  constructor(private departmentsService: DepartmentsService) {}

  ngOnInit(): void {
    this.getDepartmentById(1); // Fetch a department by ID
    this.listPagedDepartments(); // Fetch paginated departments
  }

  ngOnDestroy(): void {
    // Clean up subscriptions
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  // Fetch a single department by ID
  getDepartmentById(id: number): void {
    this.departmentsService
      .apiV1DepartmentsIdGet(id) // Individual parameter for ID
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          this.departmentDetails = response;
          console.log('Department Details:', response);
        },
        error: (error) => {
          console.error('Error fetching department details:', error);
        }
      });
  }

  // Fetch paginated departments
  listPagedDepartments(): void {
    // Create the DepartmentFilterRequest object
    const departmentFilterRequest: DepartmentFilterRequest = {
      pageIndex: 1, // Example: First page
      pageSize: 10, // Example: 10 items per page
      sort: 'nameEn', // Example: Sort by English name
      search: '', // Example: No search term
      isDeleted: false // Example: Filter out deleted departments
    };

    this.departmentsService
      .apiV1DepartmentsPagedListPost(departmentFilterRequest) // Pass the filter object
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          this.pagedDepartmentList = response;
          console.log('Paged Departments List:', response);
        },
        error: (error) => {
          console.error('Error fetching paged department list:', error);
        }
      });
  }
}