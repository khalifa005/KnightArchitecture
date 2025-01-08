import { Inject, LOCALE_ID, Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'fileSize' })
export class FileSizePipe implements PipeTransform {

  private static readonly OPTS = { maximumFractionDigits: 1 };

  private static readonly KILO = 1000;

  constructor(@Inject(LOCALE_ID) private readonly localeId: string) { }

  transform(bytes: number): string {
    if (bytes < FileSizePipe.KILO) {
      return `${bytes} B`;
    }

    const kilobytes = bytes / FileSizePipe.KILO;

    if (kilobytes < FileSizePipe.KILO) {
      return `${kilobytes.toLocaleString(this.localeId, FileSizePipe.OPTS)} kB`;
    }

    const megabytes = kilobytes / FileSizePipe.KILO;

    if (megabytes < FileSizePipe.KILO) {
      return `${megabytes.toLocaleString(this.localeId, FileSizePipe.OPTS)} MB`;
    }

    const gigabytes = megabytes / FileSizePipe.KILO;

    return `${gigabytes.toLocaleString(this.localeId, FileSizePipe.OPTS)} GB`;
  }

}
