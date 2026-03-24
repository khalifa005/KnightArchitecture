import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@jsverse/transloco';
import { CardModule } from 'primeng/card';
import { ProgressBarModule } from 'primeng/progressbar';
import { MeterGroupModule } from 'primeng/metergroup';
import { ButtonModule } from 'primeng/button';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { SelectModule } from 'primeng/select';
import { TooltipModule } from 'primeng/tooltip';

@Component({
  selector: 'app-admin-analytics',
  standalone: true,
  imports: [
    CommonModule, 
    TranslocoModule, 
    CardModule, 
    ProgressBarModule, 
    MeterGroupModule,
    ButtonModule,
    IconFieldModule,
    InputIconModule,
    SelectModule,
    TooltipModule
  ],
  template: `
    <div class="analytics-container p-4 lg:p-6" *transloco="let t; read: 'admin-analytics'">
      <div class="flex flex-column md:flex-row md:align-items-center justify-content-between mb-6 gap-4">
        <div>
          <h1 class="text-3xl font-bold text-900 m-0">{{ t('title') }}</h1>
          <p class="text-600 m-0 mt-1">{{ t('subtitle') }}</p>
        </div>
        <div class="flex gap-2">
          <p-button icon="pi pi-download" label="Export PDF" severity="secondary" rounded="true" size="small"></p-button>
          <p-button icon="pi pi-calendar" label="Last 30 Days" severity="primary" rounded="true" size="small"></p-button>
        </div>
      </div>

      <!-- Quick Stats -->
      <div class="grid mb-6">
        <div class="col-12 md:col-6 lg:col-3 p-2">
          <p-card class="shadow-2 h-full">
            <div class="flex flex-column">
               <span class="text-500 font-bold mb-2 uppercase text-xs">{{ t('metrics.active_users') }}</span>
               <div class="flex align-items-center justify-content-between">
                 <span class="text-3xl font-bold text-900">4,210</span>
                 <span class="p-1 px-2 border-round-lg bg-green-100 text-green-700 text-xs font-bold">+18%</span>
               </div>
               <div class="mt-3">
                 <p-progressBar [value]="65" [showValue]="false" styleClass="h-0.5rem"></p-progressBar>
               </div>
            </div>
          </p-card>
        </div>
        <div class="col-12 md:col-6 lg:col-3 p-2">
          <p-card class="shadow-2 h-full">
            <div class="flex flex-column">
               <span class="text-500 font-bold mb-2 uppercase text-xs">{{ t('metrics.ai_accuracy') }}</span>
               <div class="flex align-items-center justify-content-between">
                 <span class="text-3xl font-bold text-900">98.4%</span>
                 <span class="p-1 px-2 border-round-lg bg-primary-100 text-primary text-xs font-bold">STABLE</span>
               </div>
               <div class="mt-3">
                 <p-progressBar [value]="98" [showValue]="false" severity="success" styleClass="h-0.5rem"></p-progressBar>
               </div>
            </div>
          </p-card>
        </div>
        <div class="col-12 md:col-6 lg:col-3 p-2">
          <p-card class="shadow-2 h-full">
            <div class="flex flex-column">
               <span class="text-500 font-bold mb-2 uppercase text-xs">{{ t('metrics.system_health') }}</span>
               <div class="flex align-items-center justify-content-between">
                 <span class="text-3xl font-bold text-900">99.9%</span>
                 <span class="p-1 px-2 border-round-lg bg-green-100 text-green-700 text-xs font-bold">UP</span>
               </div>
               <div class="mt-3">
                 <p-progressBar [value]="99" [showValue]="false" severity="info" styleClass="h-0.5rem"></p-progressBar>
               </div>
            </div>
          </p-card>
        </div>
        <div class="col-12 md:col-6 lg:col-3 p-2">
          <p-card class="shadow-2 h-full">
            <div class="flex flex-column">
               <span class="text-500 font-bold mb-2 uppercase text-xs">API Latency</span>
               <div class="flex align-items-center justify-content-between">
                 <span class="text-3xl font-bold text-900">124ms</span>
                 <span class="p-1 px-2 border-round-lg bg-orange-100 text-orange-700 text-xs font-bold">-5%</span>
               </div>
               <div class="mt-3">
                 <p-progressBar [value]="45" [showValue]="false" severity="warn" styleClass="h-0.5rem"></p-progressBar>
               </div>
            </div>
          </p-card>
        </div>
      </div>

      <!-- Detailed Metrics -->
      <div class="grid">
        <div class="col-12 lg:col-8 p-2">
          <p-card [header]="t('charts.accuracy_label')" class="shadow-2 h-full border-round-xl">
             <div class="mt-4">
                <p-meterGroup [value]="aiMetrics" labelPosition="start"></p-meterGroup>
             </div>
             
             <div class="mt-6 flex flex-column gap-4">
                <div *ngFor="let item of aiMetrics" class="flex flex-column gap-2">
                   <div class="flex justify-content-between align-items-center">
                     <span class="font-bold text-900">{{ t('categories.' + item.label) }}</span>
                     <span class="text-sm font-medium text-600">{{ item.value }}% Confidence</span>
                   </div>
                   <p-progressBar [value]="item.value" [showValue]="false" [color]="item.color" styleClass="h-0.75rem border-round-lg"></p-progressBar>
                </div>
             </div>
          </p-card>
        </div>

        <div class="col-12 lg:col-4 p-2">
          <p-card [header]="t('charts.health_label')" class="shadow-2 h-full border-round-xl">
             <div class="flex flex-column gap-5 mt-4">
                @for(region of ['riyadh', 'jeddah', 'dammam']; track region) {
                   <div class="flex flex-column gap-2">
                      <div class="flex justify-content-between align-items-center">
                        <span class="font-medium text-800">{{ t('regions.' + region) }}</span>
                        <span class="text-green-600 font-bold">100% UP</span>
                      </div>
                      <div class="flex gap-1">
                        @for(i of [1,2,3,4,5,6,7,8,9,10,11,12]; track i) {
                           <div class="flex-1 bg-green-500 h-1.5rem border-round-sm" pTooltip="Uptime Check #{{i}}"></div>
                        }
                      </div>
                   </div>
                }
             </div>
             
             <div class="mt-6 p-4 border-round-xl bg-primary-reverse border-1 border-primary-100">
                <h4 class="m-0 text-primary-800 font-bold mb-2">AI Node Distribution</h4>
                <p class="m-0 text-primary-600 text-sm line-height-3">Global health processing nodes are currently operating at 45% capacity across 12 zones.</p>
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
    :host ::ng-deep .p-progressbar-value {
      transition: width 1s ease-in-out;
    }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminAnalyticsComponent {
  aiMetrics = [
    { label: 'lab', value: 98, color: '#6366f1', icon: 'pi pi-flask' },
    { label: 'imaging', value: 94, color: '#10b981', icon: 'pi pi-image' },
    { label: 'general', value: 99, color: '#f59e0b', icon: 'pi pi-heart' }
  ];
}

