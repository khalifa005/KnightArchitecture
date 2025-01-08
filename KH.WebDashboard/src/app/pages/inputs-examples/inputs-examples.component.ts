import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { LookupResponse } from '../../@core/models/base/response/lookup.model';
import { NbComponentSize } from '@nebular/theme';
import { valueGreaterThanZeroValidator } from '../../@core/ngx-formly/validations/value-greater-than-zero-validator';
import { fileExtensionsValidator, maxFilesValidator, requiredFileUploadValidationSizeInMB, RquiredFileUploadValidationSizeInMB } from '../../@core/utils.ts/form/validations/form.validation-helpers';

@Component({
  selector: 'app-inputs-examples',
  templateUrl: './inputs-examples.component.html',
  styleUrl: './inputs-examples.component.scss'
})
export class InputsExamplesComponent implements OnInit {
  allowedExtensions: string[] = ['csv', 'xlsx', 'png', 'jpg', 'pdf', 'mp4'];
  standAloneInputRegExp: RegExp = /^[a-zA-Z\s]*$/;
  inputControlRegExp: RegExp = /^[a-zA-Z0-9]*$/; // Allows only alphanumeric characters

  myForm: FormGroup;
  selectedValue: string = '';
  textareaValue: string = '';
  dateValue: Date = new Date();
  // Pre-calculated min and max dates for the DatePicker
  minDate: Date = new Date('2023-01-01');
  maxDate: Date = new Date('2024-12-31');

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
  // Simulated child ID to initialize
  initialChildId: number = 2;
  
  selectedDropdownValue: number = -1;

  selectedMultiSelectItems: number[] = []; // For standalone multi-select example
  
  selectedHours: number = 12; // Default for standalone hours dropdown
  selectedMinutes: number = 30; // Default for standalone minutes dropdown

  preUploadedFiles = [
    { name: 'document1.pdf', url: 'https://example.com/document1.pdf' },
    { name: 'image1.png', url: 'https://example.com/image1.png' }
  ];

  
  constructor() {
    this.myForm = new FormGroup({
      inputControl: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20),
        this.regexValidator(this.inputControlRegExp) // Add regex validation
      ]),
      textareaControl: new FormControl('', [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(200),
      ]),
      dateControl: new FormControl('', [
        Validators.required,
        this.dateRangeValidator.bind(this),
      ]),
      productCategoryControl: new FormControl(null, Validators.required), // Parent
      subCategoryControl: new FormControl(null, Validators.required), // Child
      dropdownControl: new FormControl(null, [Validators.required, valueGreaterThanZeroValidator]), // Reactive form dropdown
      multiSelectControl: new FormControl([], Validators.required), // Reactive form multi-select
      hoursControl: new FormControl(null, [Validators.required, valueGreaterThanZeroValidator]), // Reactive form hours dropdown
      minutesControl: new FormControl(null, Validators.required), // Reactive form minutes dropdown

