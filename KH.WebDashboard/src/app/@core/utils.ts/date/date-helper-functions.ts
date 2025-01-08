
import { formatDate } from "@angular/common";
import { MatDateFormats, NativeDateAdapter } from "@angular/material/core";
import { format } from 'date-fns';

// https://material.angular.io/components/datepicker/overview#choosing-a-date-implementation-and-date-format-settings
export function fixDatesInModel(model: any): any {
  const fixedModel = { ...model }; // Create a shallow copy of the model

  for (const key in fixedModel) {
    if (fixedModel.hasOwnProperty(key) && isDate(fixedModel[key])) {
      fixedModel[key] = fixDate(fixedModel[key]);
    }
  }

  return fixedModel;
}

// Check if a value is a date
function isDate(value: any): boolean {
  if(value != 'null' && value != null && value instanceof Date){
return true;
  }
  return false;
}

// Fix the date to local date string
function fixDate(date: any): string {


  const month = (date.getMonth() + 1).toString().padStart(2, '0');
const day = date.getDate().toString().padStart(2, '0');
const year = date.getFullYear();

const dateStr = `${day}/${month}/${year}`;

  const dateObj = new Date(date);
  let finalSS =  dateObj.toISOString();
  let finalSSsss =  dateObj.toLocaleString('en-US', { timeZone: 'Asia/Riyadh' });
  let finalSSsssddd =  dateObj.toLocaleString();
  let finasl =  finalSSsssddd.split('T')[0];

  let utcc =  dateObj.getUTCDate();
  let utccs =  dateObj.toUTCString();

  return finasl;
}

export function FormatDateForBackend(executionDate: Date): string {

  // Create a new date object to avoid mutating the original date
  const newDate = new Date(executionDate);


  return format(newDate, 'dd/MM/yyyy HH:mm:ss');
}

export function FormatDateTimeForBackend(executionDate: Date, executionTime:string): string {

    const timeParts = executionTime.split(':').map(Number);
    const hours = timeParts[0];
    const minutes = timeParts[1];
    const seconds = timeParts[2];
  
  // Create a new date object to avoid mutating the original date
  const newDate = new Date(executionDate);

  // Add the time parts to the new date
  newDate.setHours(newDate.getHours() + hours);
  newDate.setMinutes(newDate.getMinutes() + minutes);
  newDate.setSeconds(newDate.getSeconds() + seconds);

  return format(newDate, 'dd/MM/yyyy HH:mm:ss');
}


//for mat datepicker formly
export const MY_DATE_FORMATS: MatDateFormats = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'DD/MM/YYYY',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};


export const CUSTOM_DATE_FORMAT = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMMM YYYY',
    dateA11yLabel: 'DD/MM/YYYY',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};
export const MY_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

export const PICK_FORMATS = {
  parse: {dateInput: {month: 'short', year: 'numeric', day: 'numeric'}},
  display: {
      dateInput: 'input',
      monthYearLabel: {year: 'numeric', month: 'short'},
      dateA11yLabel: {year: 'numeric', month: 'long', day: 'numeric'},
      monthYearA11yLabel: {year: 'numeric', month: 'long'}
  }
};

export class PickDateAdapterXXX extends NativeDateAdapter {
  override format(date: Date, displayFormat: Object): string {
      if (displayFormat === 'input') {
          return formatDate(date,'dd-MMM-yyyy',this.locale);;
      } else {
          return date.toDateString();
      }
  }
}