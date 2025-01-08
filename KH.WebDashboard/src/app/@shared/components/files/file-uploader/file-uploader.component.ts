import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.scss']
})
export class FileUploaderComponent implements OnInit {
  @ViewChild('fileInput', { static: true }) fileInput!: ElementRef<HTMLInputElement>;
  @ViewChild('dropzone', { static: true }) dropzone!: ElementRef<HTMLDivElement>;

  @Input() requiredFileType: string = '';
  @Input() maxFileSizeInMB : string = '';
  @Input() maxFiles  : number = 1;
  @Input() multiple: boolean = true;
  @Input() formcontrol!: FormControl;
  @Input() isRequired: boolean = false;
  @Input() disabled: boolean = false;
  @Input() label: boolean = false;
  @Input() preUploadedFiles: { name: string, url: string }[] = []; // For managing previously uploaded files

  @Output() fileDroppedEmitter = new EventEmitter<any>();
  @Output() PreUploadedFileDroppedEmitter = new EventEmitter<any>();


  // files: File[] = [];
  files: any[] = [];

  constructor(private renderer: Renderer2) { }

  ngOnInit() {
    this.dropzone.nativeElement.addEventListener('dragover', this.onDragOver.bind(this), false);
    this.dropzone.nativeElement.addEventListener('drop', this.onDrop.bind(this), false);
    this.dropzone.nativeElement.addEventListener('dragleave', this.onDragLeave.bind(this), false);

    // Initialize pre-uploaded files
  if (this.preUploadedFiles?.length) {
    this.preUploadedFiles = this.preUploadedFiles.map(file => ({
      ...file,
      isPreUploaded: true,
      progress: 100,
    }));
  }

  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    this.renderer.addClass(this.dropzone.nativeElement, 'is-dragover');
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    this.renderer.removeClass(this.dropzone.nativeElement, 'is-dragover');
    if (event.dataTransfer?.files) {
      this.onFileDropped(Array.from(event.dataTransfer.files));
    }
  }

  onFileDropped(files: File[]) {
    this.handleFiles(files);
  }

  private handleFiles(fileList: FileList | File[]) {

    if(!this.canUploadMoreFiles()){
      return;
    }
    
    const newFiles = Array.from(fileList).map(file => ({
      file,
      progress: 0, // Initialize progress
    }));

    // Avoid duplicates by comparing file names in both lists
  const uniqueFiles = newFiles.filter(
    newFile =>
      // this.canUploadMoreFiles() &&
      !this.files.some(f => f.file.name === newFile.file.name) &&
      !this.preUploadedFiles.some(f => f.name === newFile.file.name)
  );
  this.files = [...this.files, ...uniqueFiles];


    // this.files = [...this.files, ...newFiles];

    // this.fileDroppedEmitter.emit(this.files);
    // Emit only the file objects
    this.fileDroppedEmitter.emit(this.files.map(fileObj => fileObj.file));

    // Start the upload simulation
    this.uploadFilesSimulator(0);

    if (this.formcontrol) {
      this.formcontrol.markAsTouched();
      this.formcontrol.patchValue(this.files.map(f => f.file));
    }

  }

  uploadFilesSimulator(index: number) {
    if (index >= this.files.length) {
      return; // Stop if index is out of bounds
    }

    const file = this.files[index];
    const progressInterval = setInterval(() => {
      if (file.progress >= 100) {
        clearInterval(progressInterval);
        this.uploadFilesSimulator(index + 1); // Move to the next file
      } else {
        file.progress += 10; // Simulate progress increment
      }
    }, 100);
  }


  onDragLeave(event: DragEvent) {
    event.preventDefault();
    this.renderer.removeClass(this.dropzone.nativeElement, 'is-dragover');
  }



  onBrowseFiles() {
    this.fileInput.nativeElement.click();

    if (this.formcontrol) {
      this.formcontrol.markAsTouched();
    }
  }

  fileBrowseHandler(files: FileList) {
    // this.handleFiles(files);
    this.handleFiles(Array.from(files));
    // Reset file input to allow re-upload of the same file
    this.fileInput.nativeElement.value = '';
  }



  deleteFile(index: number) {
    this.files.splice(index, 1);

    if (this.formcontrol) {
      this.formcontrol.patchValue(this.files.map(f => f.file));
    }

    // this.fileDroppedEmitter.emit(this.files);
    // Emit only the file objects
    this.fileDroppedEmitter.emit(this.files.map(fileObj => fileObj.file));
  }

  public formatBytes(bytes: number, decimals: number = 2): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const dm = Math.max(0, decimals);
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
  }


  deletePreUploadedFile(index: number) {
    const fileToDelete = this.preUploadedFiles[index];
    this.preUploadedFiles.splice(index, 1);
  
    // Emit event for server-side deletion
    this.PreUploadedFileDroppedEmitter.emit({ type: 'delete', file: fileToDelete });
  
    if (this.formcontrol) {
      this.formcontrol.markAsTouched();
    }
  }


  canUploadMoreFiles(): boolean {
    return this.preUploadedFiles.length + this.files.length < this.maxFiles;
  }

}