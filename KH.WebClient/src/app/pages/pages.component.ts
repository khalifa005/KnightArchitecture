import { Component, OnInit, OnDestroy } from "@angular/core";

@Component({
  selector: 'app-pages',
  templateUrl: './pages.component.html',
  styleUrls: ['./pages.component.css']
})
export class PagesComponent implements OnInit, OnDestroy {
  isCollapsed = false;
  ngOnDestroy(): void {
  }
  ngOnInit(): void {
  }


}

