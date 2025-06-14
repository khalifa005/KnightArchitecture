

// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  elevenLabsApiKey: 'your-elevenlabs-api-key-here',

  apiBaseUrl: 'https://localhost:44312/api/v1',
  // apiBaseUrl: 'https://localhost:5050/api/v1',
  // apiBaseUrlWithoutApiVersion: 'https://localhost:5050',
  apiBaseUrlWithoutApiVersion: 'https://localhost:44312',

  mapApiServer: {
    url: "https://localhost:7299",
    version: "v1"
  },

  defaultLanguage: 'en-US',
  supportedLanguages: ['en-US', 'ar-SA'],
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
