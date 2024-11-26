
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule } from "@angular/forms";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NzLayoutModule } from "ng-zorro-antd/layout";
import { NzMenuModule } from "ng-zorro-antd/menu";
import { BrowserModule } from "@angular/platform-browser";
import { PagesComponent } from "./pages.component";
import { RouterModule, Routes } from "@angular/router";
import { NzIconModule } from "ng-zorro-antd/icon";
import { PagesRoutingModule } from "./pages-routing.module";

@NgModule({
  declarations: [
    PagesComponent
  ],
  imports: [
    CommonModule,
    PagesRoutingModule,
    
    NzLayoutModule,
    NzMenuModule,

    FormsModule,
    NzLayoutModule,
    NzMenuModule,
    NzIconModule,

    // NzButtonModule,
    // NzDropDownModule,
    // NzBreadCrumbModule,
  ],
  providers: [
    //  { provide: NZ_I18N, useValue: ar_EG }
    ],
})
export class PagesModule { }
