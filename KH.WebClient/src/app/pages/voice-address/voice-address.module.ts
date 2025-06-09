import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { VoiceAddressComponent } from './voice-address.component';

const routes: Routes = [
  { path: '', component: VoiceAddressComponent }
];

@NgModule({
  declarations: [VoiceAddressComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes)
  ]
})
export class VoiceAddressModule {}
