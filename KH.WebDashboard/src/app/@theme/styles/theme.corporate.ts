import { NbJSThemeOptions, CORPORATE_THEME as baseTheme } from '@nebular/theme';

const baseThemeVariables = baseTheme.variables;

export const CORPORATE_THEME = {
  name: 'corporate',
  base: 'corporate',
  variables: {
    fontMain: '"Cairo", sans-serif', // Global font
    fontSecondary: '"Cairo", sans-serif', // Secondary font
    cardHeaderTextFontFamily: '"Cairo", sans-serif', // Font specifically for card headers
    // fontSize: baseThemeVariables.fontSize, // Inherit default font sizes
  },
} as NbJSThemeOptions;
