import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { default as ngLang } from '@angular/common/locales/ar';
import { ApplicationConfig, EnvironmentProviders, Provider } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import {
  provideRouter,
  withComponentInputBinding,
  withInMemoryScrolling,
  withHashLocation,
  RouterFeatures,
  withViewTransitions
} from '@angular/router';
import { I18NService, defaultInterceptor, provideBindAuthRefresh, provideStartup } from '@core';
import { provideCellWidgets } from '@delon/abc/cell';
import { provideSTWidgets } from '@delon/abc/st';
import { authJWTInterceptor, authSimpleInterceptor, provideAuth, withLocalStorage } from '@delon/auth';
import { provideSFConfig } from '@delon/form';
import { AlainProvideLang, provideAlain, en_US as delonLang } from '@delon/theme';
import { AlainAuthConfig, AlainConfig } from '@delon/util/config';
import { environment } from '@env/environment';
import { CELL_WIDGETS, SF_WIDGETS, ST_WIDGETS } from '@shared';
import { ar as dateLang } from 'date-fns/locale';
import { NzConfig, provideNzConfig } from 'ng-zorro-antd/core/config';
import { ar_EG as zorroLang } from 'ng-zorro-antd/i18n';

import { ICONS } from '../style-icons';
import { ICONS_AUTO } from '../style-icons-auto';
import { routes } from './routes/routes';
import { Configuration } from 'src/app/shared/open-api';

const defaultLang: AlainProvideLang = {
  abbr: 'ar',
  ng: ngLang,
  zorro: zorroLang,
  date: dateLang,
  delon: delonLang
};

const alainConfig: AlainConfig = {
  st: { modal: { size: 'lg' } },
  pageHeader: { homeI18n: 'home' },
  lodop: {
    license: `A59B099A586B3851E0F0D7FDBF37B603`,
    licenseA: `C94CEE276DB2187AE6B65D56B3FC2848`
  },
  auth: { login_url: '/passport/login' }
};

const ngZorroConfig: NzConfig = {};

const alainAuthConfig: AlainAuthConfig = {};

const routerFeatures: RouterFeatures[] = [
  withComponentInputBinding(),
  withViewTransitions(),
  withInMemoryScrolling({ scrollPositionRestoration: 'top' })
];

if (environment.useHash) routerFeatures.push(withHashLocation());

const providers: Array<Provider | EnvironmentProviders> = [
  {
    provide: Configuration,
    useValue: new Configuration({
      basePath: environment.api.serverUrl || 'https://localhost:5050'
    })
  },
  
  // provideHttpClient(withInterceptors([...(environment.interceptorFns ?? []), authSimpleInterceptor, defaultInterceptor])),
  provideHttpClient(withInterceptors([...(environment.interceptorFns ?? []), authJWTInterceptor, defaultInterceptor])),
  provideAnimations(),
  provideRouter(routes, ...routerFeatures),
  provideAlain({ config: alainConfig, defaultLang, i18nClass: I18NService, icons: [...ICONS_AUTO, ...ICONS] }),
  provideNzConfig(ngZorroConfig),
  
  provideAuth(),
  // provideAuth(withLocalStorage()), sessionStorage / cookie 

  provideCellWidgets(...CELL_WIDGETS),
  provideSTWidgets(...ST_WIDGETS),
  provideSFConfig({ widgets: SF_WIDGETS }),
  provideStartup(),
  ...(environment.providers || [])
];

// If you use `@delon/auth` to refresh the token, additional registration `provideBindAuthRefresh` is required
if (environment.api?.refreshTokenEnabled && environment.api.refreshTokenType === 'auth-refresh') {
  providers.push(provideBindAuthRefresh());
}

export const appConfig: ApplicationConfig = {
  providers: providers
};
