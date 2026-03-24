import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@jsverse/transloco';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { RippleModule } from 'primeng/ripple';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-help-center',
  standalone: true,
  imports: [
    CommonModule, 
    TranslocoModule, 
    InputTextModule, 
    ButtonModule, 
    CardModule, 
    IconFieldModule, 
    InputIconModule,
    RippleModule,
    RouterLink
  ],
  template: `
    <div class="help-center-container p-4 lg:p-6" *transloco="let t; read: 'help-center'">
      <!-- Hero Section -->
      <section class="help-hero text-center mb-8 py-8 md:py-12 bg-primary-reverse border-round-xl">
        <h1 class="text-4xl md:text-5xl font-bold text-900 mb-3">{{ t('title') }}</h1>
        <p class="text-xl text-600 mb-6">{{ t('subtitle') }}</p>
        
        <div class="max-w-30rem mx-auto px-4">
          <p-iconField iconPosition="left">
            <p-inputIcon class="pi pi-search"></p-inputIcon>
            <input type="text" pInputText [placeholder]="t('search_placeholder')" class="w-full p-inputtext-lg" />
          </p-iconField>
        </div>
      </section>

      <!-- Categories Grid -->
      <div class="grid mb-8">
        @for (cat of categories; track cat.key) {
          <div class="col-12 md:col-6 lg:col-4 p-3">
            <div pRipple class="category-card p-4 border-1 border-200 border-round-xl cursor-pointer hover:border-primary transition-colors h-full flex flex-column align-items-center text-center">
              <div class="category-icon mb-3 flex align-items-center justify-content-center border-round-circle bg-primary-100 text-primary p-3" style="width: 64px; height: 64px">
                <i class="pi" [ngClass]="cat.icon" style="font-size: 2rem"></i>
              </div>
              <h3 class="text-xl font-bold text-900 mb-2">{{ t('categories.' + cat.key + '.title') }}</h3>
              <p class="text-600 line-height-3">{{ t('categories.' + cat.key + '.desc') }}</p>
            </div>
          </div>
        }
      </div>

      <!-- Featured Articles -->
      <section class="featured-articles mb-8">
        <h2 class="text-2xl font-bold text-900 mb-4">{{ t('featured.title') }}</h2>
        <div class="grid">
          @for (i of [1,2,3,4]; track i) {
            <div class="col-12 md:col-6 p-2">
              <a class="article-link block p-3 border-round-lg hover:bg-emphasis no-underline text-900 flex align-items-center justify-content-between">
                <span class="flex align-items-center">
                  <i class="pi pi-file text-primary mr-3"></i>
                  {{ t('featured.article' + i) }}
                </span>
                <i class="pi pi-chevron-right text-400 text-sm"></i>
              </a>
            </div>
          }
        </div>
      </section>

      <!-- Contact CTA -->
      <div class="contact-cta p-6 border-round-xl bg-primary text-white text-center">
        <h2 class="text-3xl font-bold mb-3">{{ t('still_need_help.title') }}</h2>
        <p class="text-xl opacity-80 mb-5">{{ t('still_need_help.desc') }}</p>
        <p-button [label]="t('still_need_help.contact_btn')" routerLink="/contact-us" severity="secondary" rounded="true" size="large" icon="pi pi-envelope"></p-button>
      </div>
    </div>
  `,
  styles: [`
    .help-hero {
      background: linear-gradient(135deg, var(--primary-50) 0%, #ffffff 100%);
    }
    .category-card {
      transition: all 0.2s ease-in-out;
    }
    .category-card:hover {
      transform: translateY(-4px);
      box-shadow: 0 10px 20px rgba(0,0,0,0.05);
    }
    .article-link {
      transition: background 0.2s;
    }
    :host ::ng-deep .p-inputtext {
      border-radius: 12px;
    }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HelpCenterComponent {
  categories = [
    { key: 'getting_started', icon: 'pi-rocket' },
    { key: 'my_records', icon: 'pi-folder-open' },
    { key: 'insurance', icon: 'pi-id-card' },
    { key: 'ai_assistant', icon: 'pi-sparkles' },
    { key: 'privacy', icon: 'pi-shield' }
  ];
}

