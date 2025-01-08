import { Injectable } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { format, parseISO, startOfDay } from 'date-fns';

@Injectable()
export class DateFnsAdapter extends DateAdapter<Date> {
  override getMonthNames(style: 'long' | 'short' | 'narrow'): string[] {
      throw new Error('Method not implemented.');
  }
  override getDateNames(): string[] {
      throw new Error('Method not implemented.');
  }
  override getDayOfWeekNames(style: 'long' | 'short' | 'narrow'): string[] {
      throw new Error('Method not implemented.');
  }
  override getYearName(date: Date): string {
      throw new Error('Method not implemented.');
  }
  override getFirstDayOfWeek(): number {
      throw new Error('Method not implemented.');
  }
  override getNumDaysInMonth(date: Date): number {
      throw new Error('Method not implemented.');
  }
  override clone(date: Date): Date {
      throw new Error('Method not implemented.');
  }
  override createDate(year: number, month: number, date: number): Date {
      throw new Error('Method not implemented.');
  }
  override addCalendarYears(date: Date, years: number): Date {
      throw new Error('Method not implemented.');
  }
  override addCalendarMonths(date: Date, months: number): Date {
      throw new Error('Method not implemented.');
  }
  override addCalendarDays(date: Date, days: number): Date {
      throw new Error('Method not implemented.');
  }
  override toIso8601(date: Date): string {
      throw new Error('Method not implemented.');
  }
  override isDateInstance(obj: any): boolean {
      throw new Error('Method not implemented.');
  }
  override isValid(date: Date): boolean {
      throw new Error('Method not implemented.');
  }
  override invalid(): Date {
      throw new Error('Method not implemented.');
  }
  getYear(date: Date): number {
    return date.getFullYear();
  }

  getMonth(date: Date): number {
    return date.getMonth();
  }

  getDate(date: Date): number {
    return date.getDate();
  }

  getDayOfWeek(date: Date): number {
    return date.getDay();
  }

  // Implement other necessary methods...
  today(): Date {
    return new Date();
  }

  parse(value: any, parseFormat: any): Date | null {
    if (typeof value === 'string') {
      return parseISO(value);
    }
    return value ? new Date(value) : null;
  }

  format(date: Date, displayFormat: any): string {
    if (displayFormat === 'input') {
      return format(date, 'MM/dd/yyyy');
    } else {
      return format(date, 'EEE, MMM d, yyyy');
    }
  }

//   addCalendarYears(date: Date, years: number): Date {
//     // Implement using date-fns
//   }

  // Implement other necessary operations like addCalendarMonths, addCalendarDays, etc.
}
