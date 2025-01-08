import { NbJSThemeOptions, DEFAULT_THEME as baseTheme } from '@nebular/theme';

const baseThemeVariables = baseTheme.variables;

export const DEFAULT_THEME = {
  name: 'default',
  base: 'default',
  variables: {
    fontMain: '"Cairo", sans-serif', // Global font
    fontSecondary: '"Cairo", sans-serif', // Secondary font
    cardHeaderTextFontFamily: '"Cairo", sans-serif', // Font specifically for card headers
    // fontSize: baseThemeVariables.fontSize, // Inherit default font sizes
    // echarts: {
    //   bg: baseThemeVariables.,
    //   textColor: baseThemeVariables.fgText,
    //   axisLineColor: baseThemeVariables.fgText,
    //   splitLineColor: baseThemeVariables.separator,
    //   itemHoverShadowColor: 'rgba(0, 0, 0, 0.5)',
    //   tooltipBackgroundColor: baseThemeVariables.primary,
    //   areaOpacity: '0.7',
    // },

  },
} as NbJSThemeOptions;
