import { AfterViewInit, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Config } from 'ngx-easy-table';
import { apiColumns } from '../../@core/fakeApiData/ApiColumns';
import { apiDataItems } from '../../@core/fakeApiData/ApiDataItems';

@Component({
  selector: 'app-ngx-dy-table',
  templateUrl: './ngx-dy-table.component.html',
  styleUrl: './ngx-dy-table.component.scss'
})
export class NgxDyTableComponent implements OnInit, AfterViewInit {

  public configuration: Config;
  public apiCustomColumns: any[];
  public apiData: any[];

  paginationParam: any = {
    limit: 5,
    offset: 0,
    count: -1,
    sortColumnKey: '',
    sortOrder: ''
  };

  constructor(private readonly cdr: ChangeDetectorRef) {
  }

  ngOnInit(): void {
    this.apiData = apiDataItems;
    this.apiCustomColumns = apiColumns;
  }

  ngAfterViewInit(): void {

  }

  whenPagingAndFilterChange(event: any): void {
    console.log('table event change' + event)
  }


}