      // fileControl: new FormControl(null, Validators.required), // Reactive form file uploader
      // fileControl: new FormControl(null, [fileExtensionsValidator(this.allowedExtensions), RquiredFileUploadValidationSizeInMB(20), Validators.required]), // Reactive form file uploader
// Reactive form file uploader with corrected validators
fileControl: new FormControl([], [ fileExtensionsValidator(this.allowedExtensions), maxFilesValidator(2), requiredFileUploadValidationSizeInMB(20), Validators.required]),
    
});
  }

  ngOnInit(): void {
    
    // Listen to value changes on the autoCompleteControl
    this.myForm.get('autoCompleteControl')?.valueChanges.subscribe((value) => {
      console.log('Auto-complete value changed:', value);
      // Perform actions based on the value
    });
    
    // Listen to value changes on the fileControl
    this.myForm.get('fileControl')?.valueChanges.subscribe((value) => {
      console.log('fileControl value changed:', value);
      // Perform actions based on the value
    });


     // Initialize form controls based on the given child ID
     if (this.initialChildId) {
      this.initializeFormWithChild(this.initialChildId);
    }

    // Listen for changes in the parent category
    this.myForm.get('productCategoryControl')?.valueChanges.subscribe((parentId) => {
      this.updateSubCategories(parentId);
    });


    this.myForm.get('dropdownControl')?.setValue(this.productCategories[0].id);
    this.selectedDropdownValue = this.productCategories[0].id; // Standalone example default

     // Initialize multi-select with all options selected for reactive form
     const defaultSelectedIds = this.productCategories.map((item) => item.id);
     this.myForm.get('multiSelectControl')?.setValue(defaultSelectedIds);
 
     // Initialize multi-select for standalone example
     this.selectedMultiSelectItems = [...defaultSelectedIds];

      // Initialize hours and minutes with default values for reactive form
    this.myForm.get('hoursControl')?.setValue(12); // Default hours
    this.myForm.get('minutesControl')?.setValue(30); // Default minutes
  }

  updateSubCategoriesx(parentId: number): void {
    if (!parentId) {
      this.subCategories = [];
      this.myForm.get('subCategoryControl')?.reset(); // Reset child control
      return;
    }
    this.subCategories = this.allSubCategories.filter((sub) => sub.parentId === parentId);
    this.myForm.get('subCategoryControl')?.reset(); // Reset child control when parent changes
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
    this.myForm.get('productCategoryControl')?.setValue(parentId);

    // Update subcategories for the selected parent and set the child control value
    this.updateSubCategories(parentId, childId);
  }

  updateSubCategories(parentId: number, selectedChildId?: number): void {
    // Filter subcategories based on parentId
    this.subCategories = this.allSubCategories.filter((sub) => sub.parentId === parentId);

    if (selectedChildId) {
      // Set the child control value
      this.myForm.get('subCategoryControl')?.setValue(selectedChildId);
    } else if (this.subCategories.length > 0) {
      // Set default value for subcategory if no specific child ID is provided
      // this.myForm.get('subCategoryControl')?.setValue(this.subCategories[0].id);
    } else {
      // Reset child control if no subcategories are available
      this.myForm.get('subCategoryControl')?.reset();
    }
  }

  regexValidator(pattern: RegExp) {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null; // Don't validate if the value is empty
      }
      return pattern.test(control.value) ? null : { regex: { requiredPattern: pattern.toString() } };
    };
  }

  dateRangeValidator(control: FormControl): { [key: string]: any } | null {
    const date = new Date(control.value);
    if (date < this.minDate) {
      return { tooEarly: true };
    }
    if (date > this.maxDate) {
      return { tooLate: true };
    }
    return null;
  }

  onInputChange(newValue: string) {
    console.log('Input Value changed:', newValue);
  }

  onTextareaChange(newValue: string) {
    console.log('Textarea Value changed:', newValue);
  }

  onDateChange(newDate: Date) {
    console.log('Date Value changed:', newDate);
  }

  onAutoCompleteChange(newValue: number) {

    console.log('AutoComplete Value changed:', newValue);
  }

  onDropdownItemChange(newValue: number): void {
    console.log('Dropdown value changed:', newValue);
  }

  onMultiSelectChange(selectedItems: LookupResponse[]): void {
    // console.log('Multi-Select value changed:', selectedItems);
  }

  onHoursChange(selectedHour: number): void {
    console.log('Selected hour:', selectedHour);
  }

  onMinutesChange(selectedMinute: number): void {
    console.log('Selected minute:', selectedMinute);
  }

  standaloneFiles: File[] = []; // For standalone file uploader
  
  onFileDrop(files: File[]): void {
    console.log('Files dropped:', files);
    this.standaloneFiles = files; // Update standalone files
  }

  onOldFileDrop(files: any): void {
    console.log('old Files dropped:', files);
    
  }

  uploadedFiles: File[] = [];

  
}