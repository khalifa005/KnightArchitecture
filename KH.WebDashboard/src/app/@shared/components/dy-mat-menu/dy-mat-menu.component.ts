import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { MatMenu } from '@angular/material/menu';
import { Router } from '@angular/router';
import { NbWindowControlButtonsConfig, NbWindowService } from '@nebular/theme';
import { Logger } from '../../../@core/utils.ts/logger.service';
import { NavItem } from '../../../@core/interfaces/nav-item';
import { ComponentMappingService } from '../../services/component-mapping.service';
import { PopUpWindowTypes } from '../../../@core/const/popup-window-types';

  @Component({
  selector: 'app-dy-mat-menu',
  templateUrl: './dy-mat-menu.component.html',
  styleUrls: ['./dy-mat-menu.component.scss']
})

export class DyMatMenuComponent implements OnInit, OnChanges {
  private log = new Logger(DyMatMenuComponent.name);

  @Input() items: NavItem[];
  @Input() myDataObject!: any;
  @Input() selectedItemsIds!: any[];
  @Output() contextItemClickedEvent = new EventEmitter<boolean>();

  @ViewChild("menu", {static: true}) menu: MatMenu;
  showMenuItems = true;

  @ViewChild("childMenu", {static: true}) childMenu: MatMenu;

  minimize = true;
  maximize = false;
  fullScreen = true;
  close = true;


  constructor(
    public router: Router,
    private componentMappingService: ComponentMappingService,
     private windowService: NbWindowService) {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['myDataObject'] && changes['myDataObject'].currentValue) {
      const newValue = changes['myDataObject'].currentValue.id;
      // this.log.info(`Selected Menu Item Id is: ${newValue}`);
    }
  }
  


  handleCustomEvent(isClicked:boolean) {
    // this.log.info(isClicked);
    this.contextItemClickedEvent.emit(true);
  }

  openDynamicFormDialog(data:any, menuItem:any){
    this.showMenuItems = false;
    this.contextItemClickedEvent.emit(true);
    
    const buttonsConfig: NbWindowControlButtonsConfig = {
      minimize: this.minimize,
      maximize: this.maximize,
      fullScreen: this.fullScreen,
      close: this.close,
    };
    
    console.log("table row data from context data")
    console.log(data);


    let component = this.componentMappingService.getComponent(menuItem.route);

    if(!component){
      component = this.componentMappingService.getComponent("other");
    }

    let windowCssClass =  (menuItem.route == "NEWMOREINFO" || menuItem.route == 'ALLOCATETASK'|| menuItem.windowSize == PopUpWindowTypes.ExtraLarge) ? PopUpWindowTypes.ExtraLarge : PopUpWindowTypes.ExtraLarge; 
    windowCssClass =  menuItem.windowSize == PopUpWindowTypes.FullScreen ? PopUpWindowTypes.FullScreen : windowCssClass; 
    
    const windowTitle = data.TASKCODE ?? "data.taskcode";
    let test = this.windowService.open(
      component,
        {
          // title: menuItem.displayName +' # ' + windowTitle  ,
          title: 'Window id ' + data.id,
          // hasBackdrop: true,
          // closeOnEsc:true,
          buttons: buttonsConfig,
          windowClass: windowCssClass,
          context: 
          {
            passedRowData: data,
            isOpenedFromContext: true,
             menuItem:menuItem
            } 
        }
      )
      .onClose.subscribe(response => {
        if(response === 200){
          // call api this.getData();
        }
      });

  }
  
  openMenu(event: MouseEvent, menuItem: any) {
    this.log .info("custom actions openMenu menuItem  is "+ menuItem);
    this.log .info("custom actions this.myDataObject  is "+ this.myDataObject);

    this.openDynamicFormDialog(this.myDataObject, menuItem);
  
  }

  ngOnInit() {
    // console.log("mat-menu-init");
  }
}
