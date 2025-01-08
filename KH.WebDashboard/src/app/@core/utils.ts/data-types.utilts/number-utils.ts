export function convertCommaSeparatedToNumberArray(
  commaSeparated: string
): number[] {
  if (!commaSeparated) {
    return [];
  }

  return commaSeparated
    .split(',')
    .map((item) => {
      const trimmedItem = item.trim();
      return trimmedItem ? Number(trimmedItem) : NaN;
    })
    .filter((item) => !isNaN(item));
}

export function isNumberArray(array: any[]): array is number[] {
  return array.every((item) => typeof item === 'number');
}

export function isNumberWontCheckString(value: any): boolean {
  return typeof value === 'number' && isFinite(value);
}

export function isNumber(value: any): boolean {
  const num = parseFloat(value);
  return typeof num === 'number' && isFinite(num) && !isNaN(num);
}
