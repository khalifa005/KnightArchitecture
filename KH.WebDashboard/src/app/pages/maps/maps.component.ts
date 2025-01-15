import { Component } from '@angular/core';

@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
  styleUrl: './maps.component.scss'
})
export class MapsComponent {
title = 'Angular Google Maps Integration';
  center: google.maps.LatLngLiteral = { lat: 24.7136, lng: 46.6753 }; // Coordinates for Riyadh, Saudi Arabia
  zoom = 12;



  markerPosition: google.maps.LatLngLiteral = { lat: 24.7136, lng: 46.6753 };
  readonly position = { lat: 51.678418, lng: 7.809007 };
}
