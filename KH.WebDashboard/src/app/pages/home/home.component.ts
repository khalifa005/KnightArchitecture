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


  products = [
    { name: 'Laptop', price: '$800', quantity: 10 },
    { name: 'Phone', price: '$500', quantity: 20 },
    { name: 'Tablet', price: '$300', quantity: 15 },
    // { name: 'Monitor', price: '$200', quantity: 8 },
    // { name: 'Keyboard', price: '$50', quantity: 25 },
    // { name: 'Mouse', price: '$20', quantity: 30 },
    // { name: 'Headphones', price: '$100', quantity: 12 },
    // { name: 'Speakers', price: '$150', quantity: 18 },
    // { name: 'Printer', price: '$120', quantity: 7 },
    // { name: 'Scanner', price: '$130', quantity: 6 },
    // { name: 'Router', price: '$90', quantity: 15 },
    // { name: 'Switch', price: '$70', quantity: 10 },
    // { name: 'Hard Drive', price: '$200', quantity: 14 },
    // { name: 'SSD', price: '$150', quantity: 10 },
    // { name: 'RAM', price: '$80', quantity: 12 },
    // { name: 'Graphics Card', price: '$500', quantity: 5 },
    // { name: 'Motherboard', price: '$250', quantity: 8 },
    // { name: 'Processor', price: '$300', quantity: 9 },
    // { name: 'Power Supply', price: '$100', quantity: 11 },
    // { name: 'Case', price: '$60', quantity: 20 }
  ];

  htmlContent: string = '<h3> test h3 html</h3>';
  name: string = '';

  submit() {
    console.log('Submitted name:', this.name);
    alert(`Hello, ${this.name}!`);
  }

  bellIconConfig: NbIconConfig = { icon: 'bell-outline', pack: 'eva' };
  

}

