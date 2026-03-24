import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@jsverse/transloco';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { MultiSelectModule } from 'primeng/multiselect';
import { ProgressBarModule } from 'primeng/progressbar';
import { TooltipModule } from 'primeng/tooltip';
import { DividerModule } from 'primeng/divider';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-clinical-report',
  standalone: true,
  imports: [
    CommonModule, 
    TranslocoModule, 
    CardModule, 
    ButtonModule, 
    SelectModule, 
    DatePickerModule, 
    MultiSelectModule, 
    ProgressBarModule, 
    TooltipModule,
    DividerModule,
    FormsModule
  ],
  template: `
    <div class="clinical-report-container p-4 lg:p-6" *transloco="let t; read: 'clinical-report'">
      <div class="mb-6">
        <h1 class="text-3xl font-bold text-900 m-0">{{ t('title') }}</h1>
        <p class="text-600 m-0 mt-1">{{ t('subtitle') }}</p>
      </div>

      <div class="grid">
        <!-- Configuration Sidebar -->
        <div class="col-12 lg:col-4 p-2">
          <p-card [header]="t('config.title')" class="shadow-2 mb-4">
             <div class="flex flex-column gap-4">
                <div class="flex flex-column gap-2">
                  <label class="font-bold text-900">{{ t('config.profile') }}</label>
                  <p-select [options]="profiles" optionLabel="label" [placeholder]="t('config.profile')" styleClass="w-full"></p-select>
                </div>
                
                <div class="flex flex-column gap-2">
                  <label class="font-bold text-900">{{ t('config.range') }}</label>
                  <p-datePicker selectionMode="range" [readonlyInput]="true" [placeholder]="t('config.range')" styleClass="w-full"></p-datePicker>
                </div>

                <div class="flex flex-column gap-2">
                  <label class="font-bold text-900">{{ t('config.scope') }}</label>
                  <p-multiSelect [options]="scopes" [placeholder]="t('config.scope')" styleClass="w-full" display="chip"></p-multiSelect>
                </div>

                <p-button [label]="t('config.generate')" icon="pi pi-cog" styleClass="w-full py-3 border-round-xl mt-2"></p-button>
             </div>
          </p-card>

          <!-- AI Audio Summary Simulation -->
          <p-card [header]="t('ai_section.title')" class="shadow-2 bg-primary text-white border-round-3xl overflow-hidden relative">
            <div class="absolute w-full h-full left-0 top-0 opacity-10 pointer-events-none" style="background-image: repeating-linear-gradient(45deg, #fff 0, #fff 1px, transparent 0, transparent 50%); background-size: 10px 10px;"></div>
            <div class="flex flex-column align-items-center py-2 relative z-1">
               <div class="audio-wave flex align-items-center gap-1 mb-4 h-3rem">
                  @for(i of [1,2,3,4,5,6,7,8]; track i) {
                    <div class="wave-bar bg-white flex-1" [ngStyle]="{'height': isPlaying() ? (10 + (i * 5)) + '%' : '10%'}"></div>
                  }
               </div>
               <p-button 
                 [label]="isPlaying() ? t('ai_section.playing') : t('ai_section.audio_summary')" 
                 [icon]="isPlaying() ? 'pi pi-pause' : 'pi pi-play'" 
                 rounded="true" 
                 severity="secondary"
                 (onClick)="toggleAudio()">
               </p-button>
            </div>
          </p-card>
        </div>

        <!-- Report Preview Area -->
        <div class="col-12 lg:col-8 p-2">
          <p-card class="shadow-2 min-h-full flex flex-column border-round-3xl">
             <div class="flex align-items-center justify-content-between mb-4">
                <h2 class="text-2xl font-bold text-900 m-0">{{ t('preview.title') }}</h2>
                <div class="flex gap-2">
                   <p-button icon="pi pi-print" [rounded]="true" [text]="true" severity="secondary" [pTooltip]="t('export.print')"></p-button>
                   <p-button icon="pi pi-file-pdf" [rounded]="true" [text]="true" severity="danger" [pTooltip]="t('export.pdf')"></p-button>
                   <p-button icon="pi pi-envelope" [rounded]="true" [text]="true" severity="primary" [pTooltip]="t('export.email')"></p-button>
                </div>
             </div>

             <div class="report-content border-1 border-200 border-round-xl p-5 bg-emphasis flex-1">
                <div class="flex justify-content-between border-bottom-1 border-100 pb-4 mb-4">
                   <div class="flex flex-column">
                      <span class="text-sm font-bold text-primary mb-1 uppercase tracking-wider">DHM Clinical Report</span>
                      <span class="text-2xl font-black text-900">Khalifa Mohamed</span>
                   </div>
                   <div class="text-right flex flex-column">
                      <span class="text-sm text-500">{{ t('preview.generated') }}</span>
                      <span class="font-medium text-900">Oct 24, 2024</span>
                   </div>
                </div>

                <div class="grid mb-6">
                   <div class="col-6 md:col-3">
                      <span class="block text-500 text-xs font-bold mb-1 uppercase">Age</span>
                      <span class="text-900 font-bold">34 Years</span>
                   </div>
                   <div class="col-6 md:col-3">
                      <span class="block text-500 text-xs font-bold mb-1 uppercase">Blood</span>
                      <span class="text-900 font-bold">A+ Positive</span>
                   </div>
                   <div class="col-6 md:col-3">
                      <span class="block text-500 text-xs font-bold mb-1 uppercase">Weight</span>
                      <span class="text-900 font-bold">78.5 kg</span>
                   </div>
                   <div class="col-6 md:col-3">
                      <span class="block text-500 text-xs font-bold mb-1 uppercase">Height</span>
                      <span class="text-900 font-bold">179 cm</span>
                   </div>
                </div>

                <h3 class="text-xl font-bold text-900 mb-3 flex align-items-center gap-2">
                   <i class="pi pi-sparkles text-primary"></i>
                   {{ t('preview.summary') }}
                </h3>
                <p class="text-700 line-height-3 italic bg-white p-4 border-round-xl border-left-3 border-primary shadow-1">
                   "Patient displays excellent adherence to active medication DHM-24. Cardiovascular metrics remain within peak range (+/- 2% dev). Recommendation: Continue current regimen and upload next lab result by Nov 15th for comprehensive quarterly analysis."
                </p>

                <p-divider></p-divider>

                <div class="mt-4">
                   <span class="block text-900 font-bold mb-4 uppercase text-xs tracking-widest">Recent Activity Trend</span>
                   <div class="flex align-items-end gap-2 h-8rem px-2">
                      @for(val of [40, 65, 55, 80, 70, 95, 85, 100]; track val) {
                        <div class="flex-1 bg-primary-100 border-round-top-md hover:bg-primary transition-colors cursor-help" [style.height.%]="val" [pTooltip]="val + '% Activity'"></div>
                      }
                   </div>
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
    .wave-bar {
      width: 4px;
      border-radius: 2px;
      transition: height 0.3s ease;
    }
    .audio-wave {
      display: flex;
      align-items: center;
    }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ClinicalReportComponent {
  profiles = [
    { label: 'Khalifa Mohamed (Self)', value: 'self' },
    { label: 'Sarah Khalifa (Wife)', value: 'sarah' }
  ];

  scopes = [
    { label: 'Medical Records', value: 'records' },
    { label: 'Medications', value: 'meds' },
    { label: 'Vital Signs', value: 'vitals' },
    { label: 'AI Insights', value: 'ai' }
  ];

  isPlaying = signal(false);

  toggleAudio() {
    this.isPlaying.update(v => !v);
  }
}

