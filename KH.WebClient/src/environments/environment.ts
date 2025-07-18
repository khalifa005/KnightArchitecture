// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import * as MOCKDATA from '@_mock';
import { mockInterceptor, provideMockConfig } from '@delon/mock';
import { Environment } from '@delon/theme';
import { Environment as AlainEnvironment } from '@delon/theme';

export const environment = {
  production: false,
  useHash: true,
  api: {
    serverUrl: 'http://localhost:5000',

    baseUrl: './',
    refreshTokenEnabled: true,
    refreshTokenType: 'auth-refresh'
  },
  providers: [provideMockConfig({ data: MOCKDATA })],
  interceptorFns: [mockInterceptor]
} as MyEnvironment;


export interface MyEnvironment extends AlainEnvironment {
  api: AlainEnvironment['api'] & {
    serverUrl: string;
  };
}