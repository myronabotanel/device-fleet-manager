import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { DeviceService, Device } from '../../services/device';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-device-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './device-list.html',
  styleUrl: './device-list.css'
})
export class DeviceList implements OnInit {
  devices: Device[] = [];
  userName = localStorage.getItem('userName') ?? '';
  currentUserId = localStorage.getItem('userId') ?? '';
  searchQuery = '';

  constructor(
    private deviceService: DeviceService,
    private router: Router,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadDevices();
  }

  loadDevices(): void {
    this.deviceService.getAll().subscribe(data => {
      this.devices = [...data];
      this.cdr.detectChanges();
    });
  }

  search(): void {
    if (!this.searchQuery.trim()) {
      this.loadDevices();
      return;
    }
    this.deviceService.search(this.searchQuery).subscribe(data => {
      this.devices = [...data];
      this.cdr.detectChanges();
    });
  }

  clearSearch(): void {
    this.searchQuery = '';
    this.loadDevices();
  }

  viewDetail(id: string): void { this.router.navigate(['/devices', id]); }
  addNew(): void { this.router.navigate(['/devices/new']); }

  deleteDevice(id: string): void {
    if (confirm('Chiar vrei sa stergi acest device?')) {
      this.deviceService.delete(id).subscribe(() => this.loadDevices());
    }
  }

  assignDevice(id: string): void {
    this.deviceService.assign(id).subscribe(() => this.loadDevices());
  }

  unassignDevice(id: string): void {
    this.deviceService.unassign(id).subscribe(() => this.loadDevices());
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}