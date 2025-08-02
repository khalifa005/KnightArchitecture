/**
 * Convert to RMB yuan string
 *
 * @param digits When the value is a number type, you can specify the number of decimal places, default is 2 decimal places
 */
export function yuan(value: number | string, digits = 2): string {
  if (typeof value === 'number') {
    value = value.toFixed(digits);
  }
  return `&yen ${value}`;
}
