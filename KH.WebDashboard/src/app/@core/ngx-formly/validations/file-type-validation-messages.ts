import { FormlyFieldConfig } from "@ngx-formly/core";
import { FileExtensionError } from "../formly-file-extentions/file-extension-error";
import { FilenameForbiddenCharactersError } from "../formly-file-extentions/filename-forbidden-characters-error";
import { FilesizeError } from "../formly-file-extentions/filesize-error";
import { MaxFilenameLengthError } from "../formly-file-extentions/max-filename-length-error";
import { MaxFilesError } from "../formly-file-extentions/max-files-error";
import { MinFilenameLengthError } from "../formly-file-extentions/min-filename-length-error";
import { MinFilesError } from "../formly-file-extentions/min-files-error";
import { TotalFilesizeError } from "../formly-file-extentions/total-filesize-error";
import { FileSizePipe } from "../pipes/file-size.pipe";


export class FileTypeValidationMessages {

  private readonly fileSizePipe: FileSizePipe;

  constructor(localId: string) {
    this.fileSizePipe = new FileSizePipe(localId);
  }

  get validationMessages(): {
    name: string;
    message: string | ((error: any, field: FormlyFieldConfig) => string);
  }[] {

    return [
      {
        name: 'maxFilenameLength', message: (err: MaxFilenameLengthError) => {
          return `The filename is too long. Max length: ${err.maxFilenameLength}`;
        }
      },
      {
        name: 'minFilenameLength', message: (err: MinFilenameLengthError) => {
          return `The filename is too short. Min length: ${err.minFilenameLength}`;
        }
      },
      {
        name: 'fileExtension', message: (err: FileExtensionError) => {
          const allowedFileExtensions = err.allowedFileExtensions
            .map(ext => `'${ext}'`)
            .join(', ');
          return `The file extension '${err.actualFileExtension}' is forbidden. Allowed extensions are: ${allowedFileExtensions}`;
        }
      },
      {
        name: 'filesize', message: (err: FilesizeError) => {
          return `The file is too big. Max filesize: ${this.fileSizePipe.transform(err.maxFilesize)}`;
        }
      },
      {
        name: 'filenameForbiddenCharacters', message: (err: FilenameForbiddenCharactersError) => {
          const actualForbiddenCharacters = err.actualForbiddenCharacters
          .map(char => `'${char}'`)
          .join(', ');
          return `The filename contains forbidden characters: ${actualForbiddenCharacters}`;
        }
      },
      {
        name: 'minFiles', message: (err: MinFilesError) => {
          return `Select at minimum ${err.minFiles} files`;
        }
      },
      {
        name: 'maxFiles', message: (err: MaxFilesError) => {
          return `Select at maximum ${err.maxFiles} files`;
        }
      },
      {
        name: 'totalFilesize', message: (err: TotalFilesizeError) => {
          return `The files are too big. Max total filesize: ${this.fileSizePipe.transform(err.maxTotalFilesize)}`;
        }
      },
      {
        name: 'uploadError', message: () => {
          return 'The file could not be uploaded';
        }
      },
    ];
  }

}
