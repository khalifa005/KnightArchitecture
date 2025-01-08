import { Component } from '@angular/core';
import { FieldType } from '@ngx-formly/material';
import { FieldTypeConfig } from '@ngx-formly/core';

// import { FieldType } from '@ngx-formly/core';
// import { FieldType } from '@ngx-formly/material';
@Component({
  selector: 'app-formly-date',
  templateUrl: './formly-date.component.html',
  styleUrl: './formly-date.component.scss'
})
export class FormlyDateComponent  extends FieldType<FieldTypeConfig> {

  // check
  // UtcDateInterceptor

  
//selected 03-12-2024
//and when i log it in consol
//Tue Dec 03 2024 00:00:00 GMT+0300 (Arabian Standard Time)


// onDateChange(date: Date) {
//   const timeZone = 'Asia/Riyadh'; // Replace with your desired timezone
//   const zonedDate = utcToZonedTime(date, timeZone);
//   const formattedDate = format(zonedDate, 'yyyy-MM-dd'); // Format for backend
//   console.log('Zoned Date:', formattedDate);
//   this.formControl.setValue(formattedDate);
// }

}
