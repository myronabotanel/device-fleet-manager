import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; 
import { ActivatedRoute, Router } from '@angular/router';
import { DeviceService, Device } from '../../services/device';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-device-detail',
  imports: [CommonModule],
  templateUrl: './device-detail.html',
  styleUrl: './device-detail.css'   
})
export class DeviceDetail implements OnInit {
  device: Device | null = null;
  assignedUserName: string | null = null;  

  constructor(
    private deviceService: DeviceService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef  
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.deviceService.getById(id).subscribe(data => {
        this.device = data;

        // Daca e asignat, fetch numele userului
        if (data.userId) {
          this.deviceService.getUserById(data.userId).subscribe(user => {
            this.assignedUserName = user.name;
            this.cdr.detectChanges();
          });
        }

        this.cdr.detectChanges();
      });
    }
  }

  edit(): void {
    this.router.navigate(['/devices', this.device?.id, 'edit']);
  }

  back(): void {
    this.router.navigate(['/devices']);
  }
}