import { Injectable, Type } from "@angular/core";
import { AddNoteFormComponent } from "../components/forms/add-note-form/add-note-form.component";

@Injectable({
  providedIn: 'root'
})
export class ComponentMappingService {

  constructor() { }

  private componentMap: { [key: string]: Type<any> } = {
    'AddNoteForm': AddNoteFormComponent,
  };

  getComponent(taskType: string): Type<any> {
    // return this.componentMap[taskType];
    return AddNoteFormComponent;
    
  }
}
