import { AfterViewInit, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { NbIconConfig } from '@nebular/theme';
import { Config } from 'ngx-easy-table';
import { apiColumns } from '../../@core/fakeApiData/ApiColumns';
import { apiDataItems } from '../../@core/fakeApiData/ApiDataItems';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit, AfterViewInit {

  constructor(private readonly cdr: ChangeDetectorRef) {
  }

  ngOnInit(): void {   
  }

  ngAfterViewInit(): void {

  }


  htmlContent: string = '<h3> test h3 html</h3>';


  bellIconConfig: NbIconConfig = { icon: 'bell-outline', pack: 'eva' };
  

}

