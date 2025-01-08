export function isFileArray(data: any[]): boolean {
  return Array.isArray(data) && data.every((value) => value instanceof File);
}

export function onFileUploadValidationSize(
  event: HTMLInputEvent,
  size: number
): boolean {
  if (event.target.files!.length > 0) {
    const target = event.target as HTMLInputElement;
    const files = target.files as FileList;
    return files[0].size < size;
  }
  return false;
}

export function calculatedFileSizeInKB(sizeInMegaByte: number): number {
  const KB_TO_MB = 1048576;
  return sizeInMegaByte * KB_TO_MB;
}

export interface HTMLInputEvent extends Event {
  target: HTMLInputElement & EventTarget;
}
