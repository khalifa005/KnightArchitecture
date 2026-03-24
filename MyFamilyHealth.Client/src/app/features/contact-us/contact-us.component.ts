import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { TranslocoModule, TranslocoService } from '@jsverse/transloco';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-contact-us',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule, 
    TranslocoModule, 
    InputTextModule, 
    SelectModule, 
    TextareaModule, 
    ButtonModule, 
    CardModule,
    RippleModule
  ],
  template: `
    <div class="contact-us-container p-4 lg:p-6" *transloco="let t; read: 'contact-us'">
      <div class="text-center mb-6">
        <h1 class="text-4xl font-bold text-900 mb-2">{{ t('title') }}</h1>
        <p class="text-xl text-600">{{ t('subtitle') }}</p>
      </div>

      <div class="grid">
        <!-- Contact Form -->
        <div class="col-12 lg:col-8 p-3">
          <p-card class="h-full shadow-2 overflow-hidden">
            <form [formGroup]="contactForm" (ngSubmit)="onSubmit()" class="flex flex-column gap-4">
              <div class="grid">
                <div class="col-12 md:col-6 flex flex-column gap-2">
                  <label for="name" class="font-bold text-900">{{ t('form.name') }}</label>
                  <input pInputText id="name" formControlName="name" [placeholder]="t('form.name')" class="w-full" />
                </div>
                <div class="col-12 md:col-6 flex flex-column gap-2">
                  <label for="email" class="font-bold text-900">{{ t('form.email') }}</label>
                  <input pInputText id="email" type="email" formControlName="email" [placeholder]="t('form.email')" class="w-full" />
                </div>
              </div>

              <div class="flex flex-column gap-2">
                <label for="subject" class="font-bold text-900">{{ t('form.subject') }}</label>
                <p-select 
                  id="subject" 
                  formControlName="subject" 
                  [options]="subjects" 
                  optionLabel="label" 
                  optionValue="value"
                  [placeholder]="t('form.subject')"
                  appendTo="body"
                  styleClass="w-full">
                </p-select>
              </div>

              <div class="flex flex-column gap-2">
                <label for="message" class="font-bold text-900">{{ t('form.message') }}</label>
                <textarea 
                  pTextarea 
                  id="message" 
                  formControlName="message" 
                  [rows]="6" 
                  [placeholder]="t('form.message')"
                  [autoResize]="true"
                  class="w-full">
                </textarea>
              </div>

              <div class="mt-2 text-right">
                <p-button 
                  type="submit" 
                  [label]="t('form.submit')" 
                  icon="pi pi-send" 
                  [disabled]="contactForm.invalid"
                  [loading]="submitting"
                  styleClass="w-full md:w-auto p-button-lg px-6 border-round-xl">
                </p-button>
              </div>
            </form>
          </p-card>
        </div>

        <!-- Contact Info & Map -->
        <div class="col-12 lg:col-4 p-3 gap-4 flex flex-column">
          <p-card [header]="t('info.title')" class="shadow-2 h-full">
            <div class="flex flex-column gap-5 py-2">
              <div class="flex align-items-start gap-3">
                <div class="bg-primary-100 text-primary border-round-circle p-3 flex align-items-center justify-content-center" style="width: 48px; height: 48px">
                  <i class="pi pi-map-marker text-xl"></i>
                </div>
                <div>
                  <h4 class="m-0 text-900 font-bold mb-1">{{ t('info.address_label') }}</h4>
                  <p class="m-0 text-600 line-height-3">{{ t('info.address_value') }}</p>
                </div>
              </div>

              <div class="flex align-items-start gap-3">
                <div class="bg-primary-100 text-primary border-round-circle p-3 flex align-items-center justify-content-center" style="width: 48px; height: 48px">
                  <i class="pi pi-phone text-xl"></i>
                </div>
                <div>
                  <h4 class="m-0 text-900 font-bold mb-1">{{ t('info.phone_label') }}</h4>
                  <p class="m-0 text-600">{{ t('info.phone_value') }}</p>
                </div>
              </div>

              <div class="flex align-items-start gap-3">
                <div class="bg-primary-100 text-primary border-round-circle p-3 flex align-items-center justify-content-center" style="width: 48px; height: 48px">
                  <i class="pi pi-envelope text-xl"></i>
                </div>
                <div>
                  <h4 class="m-0 text-900 font-bold mb-1">{{ t('info.email_label') }}</h4>
                  <p class="m-0 text-600">{{ t('info.email_value') }}</p>
                </div>
              </div>

              <div class="flex align-items-start gap-3">
                <div class="bg-primary-100 text-primary border-round-circle p-3 flex align-items-center justify-content-center" style="width: 48px; height: 48px">
                  <i class="pi pi-clock text-xl"></i>
                </div>
                <div>
                  <h4 class="m-0 text-900 font-bold mb-1">{{ t('info.hours_label') }}</h4>
                  <p class="m-0 text-600">{{ t('info.hours_value') }}</p>
                </div>
              </div>
            </div>

            <!-- Placeholder Map UI -->
            <div class="mt-4 border-round-xl bg-emphasis overflow-hidden flex align-items-center justify-content-center relative" style="height: 180px">
               <div class="absolute w-full h-full opacity-20" style="background-image: radial-gradient(circle, var(--primary-color) 1px, transparent 1px); background-size: 20px 20px;"></div>
               <div class="z-1 text-center">
                 <i class="pi pi-map text-4xl text-primary-300 mb-2"></i>
                 <p class="m-0 text-500 text-sm">Interactive Map Simulation</p>
               </div>
            </div>
          </p-card>
        </div>
      </div>
    </div>
  `,
  styles: [`
    :host ::ng-deep .p-card {
      border-radius: 1.5rem;
    }
    :host ::ng-deep .p-inputtext, :host ::ng-deep .p-dropdown {
      border-radius: 0.75rem;
    }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ContactUsComponent {
  private fb = inject(FormBuilder);
  private transloco = inject(TranslocoService);
  
  submitting = false;

  contactForm = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    subject: [null, Validators.required],
    message: ['', [Validators.required, Validators.minLength(10)]]
  });

  subjects = [
    { label: this.transloco.translate('contact-us.form.subjects.technical'), value: 'technical' },
    { label: this.transloco.translate('contact-us.form.subjects.medical'), value: 'medical' },
    { label: this.transloco.translate('contact-us.form.subjects.billing'), value: 'billing' },
    { label: this.transloco.translate('contact-us.form.subjects.feedback'), value: 'feedback' },
    { label: this.transloco.translate('contact-us.form.subjects.other'), value: 'other' }
  ];

  onSubmit() {
    if (this.contactForm.valid) {
      this.submitting = true;
      console.log('Form Submitted', this.contactForm.value);
      
      // Simulate API call
      setTimeout(() => {
        this.submitting = false;
        this.contactForm.reset();
        // Here we would typically show a Toast message
      }, 1500);
    }
  }
}

