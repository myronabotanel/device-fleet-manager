import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DeviceService, Device } from '../../services/device';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-device-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './device-form.html',
  styleUrl: './device-form.css'
})
export class DeviceForm implements OnInit {
  device: Device = {
    name: '', manufacturer: '', type: 'phone',
    operatingSystem: '', osVersion: '', processor: '',
    ramAmount: 0, description: '', userId: null
  };
  isEditMode = false;
  deviceId: string | null = null;

  constructor(
    private deviceService: DeviceService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.deviceId = this.route.snapshot.paramMap.get('id');
    if (this.deviceId) {
      this.isEditMode = true;
      this.deviceService.getById(this.deviceId).subscribe(data => {
        this.device = data;
      });
    }
  }

  save(): void {
    if (!this.device.name || !this.device.manufacturer || !this.device.operatingSystem ||
        !this.device.osVersion || !this.device.processor || !this.device.description ||
        !this.device.ramAmount) {
      alert('Toate campurile sunt obligatorii!');
      return;
    }

    if (this.isEditMode && this.deviceId) {
      this.deviceService.update(this.deviceId, this.device).subscribe(() => {
        this.router.navigate(['/devices']);
      });
    } else {
      this.deviceService.create(this.device).subscribe(() => {
        this.router.navigate(['/devices']);
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/devices']);
  }
}