import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { DepartmentResponseApiResponse, DepartmentListResponsePagedListApiResponse, DepartmentsService, DepartmentFilterRequest } from 'src/open-api';

@Component({
  selector: 'app-department-manager',
  templateUrl: './department-manager.component.html',
  styleUrl: './department-manager.component.scss'
})
export class DepartmentManagerComponent implements OnInit, OnDestroy {

  constructor() {}

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
  }




}