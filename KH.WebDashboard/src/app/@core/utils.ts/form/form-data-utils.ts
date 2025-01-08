import { isFileArray } from "../file/file-utils";

export function convertToFormData(dto: any): FormData {
  const formData = new FormData();

  for (const key of Object.keys(dto)) {
    const value = dto[key];
    const typeOfCurrentValue = typeof value;

    if (isFileArray(value)) {
      for (const file of value) {
        formData.append('File', file, file.name);
      }
    }

    if (typeOfCurrentValue === 'number' && !isFileArray(value)) {
      if (value === -1) {
        // value = 'null';
      }
    }

    if (typeOfCurrentValue === 'object' && !isFileArray(value)) {
      if (Array.isArray(value)) {
        formData.append(key, JSON.stringify(value));
      } else if (value !== 'null' && value !== null && value instanceof Date) {
        formData.append(key, value.toDateString());
      } else if (
        value === 'null' ||
        value === null ||
        value === 'undefined'
      ) {
        formData.append(key, '');
      }
    } else if (value === 'null' || value === null || value === 'undefined') {
      formData.append(key, '');
    } else {
      if (!isFileArray(value)) {
        formData.append(key, value);
      }
    }
  }

  return formData;
}
