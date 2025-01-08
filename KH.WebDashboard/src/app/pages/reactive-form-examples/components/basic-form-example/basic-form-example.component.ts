import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { LookupResponse } from '../../../../@core/models/base/response/lookup.model';
import { BasicExampleForm } from './basic-form';

@Component({
  selector: 'app-basic-form-example',
  templateUrl: './basic-form-example.component.html',
  styleUrl: './basic-form-example.component.scss'
})
export class BasicFormExampleComponent implements OnInit {
  myForm: BasicExampleForm;

  minDate = new Date('2023-01-01');
  maxDate = new Date('2024-12-31');

  // Filtered Subcategories (for display based on parent selection)
  subCategories: LookupResponse[] = [];
  // Simulated child ID to initialize
  // initialChildId: number = 2;
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

  // Example model with initial data
  model = {
    inputControl: 'SampleInput',
    textareaControl: 'Sample textarea content',
    dateControl: new Date('2024-12-31'),
    // productCategoryControl: 1,
    subCategoryControl: 2,
    dropdownControl: 3,
    multiSelectControl: [1, 2],
    hoursControl: 12,
    minutesControl: 30,
    fileControl: [], // or pre-uploaded files array
  };
  

  constructor(private fb: FormBuilder) {
    // Initialize the form with the model
    this.myForm = new BasicExampleForm(this.fb, this.model);
  }

  ngOnInit(): void {
    // Example initialization of dropdown and multi-select controls //without using reactive form file (old approach) --will override the initalization from reactive.ts file
    this.myForm.dropdownControl.setValue(this.productCategories[0].id);

    //intialize multi select withn all items by default (old approach) --will override the initalization from reactive.ts file
    const defaultMultiSelectValues = this.productCategories.map(category => category.id);
    this.myForm.multiSelectControl.setValue(defaultMultiSelectValues);


    // Initialize form controls based on the given child ID
    if (this.model.subCategoryControl) {
      this.initializeFormWithChild(this.model.subCategoryControl);
    }

    // Listen for changes in the parent category
    this.myForm.productCategoryControl?.valueChanges.subscribe((parentId) => {
      this.updateSubCategories(parentId);
    });

  }

  onSubmit(): void {
    if (this.myForm.valid) {
      console.log('Form Submitted!', this.myForm.value);
    } else {
      console.log('Form is invalid. Please correct the errors.');
    }
  }

  onFileUpload(files: File[]): void {
    console.log('Files uploaded:', files);
  }


  initializeFormWithChild(childId: number): void {
    // Find the child
    const child = this.allSubCategories.find((sub) => sub.id === childId);
    if (!child) {
      console.error('Child ID not found:', childId);
      return;
    }

    // Find the parent based on the child's parentId
    const parentId = child.parentId;
    if (!parentId) {
      console.error('Parent ID not found for child:', childId);
      return;
    }

    // Set parent control value
    this.myForm.productCategoryControl?.setValue(parentId);

    // Update subcategories for the selected parent and set the child control value
    this.updateSubCategories(parentId, childId);
  }

  updateSubCategories(parentId: number, selectedChildId?: number): void {
    // Filter subcategories based on parentId
    this.subCategories = this.allSubCategories.filter((sub) => sub.parentId === parentId);

    if (selectedChildId) {
      // Set the child control value
      this.myForm.subCategoryControl?.setValue(selectedChildId);
    } else if (this.subCategories.length > 0) {
      // Set default value for subcategory if no specific child ID is provided
      // this.myForm.get('subCategoryControl')?.setValue(this.subCategories[0].id);
    } else {
      // Reset child control if no subcategories are available
      this.myForm.subCategoryControl?.reset();
    }
  }

}