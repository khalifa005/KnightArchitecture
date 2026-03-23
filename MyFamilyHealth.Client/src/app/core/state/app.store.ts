import { computed, inject } from '@angular/core';
import { signalStore, withState, withComputed, withMethods, patchState } from '@ngrx/signals';
import { TranslocoService } from '@jsverse/transloco';

export interface AppState {
  lang: string;
}

const initialState: AppState = {
  lang: 'en-US'
};

export const AppStore = signalStore(
  { providedIn: 'root' },
  withState(initialState),
  withComputed((store) => ({
    dir: computed(() => (store.lang() === 'ar-SA' ? 'rtl' : 'ltr'))
  })),
  withMethods((store, translocoService = inject(TranslocoService)) => ({
    setLanguage(lang: string) {
      patchState(store, { lang });
      translocoService.setActiveLang(lang);
      document.documentElement.dir = store.dir();
      document.documentElement.lang = lang;
    },
    initialize() {
      // Set the initial direction and language based on initial state
      translocoService.setActiveLang(store.lang());
      document.documentElement.dir = store.dir();
      document.documentElement.lang = store.lang();
    }
  }))
);
