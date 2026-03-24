import { computed, inject } from '@angular/core';
import { signalStore, withState, withComputed, withMethods, patchState } from '@ngrx/signals';
import { TranslocoService } from '@jsverse/transloco';

export interface AppState {
  lang: string;
  darkMode: boolean;
}

const DARK_MODE_KEY = 'dhm-dark-mode';

const initialState: AppState = {
  lang: 'en-US',
  darkMode: false
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
    toggleDarkMode() {
      const newValue = !store.darkMode();
      patchState(store, { darkMode: newValue });
      document.documentElement.classList.toggle('dark', newValue);
      localStorage.setItem(DARK_MODE_KEY, JSON.stringify(newValue));
    },
    initialize() {
      // Restore dark mode preference
      const saved = localStorage.getItem(DARK_MODE_KEY);
      if (saved !== null) {
        const isDark = JSON.parse(saved) === true;
        patchState(store, { darkMode: isDark });
        document.documentElement.classList.toggle('dark', isDark);
      }

      // Set the initial direction and language based on initial state
      translocoService.setActiveLang(store.lang());
      document.documentElement.dir = store.dir();
      document.documentElement.lang = store.lang();
    }
  }))
);
