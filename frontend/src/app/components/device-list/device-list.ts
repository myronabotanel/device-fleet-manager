import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { DeviceService, Device } from '../../services/device';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';


@Component({
  selector: 'app-device-list',
  imports: [CommonModule],
  templateUrl: './device-list.html',
  styleUrl: './device-list.css'
})
export class DeviceList implements OnInit {
  devices: Device[] = [];

  constructor(
    private deviceService: DeviceService,
    private router: Router,
    private authService: AuthService,
    private cdr: ChangeDetectorRef  
  ) {}

  userName = localStorage.getItem('userName') ?? '';
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }


  ngOnInit(): void {
    this.loadDevices();
  }

  loadDevices(): void {
    this.deviceService.getAll().subscribe(data => {
      this.devices = [...data];  
      this.cdr.detectChanges(); 
      console.log('Devices setate:', this.devices);
    });
  }

  viewDetail(id: string): void {
    this.router.navigate(['/devices', id]);
  }

  addNew(): void {
    this.router.navigate(['/devices/new']);
  }

  deleteDevice(id: string): void {
    if (confirm('Chiar vrei sa stergi acest device?')) {
      this.deviceService.delete(id).subscribe(() => {
        this.loadDevices();
      });
    }
  }
}