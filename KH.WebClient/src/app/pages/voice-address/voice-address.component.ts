import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Component({
  selector: 'app-voice-address',
  templateUrl: './voice-address.component.html',
  styleUrls: ['./voice-address.component.css']
})
export class VoiceAddressComponent implements OnInit {
  addressForm!: FormGroup;
  private hubConnection?: HubConnection;
  audioUrl: string | null = null;
  mediaRecorder?: MediaRecorder;
  chunks: Blob[] = [];

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.addressForm = this.fb.group({
      street: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      zip: ['', Validators.required]
    });
    this.setupSignalR();
  }

  setupSignalR() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('/signalrSpeechHub')
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(err => console.error(err));

    this.hubConnection.on('TranscriptionResult', data => {
      this.addressForm.patchValue(data);
    });

    this.hubConnection.on('TtsReady', url => {
      this.audioUrl = url;
      const audio = new Audio(url);
      audio.play();
    });
  }

  record() {
    navigator.mediaDevices.getUserMedia({ audio: true }).then(stream => {
      this.chunks = [];
      this.mediaRecorder = new MediaRecorder(stream);
      this.mediaRecorder.ondataavailable = e => this.chunks.push(e.data);
      this.mediaRecorder.onstop = () => {
        const blob = new Blob(this.chunks, { type: 'audio/webm' });
        const formData = new FormData();
        formData.append('file', blob, 'recording.webm');
        fetch('/api/speech/upload', { method: 'POST', body: formData });
      };
      this.mediaRecorder.start();
      setTimeout(() => this.mediaRecorder?.stop(), 5000);
    });
  }
}
