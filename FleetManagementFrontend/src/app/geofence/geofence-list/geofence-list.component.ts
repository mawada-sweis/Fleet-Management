import { Component, OnInit } from '@angular/core';
import { GeofenceService } from '../../services/geofence.service';
import {
  CircleGeofence,
  Geofence,
  PolygonGeofence,
  RectangleGeofence,
} from '../geofence.model';
import { MatDialog } from '@angular/material/dialog';
import { GeofenceDetailDialogComponent } from '../geofence-detail-dialog/geofence-detail-dialog.component';

@Component({
  selector: 'app-geofence-list',
  templateUrl: './geofence-list.component.html',
  styleUrls: ['./geofence-list.component.css'],
})
export class GeofenceListComponent implements OnInit {
  geofences: Geofence[] = [];
  circleGeofences: CircleGeofence[] = [];
  polygonGeofences: PolygonGeofence[] = [];
  rectangleGeofences: RectangleGeofence[] = [];
  errorMessage: string = '';

  constructor(
    private geofenceService: GeofenceService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadGeofences();
  }

  loadGeofences(): void {
    this.geofenceService.getGeofences().subscribe(
      (response) => {
        if (response && response.DicOfDT && response.DicOfDT.Geofences) {
          this.geofences = response.DicOfDT.Geofences.map((geofence) => ({
            ...geofence,
            AddedDate: this.convertEpochToDatetime(geofence.AddedDate),
            StrokeColor: geofence.StrockColor,
            StrokeOpacity: geofence.StrockOpacity,
            StrokeWeight: geofence.StrockWeight,
          }));
        }
      },
      (error) => {
        console.error('Error loading geofences:', error);
        this.errorMessage = 'Failed to load geofences';
      }
    );
    this.geofenceService.getCircleGeofence().subscribe(
      (response) => (this.circleGeofences = response.DicOfDT.CircleGeofence),
      (error) => (this.errorMessage = 'Failed to load circle geofences.')
    );

    this.geofenceService.getPolygonGeofence().subscribe(
      (response) => (this.polygonGeofences = response.DicOfDT.PolygonGeofence),
      (error) => (this.errorMessage = 'Failed to load polygon geofences.')
    );

    this.geofenceService.getRectangleGeofence().subscribe(
      (response) =>
        (this.rectangleGeofences = response.DicOfDT.RectangleGeofence),
      (error) => (this.errorMessage = 'Failed to load rectangle geofences.')
    );
  }

  convertEpochToDatetime(epoch: string): string {
    const date = new Date(parseInt(epoch, 10) * 1000);

    const year = new Intl.DateTimeFormat('en', {
      year: 'numeric',
      timeZone: 'UTC',
    }).format(date);
    const month = new Intl.DateTimeFormat('en', {
      month: '2-digit',
      timeZone: 'UTC',
    }).format(date);
    const day = new Intl.DateTimeFormat('en', {
      day: '2-digit',
      timeZone: 'UTC',
    }).format(date);
    const hour = new Intl.DateTimeFormat('en', {
      hour: '2-digit',
      hour12: false,
      timeZone: 'UTC',
    }).format(date);
    const minute = new Intl.DateTimeFormat('en', {
      minute: '2-digit',
      timeZone: 'UTC',
    }).format(date);
    const second = new Intl.DateTimeFormat('en', {
      second: '2-digit',
      timeZone: 'UTC',
    }).format(date);

    return `${year}-${month}-${day} ${hour}:${minute}:${second}`;
  }
  showMore(geofence: Geofence): void {
    let geofenceDetails;

    switch (geofence.GeofenceType) {
      case 'Circle':
        geofenceDetails = this.circleGeofences.find(
          (g) => g.GeofenceID === geofence.GeofenceID
        );
        break;
      case 'Polygon':
        geofenceDetails = this.polygonGeofences.find(
          (g) => g.GeofenceID === geofence.GeofenceID
        );
        break;
      case 'Rectangle':
        geofenceDetails = this.rectangleGeofences.find(
          (g) => g.GeofenceID === geofence.GeofenceID
        );
        break;
    }

    if (geofenceDetails) {
      this.dialog.open(GeofenceDetailDialogComponent, {
        data: geofenceDetails,
      });
    }
  }
}
