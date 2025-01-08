import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { LookupResponse } from '../../../../@core/models/base/response/lookup.model';
import { AdvancedExampleForm } from './advanced-form-example';

@Component({
  selector: 'app-advanced-form-example',
  templateUrl: './advanced-form-example.component.html',
  styleUrl: './advanced-form-example.component.scss'
})
export class AdvancedFormExampleComponent implements OnInit {
  myForm: AdvancedExampleForm;

  minDate = new Date('2023-01-01');
  maxDate = new Date('2024-12-31');

  // Product Categories (Parent)
  productCategories: LookupResponse[] = [
    { id: 1, nameEn: 'Electronics', nameAr: 'الإلكترونيات' },
    { id: 2, nameEn: 'Clothing', nameAr: 'الملابس' },
    { id: 3, nameEn: 'Home Appliances', nameAr: 'الأجهزة المنزلية' },
  ];
  // Subcategories (Child)
  allSubCategories: LookupResponse[] = [
    { id: 1, parentId: 1, nameEn: 'Mobile Phones', nameAr: 'الهواتف المحمولة' },
    { id: 2, parentId: 1, nameEn: 'Laptops', nameAr: 'الحواسيب المحمولة' },
    { id: 3, parentId: 2, nameEn: 'Men’s Wear', nameAr: 'ملابس الرجال' },
    { id: 4, parentId: 2, nameEn: 'Women’s Wear', nameAr: 'ملابس النساء' },
    { id: 5, parentId: 3, nameEn: 'Refrigerators', nameAr: 'الثلاجات' },
    { id: 6, parentId: 3, nameEn: 'Washing Machines', nameAr: 'الغسالات' },
  ];
  // Filtered Subcategories (for display based on parent selection)
  subCategories: LookupResponse[] = [];
  
  // Example model with initial data
  model = {
    inputControl: 'SampleInput',
    textareaControl: 'Sample textarea content',
    dateControl: new Date('2024-12-31'),
    subCategoryControl: 2,
    dropdownControl: 3,
    multiSelectControl: [1, 2],
    hoursControl: 12,
    minutesControl: 30,
    fileControl: [], // or pre-uploaded files array
    workingDays: [
      { workingDate: new Date('2024-11-01'), fromWorkingHour: 9, toWorkingHour: 15 },
      { workingDate: new Date('2024-11-02'), fromWorkingHour: 11, toWorkingHour: 21 },
    ],
  };
  
  constructor(private fb: FormBuilder) {
    // Initialize the form with the model
    this.myForm = new AdvancedExampleForm(this.fb, this.model, this.allSubCategories);
  }

  ngOnInit(): void {
  // Subscribe to subCategories$ to update subCategories for the template
  this.myForm.subCategories$.subscribe((categories) => {
    this.subCategories = categories;
  });

  }

  // Method to add a new working day
  onAddWorkingDay(): void {
    this.myForm.addWorkingDay();
  }

  onRemoveWorkingDay(index: any): void {
    this.myForm.removeWorkingDay(index);

  }

  
  onFileUpload(files: File[]): void {
    console.log('Files uploaded:', files);
  }

  onSubmit(): void {
    if (this.myForm.valid) {
      console.log('Form Submitted!', this.myForm.value);
    } else {
      console.log('Form is invalid. Please correct the errors.');
    }
  }



  get totalWorkingDaysCount(): number {
    return this.myForm.workingDaysArray.length;
  }
  
  get totalFromWorkingHours(): number {
    return this.myForm.workingDaysArray.controls.reduce((sum, group) => {
      const fromHour = group.get('fromWorkingHour')?.value || 0;
      return sum + fromHour;
    }, 0);
  }

  
}